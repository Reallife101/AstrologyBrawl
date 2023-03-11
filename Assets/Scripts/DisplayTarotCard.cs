using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DisplayTarotCard : MonoBehaviourPunCallbacks
{
    public void OnInitialize(Texture2D image, float StayTime)
    {
        GetComponent<RawImage>().texture = image;
        StartCoroutine(DestroySelf(StayTime));
    }

    private IEnumerator DestroySelf(float StayTime)
    {
        yield return new WaitForSeconds(StayTime);

        Destroy(gameObject);
    }

}
