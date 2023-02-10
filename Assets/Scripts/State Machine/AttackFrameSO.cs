using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackFrameSO", menuName = "ScriptableObjects/AttackSO")]
public class AttackFrameSO : ScriptableObject
{
    public int damage;
    public string attackAnimationName;
    public float duration;
    public float inputBufferTime;
    public float forwardMovement;
    public float finisherEndlag;
    public float hitStunTime;
    public float knockbackPower;
    public Vector2 launchDirection;
    public AttackFrameSO NextAttackFrameSO = null;
}
