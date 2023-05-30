using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virgoDash : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float slideSpeed;
    [SerializeField] private Transform parent;
    public void DashCast()
    {
        if (parent.localScale.x < 0)
        {
            rb.AddForce(-parent.right * slideSpeed, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(parent.right * slideSpeed, ForceMode2D.Impulse);
        }

    }
}
