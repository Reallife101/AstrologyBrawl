using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject MenuCanvas;

    private PlayerControllerInputAsset input;
    private InputAction menuToggle;

    private string leave_to_level = "New Lobby";

    private void Start()
    {
        MenuCanvas.SetActive(false);
        input = new PlayerControllerInputAsset();
        menuToggle = input.Player.Menu;

        menuToggle.started += ToggleMenu;

        menuToggle.Enable();
    }
    public void LeaveMatch()
    {
        Debug.Log("Leaving Match");
        menuToggle.Disable();

        Hashtable hash = new Hashtable();
        hash.Add("left", true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties);
        PhotonNetwork.LoadLevel(leave_to_level);
    }

    public void QuitGame()
    { 
       Application.Quit();
    }


    public void ToggleMenu(InputAction.CallbackContext context)
    {
        MenuCanvas.SetActive(!MenuCanvas.activeInHierarchy);
    }


    public override void OnDisable()
    {
        menuToggle.started -= ToggleMenu; 
    }


}
