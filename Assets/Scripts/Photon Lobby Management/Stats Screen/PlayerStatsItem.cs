using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerStatsItem : MonoBehaviourPunCallbacks
{
    public TMP_Text playerName;
    public TMP_Text killsText;
    public TMP_Text deathsText;
    public Image backgroundImage;
    public GameObject crown;
    public Color highlightColor;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image playerAvatar;
    public Sprite[] avatars;

    Player player;

    private void Awake()
    {
        crown.SetActive(false);
    }

    public void ApplyLocalChanges()
    {
        backgroundImage.color = highlightColor;
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            playerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
        if (player.CustomProperties.TryGetValue("kills", out object kills))
        {
            killsText.text = kills.ToString();
        }

        if (player.CustomProperties.TryGetValue("killsToWin", out object killstowin))
        {     
            if (killsText.text.ToString() == killstowin.ToString())
            {
                crown.SetActive(true);
            }
        }
        if (player.CustomProperties.TryGetValue("deaths", out object deaths))
        {
            deathsText.text = deaths.ToString();
        }

    }
}
