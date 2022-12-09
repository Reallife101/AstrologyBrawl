using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    private PhotonView myPV;
    private Rigidbody2D myRB;
    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myRB = GetComponent<Rigidbody2D>();
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
    }
}
