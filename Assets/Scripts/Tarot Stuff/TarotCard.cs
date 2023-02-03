using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public abstract class TarotCard : MonoBehaviourPunCallbacks
{
    public abstract void Effect(int actorNumber);
}
