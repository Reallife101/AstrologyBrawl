using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedCounter : counter
{
    private DamageManager dmgManager;
    private void Start()
    {
        dmgManager = GetComponent<DamageManager>();
    }

    //private int counter; 
    //[SerializeField] private float damageTaken = 0;


    public override void onCounter()
    {
        //Debug.Log(counter++);

        //damageTaken += health.damageTaken;
        dmgManager.affectDamage("stack", health.damageTaken, 0, true);

        //Debug.Log("Countered! Damage Taken " + damageTaken);
    }

}
