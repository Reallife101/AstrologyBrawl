using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public PlayerManager myPM;
    private PhotonView myPV;
    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Awake()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();

        myPM = PhotonView.Find((int) myPV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Return if not local avatar
        if (!myPV.IsMine)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRB.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.B) & PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
}
