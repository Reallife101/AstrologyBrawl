using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healBuff : tempBuffAbility
{
    [SerializeField] private float tickRate;
    [SerializeField] private float healAmount;
    [SerializeField] private Health playerHealth;
    public override void endEffect()
    {

    }

    public override void startEffect()
    {
        StartCoroutine(heal());
    }


    IEnumerator heal()
    {
        for (float length = getBuffLength(); length > 0; length -= tickRate)
        {
            playerHealth.RPC_HealDamage(healAmount);
            yield return new WaitForSeconds(tickRate);
        }
    }
}
