using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class counter : Ability
{
    [SerializeField] protected Health health;
    [SerializeField] private float counterLength;
    

    public abstract void onCounter();

    public override void Use()
    {
        health.setCounter(true);
        StartCoroutine(Wait(counterLength));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        health.setCounter(false);
    }

}
