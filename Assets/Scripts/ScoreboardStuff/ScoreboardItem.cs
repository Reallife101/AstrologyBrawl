using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text UsernameText;
    [SerializeField] private TMP_Text KillText;
    [SerializeField] private TMP_Text DeathText;

    public void Initialize(Player player)
    {
        UsernameText.text = player.NickName;
    }
}
