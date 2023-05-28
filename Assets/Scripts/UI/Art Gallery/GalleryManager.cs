using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GalleryManager : MonoBehaviour
{
    [Serializable]
    public struct ArtPiece
    {
        public Sprite sprite;
        public string artName;
        public string artist;
    }

    [SerializeField] ArtPiece[] artPieces;
    [SerializeField] Image artViewImage;
    [SerializeField] TMP_Text description;
    int current;


    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        UpdateArtView();
    }

    void UpdateArtView()
    {
        ArtPiece art = artPieces[current];
        artViewImage.sprite = art.sprite;
        description.text = $"{art.artName} - {art.artist}";
    }

    public void OnClickLeft()
    {
        current = current != 0 ? (current - 1) % artPieces.Length : artPieces.Length - 1;
        UpdateArtView();
    }

    public void OnClickRight()
    {
        current = (current + 1) % artPieces.Length;
        UpdateArtView();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
