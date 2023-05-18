using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI start_text;
    [SerializeField]
    Image Larrow;
    [SerializeField]
    Image Rarrow;

    [SerializeField]
    TextMeshProUGUI leave_text;

    [SerializeField] List<Sprite> splashArts;
    [SerializeField] private Image LeftSplash;
    [SerializeField] private Image RightSplash;

    private PlayerControllerInputAsset input;

    private InputAction startGame;
    private InputAction quitGame;

    [SerializeField]
    private float duration;

    private bool fade;

    private float alpha_value = 1f;


    private void Awake()
    {
        start_text.canvasRenderer.SetAlpha(0f);
        input = new PlayerControllerInputAsset();
        startGame = input.Title.Start;
        quitGame = input.Title.Quit;


        startGame.started += nextScene =>
        {
            Debug.Log("Loading Next Level");
            LoadScene();
        };

        quitGame.performed += quit =>
        {
            Debug.Log("QUITTING");
            Application.Quit();
        };

        startGame.Enable();
        quitGame.Enable();

        int rightIndex = Random.Range(0, splashArts.Count);
        RightSplash.sprite = splashArts[rightIndex];
        splashArts.Remove(splashArts[rightIndex]);
        int leftIndex = Random.Range(0, splashArts.Count);
        LeftSplash.sprite = splashArts[leftIndex];
        splashArts.Remove(splashArts[leftIndex]);


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetJoystickNames().Length >= 1 && Input.GetJoystickNames()[0] != "")
        {
            leave_text.text = "Hold B to quit";
        }
        else
        {
            leave_text.text = "Hold q to quit";
        }


        if (fade) 
        {
            if (Input.anyKeyDown)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            
            start_text.CrossFadeAlpha(alpha_value, duration, false);

            if (start_text.canvasRenderer.GetAlpha() > .9)
                alpha_value = 0f;
            else if (start_text.canvasRenderer.GetAlpha() < 0.1)
                alpha_value = 1f;

        }

        Larrow.canvasRenderer.SetAlpha(start_text.canvasRenderer.GetAlpha());
        Rarrow.canvasRenderer.SetAlpha(start_text.canvasRenderer.GetAlpha());
    }

    void LoadScene() 
    {
        input.Dispose();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    void Fade() 
    {
        fade = true;
    }

}
