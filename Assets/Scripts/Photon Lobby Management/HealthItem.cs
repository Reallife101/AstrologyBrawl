using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class HealthItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private TMPro.TMP_Text KillText;

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
        HealthBar.maxValue = health;
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

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (changedProps.ContainsKey("kills"))
        {
            targetPlayer.CustomProperties.TryGetValue("kills", out object kills);
            string KillString = (string)kills;

            KillText.text = "Kills: " + KillString;
        }
    }
}
