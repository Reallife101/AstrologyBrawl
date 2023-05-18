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
        FOOL,
        HERMIT,
        DEVIL
    }

    protected CardNames cardName;

    public CardNames GetCardName()
    {
        return cardName;
    }

    //calls doEffect for everyone described by params me and others
    //do not use for stuff like justice where everything has to be calculated at once before being applied
    protected void doTo(bool me, bool others, int actorNumber)
    {
        int aNum;
        PhotonView[] controllers = FindObjectsOfType<PhotonView>();
        if (me && others)
        {
            for (int i = 0; i < controllers.Length; ++i)
            {
                doEffect(controllers[i].gameObject.GetComponent<PhotonView>().Owner.ActorNumber);
            }
        }
        else if (me)
        {
            doEffect(actorNumber);
        }
        else if (others)
        {
            aNum = -1;
            for (int i = 0; i < controllers.Length; ++i)
            {
                aNum = controllers[i].gameObject.GetComponent<PhotonView>().Owner.ActorNumber;
                if (aNum != actorNumber)
                {
                    doEffect(aNum);
                }
            }
        }
    }

    //do effect is the behavoir to happen for one player for given actorNumber passed in
    protected abstract void doEffect(int actorNumber);

    //effect gets called by tarot card collision event
    public abstract void Effect(int actorNumber);
}
