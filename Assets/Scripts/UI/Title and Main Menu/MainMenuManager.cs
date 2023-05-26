using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //private PlayerControllerInputAsset input;

    //private InputAction startGame;
    //private InputAction quitGame;


    private void Awake()
    {
        
        //input = new PlayerControllerInputAsset();
        /*
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
        */
    }

    public void Play()
    {
        //input.Dispose();
        Debug.Log("Going to Connect to Server");
        SceneManager.LoadScene("ConnectToServer");
    }

    public void HowToPlay()
    {
        //input.Dispose();
        Debug.Log("Going to How to Play");
        //SceneManager.LoadScene("HowToPlay");
    }

    public void Quit()
    {
        //input.Dispose();
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void Credits()
    {
        //input.Dispose();
        Debug.Log("Going to Credits");
        SceneManager.LoadScene("Credits");
    }

    public void Gallery()
    {
        //input.Dispose();
        Debug.Log("Going to Gallery"); // Delete after creating gallery
        //SceneManager.LoadScene("Gallery");
    }
}
