using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class tempBuffAbility : Ability
{
    [SerializeField] private float buffLength;

    private float t = 0f;
    private bool p = false;
    public override void Use()
    {
        t = buffLength;
        p = true;
        startEffect();

    }

    public float getBuffLength()
    {
        return buffLength;
    }

    // Start is called before the first frame update
    void Start()
    {
        t = 0f;
        p = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (p)
        {
            if (t <= 0f)
            {
                endEffect();
                p = false;
            }
            else
            {
                t -= Time.deltaTime;
            }
        }
    }

    public abstract void startEffect();
    public abstract void endEffect();

}
