using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    private float cooldownTime;
    private float currentCooldown;

    private void Start()
    {
        currentCooldown = 0f;
    }
    public void activate()
    {
        //If no cooldown, 
        if (currentCooldown <= 0f)
        {
            currentCooldown = cooldownTime;
            Use();
        }
    }
    public abstract void Use();

    private void Update()
    {
        // Reduce cooldown
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
        }

        // Note: Some consideration to using couritines instead, currently no need to though
    }
}
