using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class TarotSelect : MonoBehaviour
{
    [SerializeField]
    private TMP_Text title;
    [SerializeField]
    private TMP_Text description;
    [SerializeField] 
    private TMP_Text isActive;

    [SerializeField]
    private Animator carouselAnimator;

    [SerializeField]
    private Animator tarotWheel;

    [SerializeField]
    private List<GameObject> visibleCarousel = new List<GameObject>();

    [SerializeField]
    private List<TarotCardCarousel> carousel = new List<TarotCardCarousel>();

    [SerializeField]
    private List<TarotCard> cardsSelected = new List<TarotCard>();

    [SerializeField]
    private List<TarotCard> cardsNotSelected = new List<TarotCard>() { TarotCard.Justice, TarotCard.Devil, TarotCard.Fool, TarotCard.Hermit,
                                                                        TarotCard.Lovers, TarotCard.Magician};

    private int top;
    private int bottom;
    private int viewCardIndex;

    [SerializeField]
    private int MaxNumOfCards;

    public enum TarotCard
    {
        Justice,
        Devil,
        Fool,
        Hermit,
        Lovers,
        Magician
    }

    public enum CharacterAssociation
    { 
        Aries,
        Pisces,
        Aquarius,
        Capricorn,
        Sagittarius,
        Scorpio,
        Libra,
        Virgo,
        Leo,
        Cancer,
        Gemini,
        Taurus   
    }

    [Serializable]
    public struct TarotCardCarousel
    {
        public TarotCard card;
        public Sprite tarotSprite;
        public bool isActive;
        public string title;
        public string description;
        public CharacterAssociation associatedCharacter;
    }


    void Start()
    {
        top = 0;
        bottom = (top + visibleCarousel.Count - 1) % carousel.Count;
    }

    public void MoveCarousel(int direction)
    {
        //Debug.Log("Moving in " + direction);
        if (direction < 0 && top % carousel.Count == 0)
            top = carousel.Count - 1;
        else
            top = (top + direction) % carousel.Count;

        if (direction < 0 && bottom % carousel.Count == 0)
            bottom = (top + visibleCarousel.Count - 1) % carousel.Count;
        else
            bottom = (bottom + direction) % carousel.Count;

        if (bottom < top)
            viewCardIndex = ((top + bottom + carousel.Count) / 2) % carousel.Count;
        else
            viewCardIndex = ((top + bottom) / 2) % carousel.Count;

        if (direction > 0)
            carouselAnimator.SetBool("MovedUp", true);
        else
            carouselAnimator.SetBool("MovedDown", true);

        RotateWheel(carousel[viewCardIndex].associatedCharacter);
    }

    public void ReplaceImages()
    {
        //Debug.Log("Replacing Images");
        int top_copy = top;
        // Debug.Log("Bottom " + bottom);
        // Debug.Log("VIEW CARD INDEX " + viewCardIndex);
        foreach (GameObject GO in visibleCarousel) 
        {
            //Debug.Log(top_copy);
            Image currentImage = GO.GetComponent<Image>();
            currentImage.sprite = carousel[top_copy].tarotSprite;
            if (viewCardIndex == top_copy)
                ChangeText(top_copy);
            top_copy = (top_copy + 1) % carousel.Count;
        }        

        ResetSortingOrder();
    }


    public void ChangeText(int index)
    {
        title.text = carousel[index].title;
        description.text = carousel[index].description;
        isActive.text = carousel[index].isActive ? "Yes" : "No";
    }

    public void ChangeSortingOrder(int direction)
    {
        //Debug.Log("CHAGING ORDER " + direction);
        int start = 0;
        int end = visibleCarousel.Count - 2;
        bool flipped_sign = false;

        if (direction < 0)
        {
            start = 1;
            end = visibleCarousel.Count - 1;
        }
        /*Debug.Log("START " + start);
        Debug.Log("END " + end);*/
        for (int i = start; i <= end; ++i)
        {
            if ((i > (start + end) / 2) && !flipped_sign)
            {
                flipped_sign = true;
                direction *= -1; //reverse direction
            }
            //Debug.Log("Direction " + direction + ", " + i);
            Canvas canvas = visibleCarousel[i].GetComponent<Canvas>();
            canvas.sortingOrder += direction;
        }


        carouselAnimator.SetBool("MovedUp", false);
        carouselAnimator.SetBool("MovedDown", false);
    }

    public void ResetSortingOrder()
    {
        //Debug.Log("SORTING ORDER");
        int max_order = 7;
        int mid = visibleCarousel.Count / 2;

        visibleCarousel[mid].GetComponent<Canvas>().sortingOrder = max_order;

        for (int i = mid - 1; i >= 0; --i)
        {
            
            Canvas canvas = visibleCarousel[i].GetComponent<Canvas>();
            canvas.sortingOrder = max_order - (mid - i);
        }

        for (int i = mid + 1; i < visibleCarousel.Count; ++i)
        {
            Canvas canvas = visibleCarousel[i].GetComponent<Canvas>();
            canvas.sortingOrder = max_order - (i - mid);
        }

    }

    public void SaveCard()
    {
        if (cardsSelected.Count >= MaxNumOfCards)
        {
            Debug.Log("You reached the card limit");
            return;
        }

        TarotCardCarousel tarotCardInfo = carousel[viewCardIndex];
        tarotCardInfo.isActive = !tarotCardInfo.isActive;
        carousel[viewCardIndex] = tarotCardInfo;
        isActive.text = carousel[viewCardIndex].isActive ? "Yes" : "No";

        if (carousel[viewCardIndex].isActive && !cardsSelected.Contains(carousel[viewCardIndex].card))
        {
            cardsSelected.Add(carousel[viewCardIndex].card);
            cardsNotSelected.Remove(carousel[viewCardIndex].card);
        }
        else if (!carousel[viewCardIndex].isActive)
        {
            cardsSelected.Remove(carousel[viewCardIndex].card);
            cardsNotSelected.Add(carousel[viewCardIndex].card);
        }
    }

    public void SaveSelectedCards()
    {
        Hashtable hash = new Hashtable();
        string baseName = "TarotCards";
        string[] cards = new string[MaxNumOfCards];
        
        for (int i = 0; i < cardsSelected.Count; ++i)
        {
            cards[i] = cardsSelected[i].ToString();
        }

        hash.Add(baseName, cards);

        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void SaveCards()
    {
        System.Random random = new System.Random();

        for(int i = cardsSelected.Count; i < MaxNumOfCards; ++i)
        {
            TarotCard card = cardsNotSelected[random.Next(0, cardsNotSelected.Count)];
            cardsSelected.Add(card);
            cardsNotSelected.Remove(card);
        }

        SaveSelectedCards();
    }


    public void LeaveTarotSelect()
    { 
        gameObject.SetActive(false);
    }


    public void RotateWheel(CharacterAssociation character)
    {
        //Debug.Log("Rotating Wheel");
        Debug.Log((int)character);
        tarotWheel.SetInteger("Name", (int)character);   
    }

}
