using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


public class HealthItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private Slider HealthBar;
    [SerializeField] private TMPro.TMP_Text KillText;
    [SerializeField] private Image character;

    [SerializeField] private GameObject mixupGO;
    [SerializeField] private GameObject iconicGO;
    [SerializeField] private GameObject shield;


    [SerializeField] private Transform TarotHolder;


    [SerializeField]
    private Animator iconicAnimator;
    [SerializeField]
    private Animator mixupAnimator;

    private int index;

    private float maxCooldown1;
    private float maxCooldown2;
    private int OwnerActorNumber;
    private float CurrentHealth;
    private shieldHealth ShieldHealth;

    public void Initialize(string nickname, int actornumber, Sprite sprite)
    {
        OwnerActorNumber = actornumber;
        SetPlayerName(nickname);
        character.sprite = sprite;
    }

    private void Update()
    {
        if (ShieldHealth)
            shield.SetActive(ShieldHealth.canActivate);
    }

    public void SetShieldHealth(shieldHealth s)
    { 
        ShieldHealth = s;
    }


    public void ActivateTimers()
    {
        mixupGO.SetActive(true);
        iconicGO.SetActive(true);
        shield.SetActive(true);
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
        //Debug.Log("Iconic Animator " + cd1 + " Mixup Animator " + cd2);
        iconicAnimator.SetFloat("percentCooldown", cd1);
        mixupAnimator.SetFloat("cooldownPercent", cd2);

    }

    public Transform GetTarotHolder()
    {
        return TarotHolder;
    }

    public int getActorNumber()
    {
        return OwnerActorNumber;
    }
    
}
