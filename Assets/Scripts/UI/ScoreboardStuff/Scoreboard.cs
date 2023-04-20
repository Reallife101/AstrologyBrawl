using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject scoreboardItemPrefab;
    [SerializeField] private CanvasGroup CanvasGroup;
    [SerializeField] private GameObject scoreBoardContainer;

    private bool scoreboardOff = true;

    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    public void AddScoreboardItem(Player player)
    {
        ScoreboardItem newItem = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        newItem.Initialize(player);
        scoreboardItems[player] = newItem;
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }

    public void ToggleScoreboard()
    {
        if (scoreboardOff)
        {
            //CanvasGroup.alpha = 1;
            scoreboardOff = false;
        }
        else
        {
            //CanvasGroup.alpha = 0;
            scoreboardOff = true;
        }

        scoreBoardContainer.SetActive(!scoreBoardContainer.activeInHierarchy);                     

    }
}
