using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class addPlayersToFollow : MonoBehaviour
{
    private CinemachineTargetGroup targetGroup;
    // Start is called before the first frame update
    void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
    }

    public void removeMember(Transform t)
    {
        targetGroup.RemoveMember(t);
    }
    public void removeAllMember()
    {
        foreach (CinemachineTargetGroup.Target t in targetGroup.m_Targets)
        {
            removeMember(t.target);
        }
    }

    public void addAllMembers(GameObject[] players)
    {
        foreach (GameObject go in players)
        {
            PhotonView pv = go.GetComponent<PhotonView>();

            if (pv != null && pv.IsMine)
            {
                targetGroup.AddMember(go.transform, 1.25f, 7);
            }
            else
            {
                targetGroup.AddMember(go.transform, 1, 3);
            }
        }

        GameObject center = GameObject.FindGameObjectWithTag("centerStage");
        if (center != null)
        {
            targetGroup.AddMember(center.transform, 1.125f, 1);
        }
    }

    public void changeMembers(GameObject[] players)
    {
        removeAllMember();
        addAllMembers(players);
    }

    public void addMember(Transform t)
    {
        targetGroup.AddMember(t, 1, 1);
    }

    public void addMember(Transform t, float weight, float radius)
    {
        targetGroup.AddMember(t, weight, radius);
    }

}
