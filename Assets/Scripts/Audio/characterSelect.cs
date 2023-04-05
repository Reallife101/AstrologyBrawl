using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelect : MonoBehaviour

{
    [SerializeField] FMODUnity.EventReference LibraLock;
    [SerializeField] FMODUnity.EventReference SaggiLock;

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
    }
}