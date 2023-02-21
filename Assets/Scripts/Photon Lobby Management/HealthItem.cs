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
    [SerializeField] private Image a1CDCircle;
    [SerializeField] private Image a2CDCircle;

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

    public void UpdateKillCount(int kills)
    {
        KillText.text = "Kills: " + kills;
    }

    public void UpdateCooldown(float cd1, float cd2)
    {
        a1CDCircle.fillAmount += 1 / cd1 * Time.deltaTime;
        a2CDCircle.fillAmount += 1 / cd2 * Time.deltaTime;

        if (a1CDCircle.fillAmount >= 1)
        {
            a1CDCircle.fillAmount = 0;
        }

        if (a2CDCircle.fillAmount >= 1)
        {
            a2CDCircle.fillAmount = 0;
        }
    }
}
