using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class additionalAbilities : MonoBehaviour
{
    [SerializeField]
    Ability smoke;
    [SerializeField]
    Ability miscParticle1;
    [SerializeField]
    Ability m2;
    [SerializeField]
    Ability i2;
    public void useSmoke()
    {
        smoke.Use();
    }
    public void useMiscParticle1()
    {
        miscParticle1.Use();
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
