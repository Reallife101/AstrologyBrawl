using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI start_text;

    [SerializeField]
    TextMeshProUGUI leave_text;

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

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetJoystickNames().Length >= 1 && Input.GetJoystickNames()[0] != "")
        {
            start_text.text = "press A to start";
            leave_text.text = "hold B to quit";
        }
        else
        {
            start_text.text = "press any button to start";
            leave_text.text = "hold q to quit";
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
    }

    void LoadScene() 
    {
        startGame.Disable();
        quitGame.Disable();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    void Fade() 
    {
        fade = true;
    }

}
