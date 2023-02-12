using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class libraCounter : counter
{
    private int counter; 
    [SerializeField] private float damageTaken = 0;


    public override void onCounter()
    {

       //Debug.Log(counter++);

        damageTaken += health.damageTaken;

        //Debug.Log("Countered! Damage Taken " + damageTaken);

    }


    public float get_DamageIncrease()
    {
        return damageTaken;  
    }

    public void ResetDamage() {

        damageTaken = 0;
    
    }
}
