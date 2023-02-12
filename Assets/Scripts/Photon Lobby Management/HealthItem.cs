using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private TMPro.TMP_Text Health;

    private int OwnerActorNumber;
    private float currentHealth = 200;

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
        currentHealth = currentHealth - health;
        if(health < 0f)
        {
            health = 0f;
        }
        Health.text = currentHealth.ToString();
    }
}
