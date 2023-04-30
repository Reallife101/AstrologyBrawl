using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class picsesInvul : Ability
{
    [SerializeField] PlayerHealth ph;
    [SerializeField] StateManager sm;
    bool isActive = false;
    public override void Use()
    {
        isActive = !isActive;
        ph.setInvincible(isActive);
    }

    public void Dive()
    {
        sm.enterInfiniteRecovery();
    }

    public void Surface()
    {
        sm.setStateIdle();
    }
}
