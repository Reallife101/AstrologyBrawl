using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class destroySelf : MonoBehaviour
{
    public void destorySelfPhoton()
    {
        PhotonView pv = GetComponent<PhotonView>();
        if (pv.IsMine)
            PhotonNetwork.Destroy(gameObject);
    }
}
