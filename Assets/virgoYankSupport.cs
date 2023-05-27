using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virgoYankSupport : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void VirgoMixupOnHitAnimTrigger()
    {
        anim.SetTrigger("mixupConnected");
    }
}
