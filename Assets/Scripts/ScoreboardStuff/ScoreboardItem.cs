using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text UsernameText;
    [SerializeField] private TMP_Text KillText;
    [SerializeField] private TMP_Text DeathText;
    private Player playerValue;

    public void Initialize(Player player)
    {
        UsernameText.text = player.NickName;
        playerValue = player;
        UpdateStats();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (targetPlayer == playerValue)
        {
            if (changedProps.ContainsKey("kills") || changedProps.ContainsKey("deaths"))
            {
                UpdateStats();
            }
        }
    }

    public void UpdateStats()
    {
        if(playerValue.CustomProperties.TryGetValue("kills", out object kills))
        {
            KillText.text = kills.ToString();
        }

        if (playerValue.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            DeathText.text = deaths.ToString();
        }
    }
}
