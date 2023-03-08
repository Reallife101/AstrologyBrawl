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
    [SerializeField] private GameObject CooldownTimer1;
    [SerializeField] private GameObject CooldownTimer2;
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

    public void ActivateTimers()
    {
        CooldownTimer1.SetActive(true);
        CooldownTimer2.SetActive(true);
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

    public void IncreaseHealthUI(float health)
    {
        CurrentHealth = CurrentHealth + health;
        if (CurrentHealth > HealthBar.maxValue)
        {
            CurrentHealth = HealthBar.maxValue;
        }
        HealthBar.value = CurrentHealth;
    }

    public void ChangeHealthUI(float health)
    {
        CurrentHealth = health;
        HealthBar.value = CurrentHealth;
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
        HandleCooldowns(cd1, maxCooldown1, a1CDCircle, CD1Text);
        HandleCooldowns(cd2, maxCooldown2, a2CDCircle, CD2Text);
    }

    private void HandleCooldowns(float cd, float maxcd, Image circle, TMPro.TMP_Text textObject)
    {
        // if not on cooldown ...
        if (cd == 0)
        {
            // ... reset circle and text
            circle.fillAmount = 0;
            textObject.text = "";
        }
        // otherwise ...
        else if (cd > 0)
        {
            // decrement timer ui
            circle.fillAmount = cd / maxcd + Time.deltaTime;
            textObject.text = ((int)cd).ToString();
        }
    }
}
