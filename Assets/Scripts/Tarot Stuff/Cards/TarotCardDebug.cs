using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardDebug : TarotCard
{

    public override void Effect(int actorNumber)
    {
        Debug.Log("actorNumber: "+ actorNumber);


    }

    protected override void doEffect(int actorNumber)
    {
        throw new System.NotImplementedException();
    }
}
