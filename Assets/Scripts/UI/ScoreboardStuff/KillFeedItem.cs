using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class KillFeedItem : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text killerUsername;
    [SerializeField] private TMP_Text killedPlayerUsername;
    [SerializeField] private Image killerImage;
    [SerializeField] private Image killedPlayerImage;
    public Sprite[] avatars;
    [SerializeField] private float lifetime;


    public void Initialize(Player Killer, Player KilledPlayer)
    {
        killerUsername.text = Killer.NickName;
        killedPlayerUsername.text = KilledPlayer.NickName;
        killerImage.sprite = avatars[(int)Killer.CustomProperties["playerAvatar"]];
        killedPlayerImage.sprite = avatars[(int)KilledPlayer.CustomProperties["playerAvatar"]];
        StartCoroutine(BeginDeath());
    }

    IEnumerator BeginDeath()
    {
        yield return new WaitForSeconds(lifetime);
        Object.Destroy(gameObject);
    }

}
