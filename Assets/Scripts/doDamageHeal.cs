using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doDamageHeal : doDamage
{
    [SerializeField] Health hp;
    [SerializeField] float healAmount;
    public override void extraEffect(float damage)
    {
        hp.healDamage(healAmount);
    }
}
