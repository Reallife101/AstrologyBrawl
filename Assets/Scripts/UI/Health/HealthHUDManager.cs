using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class HealthHUDManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject HealthItemPrefab;
    [SerializeField] private Transform HUDTransform;
    [SerializeField] private List<Sprite> characterImages;

    public List<HealthItem> HealthItems = new List<HealthItem>();

    public HealthItem AddHealthItem(string nickname, int actornum, Player player)
    {
        GameObject healthItem = Instantiate(HealthItemPrefab, HUDTransform);
        HealthItem item = healthItem.GetComponent<HealthItem>();
        Sprite character = characterImages[(int)player.CustomProperties["playerAvatar"]];
        item.Initialize(nickname, actornum, character);
        HealthItems.Add(item);
        SortHealthItems();
        return item;
    }

    public void SortHealthItems()
    {
        //sort the list (it still does not work, but it is not essential for health to work)
        if (HealthItems.Count > 1)
        {
            for (int i = 0; i < HealthItems.Count - 1; i++)
            {
                for (int j = i + 1; j < HealthItems.Count; j++)
                {
                    if (HealthItems[i].GetOwnerActorNumber() > HealthItems[j].GetOwnerActorNumber())
                    {
                        //For orderliness in the list
                        HealthItem tempitem = HealthItems[i];
                        HealthItems[i] = HealthItems[j];
                        HealthItems[j] = tempitem;
                    }
                }
                HealthItems[i].transform.SetSiblingIndex(i);
            }
        }
    }


    public void UpdateFrames()
    {
        foreach (HealthItem item in HealthItems)
        {
            if (!PhotonNetwork.CurrentRoom.Players.ContainsKey(item.getActorNumber()))
                Destroy(item.gameObject);
        }
    }

}
