using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [SerializeField]
    protected float cooldownTime;
    protected float currentCooldown;
    public bool abilitySoundCheck;
    [SerializeField] protected Animator playerAnimator;
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

    public virtual void release()
    {
        //Wait to be overwritten
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
        else if (currentCooldown < 0f)
        {
            currentCooldown = 0f;
        }

        // Note: Some consideration to using couritines instead, currently no need to though
    }

    public float MaxCooldownTime()
    {
        return cooldownTime;
    }

    public float CurrCooldownTime()
    {
        if (currentCooldown == 0)
            return cooldownTime / cooldownTime;
        return currentCooldown / cooldownTime;
    }
}
