using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    private float cooldownTime;
    private float currentCooldown;
    public bool abilitySoundCheck;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private string triggerName;

    private void Start()
    {
        currentCooldown = 0f;
        abilitySoundCheck = true;
    }
    public void activate()
    {
        //If no cooldown, 
        if (currentCooldown <= 0f)
        {
            currentCooldown = cooldownTime;
            abilitySoundCheck = true;
            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger(triggerName);
            }
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
            abilitySoundCheck = false;
         
        }

        // Note: Some consideration to using couritines instead, currently no need to though
    }
}
