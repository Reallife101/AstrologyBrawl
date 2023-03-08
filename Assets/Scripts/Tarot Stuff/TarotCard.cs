using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public abstract class TarotCard : MonoBehaviourPunCallbacks
{
    public enum CardNames
    {
        JUSTICE,
        MAGICIAN,
        LOVERS
    }

    protected CardNames cardName;

    public CardNames GetCardName()
    {
        return cardName;
    }

    public abstract void Effect(int actorNumber);
}
