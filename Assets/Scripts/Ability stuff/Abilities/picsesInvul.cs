using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class picsesInvul : Ability
{
    [SerializeField] PlayerHealth ph;
    [SerializeField] StateManager sm;

    public override void Use()
    {
    }

    public void Dive()
    {
        ph.setInvincible(true);
    }

    public void EndDive()
    {
        ph.setInvincible(false);
    }
}
