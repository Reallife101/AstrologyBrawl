using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private TMPro.TMP_Text Health;

    private int OwnerActorNumber;

    public void Initialize(string nickname, int actornumber)
    {
        OwnerActorNumber = actornumber;
        SetPlayerName(nickname);
        SetHealth(100);
    }

    public void SetPlayerName(string nickname)
    {
        PlayerName.text = nickname;
    }

    public void SetHealth(int health)
    {
        Health.text = health.ToString();
    }
}
