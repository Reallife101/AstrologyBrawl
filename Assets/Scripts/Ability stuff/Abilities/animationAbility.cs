using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationAbility : Ability
{
    [SerializeField]
    private AttackFrameSO attackData;
    [SerializeField]
    private bool isIconic;
    [SerializeField]
    private bool isMixup;
    [SerializeField]
    private StateManager stateManager;
    public override void Use()
    {
        if (stateManager)
        {
            if (isIconic)
            {
                startAnimationIconic();
            }
            else if (isMixup)
            {
                startAnimationMixup();
            }
        }
    }

    public void startAnimationIconic()
    {
        stateManager.StartCastingIconic(attackData);
    }

    public void startAnimationMixup()
    {
        stateManager.StartCastingMixup(attackData);
    }

}
