using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doDamageHurt : doDamage
{
    [SerializeField] Health hp;
    [SerializeField] float selfDamageAmount;
    public override void extraEffect(float damage)
    {
        hp.TakeDamage(selfDamageAmount, Vector2.zero, 0f);
    }
}
