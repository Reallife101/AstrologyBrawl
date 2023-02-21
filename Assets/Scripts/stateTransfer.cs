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
        stateManager.EndCasting(.1f);
    }
}
