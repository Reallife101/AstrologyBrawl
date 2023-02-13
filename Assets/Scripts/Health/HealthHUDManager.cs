using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class HealthHUDManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject HealthItemPrefab;
    [SerializeField] private Transform HUDTransform;

    private List<HealthItem> HealthItems = new List<HealthItem>();

    public HealthItem AddHealthItem(string nickname, int actornum)
    {
        HealthItem item = Instantiate(HealthItemPrefab, HUDTransform).GetComponent<HealthItem>();
        item.Initialize(nickname, actornum);
        HealthItems.Add(item);

        //sort the list (it still does not work, but it is not essential for health to work)
        /*if(HealthItems.Count > 1)
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

                        //For accurate visible transforms
                        Transform temp = HealthItems[i].transform;
                        HealthItems[i].transform.position = HealthItems[j].transform.position;
                        HealthItems[i].transform.rotation = HealthItems[j].transform.rotation;
                        HealthItems[j].transform.position = temp.transform.position;
                        HealthItems[j].transform.rotation = temp.transform.rotation;
                    }
                }
            }
        }*/

        return item;
    }
}
