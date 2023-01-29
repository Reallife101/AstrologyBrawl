using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class username : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TMP_Text text;

    
    void Start()
    {
     
        text.text = playerPV.Owner.NickName;

    }

    private void Update()
    {
        gameObject.transform.localScale = new Vector3(playerPV.gameObject.transform.localScale.x, 1, 1);
    }


}
