using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTestCard : TarotCard
{
    public override void Apply()
    {
        Debug.Log("Apply Effect");
    }

    private void Start()
    {
        Apply();
    }
}
