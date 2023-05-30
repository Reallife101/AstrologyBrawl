using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverImage : MonoBehaviour
{
    [SerializeField] private List<Image> gameOverImages;
    public Sprite loserImage;
    public Sprite winnerImage;

    private void Awake()
    {
        foreach(Image I in gameOverImages)
        {
            I.sprite = loserImage;
        }
    }

    public void SetWinnerImage()
    {
        foreach (Image I in gameOverImages)
        {
            I.sprite = winnerImage;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
