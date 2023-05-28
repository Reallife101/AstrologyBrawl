using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using System;

public class levelInfoItem : MonoBehaviourPunCallbacks
{
    [Serializable]
    public struct Level
    {
        public Button btn;
        public string levelName;
        public string description;
        public Image img;
        public GameObject background;
    }

    [SerializeField]
    private List<Level> levels;

    [SerializeField]
    private TMP_Text levelText;
    [SerializeField]
    private TMP_Text description;
    [SerializeField]
    private Image levelImage;
    [SerializeField]
    private GameObject playbtn;


    private GameObject lastbackgroundImage;


    
    // Start is called before the first frame update
    void Start()
    {
        playbtn.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
            playbtn.SetActive(true);

        for(int i = 0; i < levels.Count; ++i)
        {
            int indexCopy = i;
            levels[i].btn.onClick.AddListener(delegate { DisplayLevel(indexCopy); } );
        }

        DisplayLevel(0); //by default it would put the first button
    }

    public string getSceneName()
    {
        return levelText.text;
    }

    public void DisplayLevel(int index)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        photonView.RPC(nameof(RPC_DisplayLevel), RpcTarget.All, index);
    }

    [PunRPC]
    public void RPC_DisplayLevel(int index)
    {
        if (lastbackgroundImage)
            lastbackgroundImage.SetActive(false);
        levelText.text =  levels[index].levelName;
        description.text = levels[index].description;
        levelImage.sprite = levels[index].img.sprite;
        lastbackgroundImage = levels[index].background;
        lastbackgroundImage.SetActive(true);
    }

}
