using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterSelect : MonoBehaviour

{
    [SerializeField] FMODUnity.EventReference LibraLock;

    PlayerItem PlayerItem;

    public void Awake()
    {
        PlayerItem = GetComponent<PlayerItem>();
    }


    public void CallCharacterLock()
    {
        if ((int)PlayerItem.playerProperties["playerAvatar"] == 1)
        {
            FMODUnity.RuntimeManager.PlayOneShot(LibraLock);
            Debug.Log("i am a sussy amogus");
        }
    }
}