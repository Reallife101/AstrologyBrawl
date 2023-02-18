using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private Slider HealthBar;

    private int OwnerActorNumber;
    private float CurrentHealth;

    public void Initialize(string nickname, int actornumber)
    {
        OwnerActorNumber = actornumber;
        SetPlayerName(nickname);
    }

    public int GetOwnerActorNumber()
    {
        return OwnerActorNumber;
    }

    public void SetPlayerName(string nickname)
    {
        PlayerName.text = nickname;
    }

    public void SetHealthUI(float health)
    {
        HealthBar.value = health;
        CurrentHealth = health;
    }

    public void DecreaseHealthUI(float damage)
    {
        CurrentHealth = CurrentHealth - damage;
        if (CurrentHealth < 0f)
        {
            CurrentHealth = 0f;
        }
        HealthBar.value = CurrentHealth;
    }
}
