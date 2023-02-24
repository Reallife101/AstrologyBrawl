using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manageAbilities : MonoBehaviour
{
    [SerializeField]
    Ability light;
    [SerializeField]
    Ability heavy;
    [SerializeField]
    Ability mixup;
    [SerializeField]
    Ability iconic;
    public void useLight()
    {
        light.Use();
    }
    public void useHeavy()
    {
        heavy.Use();
    }
    public void useMixup()
    {
        mixup.Use();
    }
    public void useIconic()
    {
        iconic.Use();
    }
}
