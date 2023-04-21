using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerController))]
public class speedBuff : tempBuffAbility
{
    [SerializeField] float buffSpeed;

    private playerController pc;
    private float oldSpeed;

    public override void endEffect()
    {
        pc.MoveSpeed = oldSpeed;
    }

    public override void startEffect()
    {
        oldSpeed = pc.MoveSpeed;
        pc.MoveSpeed = buffSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<playerController>();
    }

}
