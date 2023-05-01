using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelectAudio : MonoBehaviour

{
    [SerializeField] FMODUnity.EventReference LibraLock;
    [SerializeField] FMODUnity.EventReference SaggiLock;

    CharacterSelect PlayerItem;

    public void Awake()
    {
        PlayerItem = GetComponent<CharacterSelect>();
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