using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelect : MonoBehaviour

{
    [SerializeField] FMODUnity.EventReference LibraLock;
    [SerializeField] FMODUnity.EventReference SaggiLock;
    [SerializeField] FMODUnity.EventReference PiscesLock;
    [SerializeField] FMODUnity.EventReference TaurusLock;
    [SerializeField] FMODUnity.EventReference AquaLock;
    [SerializeField] FMODUnity.EventReference VirgoLock;

    PlayerItem PlayerItem;

    public void Awake()
    {
        PlayerItem = GetComponent<PlayerItem>();
    }


    public void CallCharacterLock()
    {
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 0)
        {
            FMODUnity.RuntimeManager.PlayOneShot(LibraLock);
        }
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 1)
        {
            FMODUnity.RuntimeManager.PlayOneShot(SaggiLock);
        }
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 2)
        {
            FMODUnity.RuntimeManager.PlayOneShot(PiscesLock);
        }
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 3)
        {
            FMODUnity.RuntimeManager.PlayOneShot(TaurusLock);
        }
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 4)
        {
            FMODUnity.RuntimeManager.PlayOneShot(AquaLock);
        }
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 5)
        {
            FMODUnity.RuntimeManager.PlayOneShot(VirgoLock);
        }
    }
}