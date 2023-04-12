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
        LOVERS,
        FOOL
    }

    protected CardNames cardName;

    public CardNames GetCardName()
    {
        return cardName;
    }

    //do not use for stuff like justice where everything has to be calculated at once before being applied
    public void doTo(bool me, bool others, int actorNumber)
    {
        int aNum;
        PhotonView[] controllers = FindObjectsOfType<PhotonView>();
        if (me && others)
        {
            for (int i = 0; i < controllers.Length; ++i)
            {
                Effect(controllers[i].gameObject.GetComponent<PhotonView>().Owner.ActorNumber);
            }
        }
        else if (me)
        {
            Effect(actorNumber);
        }
        else if (others)
        {
            aNum = -1;
            for (int i = 0; i < controllers.Length; ++i)
            {
                aNum = controllers[i].gameObject.GetComponent<PhotonView>().Owner.ActorNumber;
                if (aNum != actorNumber)
                {
                    Effect(aNum);
                }
            }
        }
    }

    public abstract void Effect(int actorNumber);
}
