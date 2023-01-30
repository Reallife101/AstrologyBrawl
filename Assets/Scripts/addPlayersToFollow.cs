using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
            targetGroup.AddMember(go.transform, 1, 1);
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
