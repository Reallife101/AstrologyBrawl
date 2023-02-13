using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private TMPro.TMP_Text Health;

    private int OwnerActorNumber;
    private float CurrentHealth;

    public void Initialize(string nickname, int actornumber)
    {
        OwnerActorNumber = actornumber;
        SetPlayerName(nickname);
    }

    public void SetPlayerName(string nickname)
    {
        PlayerName.text = nickname;
    }

    public void SetHealthUI(float health)
    {
        Health.text = health.ToString();
        CurrentHealth = health;
    }

    public void DecreaseHealthUI(float damage)
    {
        CurrentHealth = CurrentHealth - damage;
        if (CurrentHealth < 0f)
        {
            CurrentHealth = 0f;
        }
        Health.text = CurrentHealth.ToString();
    }
}
