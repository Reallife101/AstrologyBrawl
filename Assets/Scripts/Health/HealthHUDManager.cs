using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHUDManager : MonoBehaviour
{


    public void Awake()
    {
        GameObject[] test = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(test);
    }

    public void Start()
    {
        GameObject[] test = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(test);
    }
}
