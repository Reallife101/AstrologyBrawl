using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HealthItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMPro.TMP_Text PlayerName;
    [SerializeField] private TMPro.TMP_Text Health;

    private int OwnerActorNumber;
    private float CurrentHealth;

    private PhotonView PV;


    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    public void Initialize(string nickname, int actornumber)
    {
        OwnerActorNumber = actornumber;
        SetPlayerName(nickname);
    }


    public void SetPlayerName(string nickname)
    {
        PV.RPC("RPC_SetPlayerName", RpcTarget.All, nickname);
    }

    [PunRPC]
    public void RPC_SetPlayerName(string nickname)
    {
        PlayerName.text = nickname;
    }

    public void SetHealthUI(float health)
    {
        PV.RPC("RPC_SetHealthUI", RpcTarget.All, health);
    }


    [PunRPC]
    public void RPC_SetHealthUI(float health)
    {
        Health.text = health.ToString();
        CurrentHealth = health;
    }

    public void DecreaseHealthUI(float damage)
    {
        PV.RPC("RPC_DecreaseHealthUI", RpcTarget.All, damage);
    }

    [PunRPC]
    public void RPC_DecreaseHealthUI(float damage)
    {
        CurrentHealth = CurrentHealth - damage;
        if (CurrentHealth < 0f)
        {
            CurrentHealth = 0f;
        }
        Health.text = CurrentHealth.ToString();
    }
}
