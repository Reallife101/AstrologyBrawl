using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Healthbar : MonoBehaviour
{
    private TMPro.TMP_Text healthText;

    // if healing pls add cap thing pls ;c;;c;;;
    public void UpdateHealthUI(float currentHealth)
    {
        if (currentHealth < 0f) { currentHealth = 0f; }
        healthText.text = currentHealth.ToString();
    }

    private void Awake()
    {
        healthText = GetComponentInChildren<TMPro.TMP_Text>();
    }

    
}
