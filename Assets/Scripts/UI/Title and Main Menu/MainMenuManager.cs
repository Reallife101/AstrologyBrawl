using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject howToPlay;
    [SerializeField] StudioEventEmitter music;

    public void Play()
    {
        SceneManager.LoadScene("ConnectToServer");
    }

    public void HowToPlay()
    {
        mainMenu.SetActive(!mainMenu.activeInHierarchy);
        howToPlay.SetActive(!howToPlay.activeInHierarchy);
    }

    public void Quit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Gallery()
    {
        SceneManager.LoadScene("ArtGallery");
    }
}
