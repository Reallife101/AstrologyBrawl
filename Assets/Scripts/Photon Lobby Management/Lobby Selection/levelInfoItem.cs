using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class levelInfoItem : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string[] levels;

    private int levelIndex;
    private TMP_Text levelText;
    
    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<TMP_Text>();
        levelIndex = 0;
        if (PhotonNetwork.IsMasterClient)
        {
            updateText();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            updateText();
        }
    }


    void updateText()
    {
        levelText.text = levels[levelIndex];
        this.photonView.RPC(nameof(RPC_UpdateText), RpcTarget.All, levels[levelIndex]);
    }

    public void updateText(string s)
    {
        levelText.text = s;
    }

    [PunRPC]
    void RPC_UpdateText(string s)
    {
        updateText(s);
    }

    public string getSceneName()
    {
        return levels[levelIndex];
    }

    public void OnClickLeft()
    {
        if (levelIndex == 0)
        {
            levelIndex = levels.Length - 1;
        }
        else
        {
            levelIndex = levelIndex - 1;
        }
        updateText();
    }

    public void OnClickRight()
    {
        if (levelIndex == levels.Length - 1)
        {
            levelIndex = 0;
        }
        else
        {
            levelIndex = levelIndex + 1;
        }
        updateText();
    }

}
