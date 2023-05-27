using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateTransfer : MonoBehaviour
{
    [SerializeField] StateManager stateManager;

    public void startAnimation()
    {
        stateManager.StartCasting();
    }

    public void endAnimation()
    {
        stateManager.EndCasting(.2f);
    }

    public void endAnimation(float time)
    {
        stateManager.EndCasting(time);
    }

}
