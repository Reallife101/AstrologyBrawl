using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapAnimPlunge : MonoBehaviour
{
    [SerializeField]
    private capPlunge cp;

    public void Plunge()
    {
        cp.Plunge();
    }
}
