using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(playerController))]
[RequireComponent(typeof(DamageManager))]
public class damageBuff : tempBuffAbility
{
/*    [SerializeField] private float lightDamage;
    [SerializeField] private float heavyDamage;
    [SerializeField] private float ability1Damage;
    [SerializeField] private float ability2Damage;

    [SerializeField] float debuffSpeed;



    private float oldlightDamage;
    private float oldheavyDamage;
    private float oldability1Damage;
    private float oldability2Damage;

    private playerController pc;*/
    private DamageManager dm;
    [SerializeField]
    private float damageFactor;
/*    private float oldSpeed;*/

    public override void endEffect()
    {
        /*pc.MoveSpeed = oldSpeed;

        dm.lightDamage = oldlightDamage;
        dm.heavyDamage = oldheavyDamage;
        dm.ability1Damage = oldability1Damage;
        dm.ability2Damage = oldability2Damage;*/
    }

    public override void startEffect()
    {
        //Debug.Log("STARTING EFFECT");

        dm.affectAllDamage(damageFactor, 5, false, true);
        /*oldSpeed = pc.MoveSpeed;
        pc.MoveSpeed = debuffSpeed;

        oldlightDamage = dm.lightDamage;
        oldheavyDamage = dm.heavyDamage;
        oldability1Damage = dm.ability1Damage;
        oldability2Damage = dm.ability2Damage;

        dm.lightDamage = lightDamage;
        dm.heavyDamage = heavyDamage;
        dm.ability1Damage = ability1Damage;
        dm.ability2Damage = ability2Damage;*/

    }

    // Start is called before the first frame update
    void Start()
    {
        //pc = GetComponent<playerController>();
        dm = GetComponent<DamageManager>();
    }


}
