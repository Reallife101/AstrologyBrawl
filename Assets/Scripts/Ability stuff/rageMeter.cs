using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rageMeter : MonoBehaviour
{
    [SerializeField] float minDamage;
    [SerializeField] float maxDamage;
    [SerializeField] float minHealth;
    [SerializeField] float maxHealth;
    [SerializeField] doDamage dd;
    [SerializeField] Health hl;

    private float oldHL;

    private void Update()
    {
        if (oldHL != hl.currentHealth)
        {
            dd.SetValues(calculateDamage());
        }

        oldHL = hl.currentHealth;
    }

    float calculateDamage()
    {
        float damagepercent = Mathf.Clamp((hl.currentHealth - minHealth) / (maxHealth - minHealth), 0, 1);

        return minDamage + (maxDamage - minDamage)*(1-damagepercent);
    }
}
