using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpBuff : tempBuffAbility
{
    [SerializeField] float newJump;
    [SerializeField] float newDoubleJump;

    private playerController pc;
    private float oldJump;
    private float oldDoubleJump;

    public override void endEffect()
    {
        pc.JumpPower = oldJump;
        pc.DoubleJumpPower = oldDoubleJump;
    }

    public override void startEffect()
    {
        oldJump = pc.JumpPower;
        pc.JumpPower = newJump;
        oldDoubleJump = pc.DoubleJumpPower;
        pc.DoubleJumpPower = newDoubleJump;
    }

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<playerController>();
    }
}
