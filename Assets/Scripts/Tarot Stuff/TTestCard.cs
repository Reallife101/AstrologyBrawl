using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class TTestCard : TarotCard
{
    [SerializeField] private float moveSpeedDecreaseFactor = 0.7f;

    public override void Effect()
    {
        Debug.Log("wOEW ozers ?R !");

        pc.MoveSpeed = pc.MoveSpeed * moveSpeedDecreaseFactor;
    }

    
}
