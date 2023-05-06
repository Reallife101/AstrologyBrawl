using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(playerController))]
public class panFluteAbility : Ability
{
    [SerializeField] Ability up;
    [SerializeField] Ability down;
    [SerializeField] Ability left;
    [SerializeField] Ability right;
    [SerializeField] private Animator uiAni;
    [SerializeField] private PhotonView pvAni;

    private playerController pc;
    private bool poll = false;
    public override void Use()
    {
        poll = true;

        if (pvAni.IsMine)
        {
            uiAni.SetTrigger("select");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<playerController>();
        poll = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (poll)
        {
            // Read Movement Controls
            Vector2 movementVector = pc.playerMove.ReadValue<Vector2>();

            if (movementVector != Vector2.zero)
            {
                // Yes, this code is inefficient, fight me
                // Calculate what direction has strongest magnitude
                if (Mathf.Abs(movementVector.x)> Mathf.Abs(movementVector.y))
                {
                    if (movementVector.x>0)
                    {
                        right.Use();
                        uiAni.SetTrigger("damage");
                    }
                    else
                    {
                        left.Use();
                        uiAni.SetTrigger("heal");
                    }
                }
                else
                {
                    if (movementVector.y > 0)
                    {
                        up.Use();
                        uiAni.SetTrigger("jump");
                    }
                    else
                    {
                        down.Use();
                        uiAni.SetTrigger("speed");
                    }
                }

                //Stop Polling
                poll = false;
            }
        }
    }
}
