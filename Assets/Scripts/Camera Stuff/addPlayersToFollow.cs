using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;

public class addPlayersToFollow : MonoBehaviour
{
    private CinemachineTargetGroup targetGroup;

    [SerializeField]
    CinemachineVirtualCamera VirtualCamera;

    [SerializeField]
    float DepthUpdateSpeed = 5f;
    [SerializeField]
    float minDepth = 5f;
    [SerializeField]
    float MaxDepth = 10f;
    [SerializeField]
    float minDistance = 5f;
    [SerializeField]
    float MaxDistance = 30f;

    private float currDepth = 6f;

    private GameObject[] people;

    // Start is called before the first frame update
    void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
        people = GameObject.FindGameObjectsWithTag("Player");
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
        people = players;
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

    private void Update()
    {
        if (VirtualCamera != null)
        {
            calcDepth();
            moveCamera();
        }
    }

    private void calcDepth()
    {
        if (people != null)
        {
            
            if (people.Length == 1)
            {
                currDepth = minDepth;
                return;
            }

            float maxFound = 0f;
            foreach (GameObject p in people)
            {
                if (p!=null)
                {
                    foreach (GameObject z in people)
                    {
                        if (z!=null)
                        {
                            float dist = Vector3.Distance(p.transform.position, z.transform.position);

                            if (dist > maxFound)
                            {
                                maxFound = dist;
                            }
                        }
                    }
                }
            }

            //calculate currDepth
            float percent = Mathf.Clamp((maxFound - minDistance) / (MaxDistance - minDistance), 0, 1);
            currDepth = minDepth + (MaxDepth - minDepth)* percent;

        }
    }

    private void moveCamera()
    {
        if (VirtualCamera.m_Lens.OrthographicSize != currDepth)
        {
            VirtualCamera.m_Lens.OrthographicSize = Mathf.MoveTowards(VirtualCamera.m_Lens.OrthographicSize, currDepth, DepthUpdateSpeed * Time.deltaTime);
        }
    }

}
