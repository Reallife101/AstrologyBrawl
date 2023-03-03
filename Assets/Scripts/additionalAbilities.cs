using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class additionalAbilities : MonoBehaviour
{
    [SerializeField]
    Ability l2;
    [SerializeField]
    Ability h2;
    [SerializeField]
    Ability m2;
    [SerializeField]
    Ability i2;
    public void useL2()
    {
        l2.Use();
    }
    public void useH2()
    {
        h2.Use();
    }
    public void useM2()
    {
        m2.Use();
    }
    public void useI2()
    {
        i2.Use();
    }
}
