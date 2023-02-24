using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateFastFall : MonoBehaviour
{
    [SerializeField] playerController pc;

    public void startFastFall()
    {
        pc.setFastFall(true);
    }
}
