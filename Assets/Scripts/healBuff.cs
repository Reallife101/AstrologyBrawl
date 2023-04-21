using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healBuff : tempBuffAbility
{
    [SerializeField] private float tickRate;
    [SerializeField] private float healAmount;
    [SerializeField] private Health playerHealth;
    private float hold = 0f;
    private bool check = false;
    public override void endEffect()
    {
        check = false;
    }

    public override void startEffect()
    {
        check = true;
        hold = tickRate;
    }

    // Start is called before the first frame update
    void Start()
    {
        hold = tickRate;
        check = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            if (hold <=0f)
            {
                playerHealth.RPC_HealDamage(healAmount);
                hold = tickRate;
            } else 
            {
                hold -= Time.deltaTime;
            }
        }
    }
}
