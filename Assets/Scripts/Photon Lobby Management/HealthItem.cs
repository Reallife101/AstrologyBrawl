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
    [SerializeField] private TMPro.TMP_Text CD1Text;
    [SerializeField] private TMPro.TMP_Text CD2Text;

    private float maxCooldown1;
    private float maxCooldown2;
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

    public void SetMaxCooldowns(float MaxCooldown1, float MaxCooldown2)
    {
        maxCooldown1 = MaxCooldown1;
        maxCooldown2 = MaxCooldown2;
    }

    public void UpdateCooldown(float cd1, float cd2)
    {
        Debug.Log(cd1 + ", " + cd2);
        if (cd1 >= Mathf.Epsilon && a1CDCircle.fillAmount < 1)
        {
            a1CDCircle.fillAmount += 1 / maxCooldown1 * Time.deltaTime;
            CD1Text.text = ((int)cd1).ToString();
        }

        if (cd2 >= Mathf.Epsilon && a2CDCircle.fillAmount < 1)
        {
            a2CDCircle.fillAmount += 1 / maxCooldown2 * Time.deltaTime;
            CD2Text.text = ((int)cd2).ToString();
        }

        if (a1CDCircle.fillAmount >= 1)
        {
            a1CDCircle.fillAmount = 0;
            CD1Text.text = "";
        }

        if (a2CDCircle.fillAmount >= 1)
        {
            a2CDCircle.fillAmount = 0;
            CD2Text.text = "";
        }
    }
}
