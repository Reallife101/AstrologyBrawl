using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [Header("Damage Values")]
    [SerializeField] private float Light;
    [SerializeField] private float Heavy;
    [SerializeField] private float Ability1;
    [SerializeField] private float Ability2;
    [SerializeField] private float stacked;

    [SerializeField] private float LightBuff=1;
    [SerializeField] private float HeavyBuff=1;
    [SerializeField] private float A1Buff=1;
    [SerializeField] private float A2Buff=1;
    [SerializeField] private float StackBuff=1;

    public enum AttackStates
    {
        NoDamage,
        Light,
        Heavy,
        Ability1,
        Ability2,
        Stacked
    }
    //Getter for DoDamage
    public float stackedDamage { get { return stacked; } private set { stacked = value; } }
    public float lightDamage { get { return Light; } set { Light = value; } }
    public float heavyDamage { get { return Heavy; } set { Heavy = value; } }
    public float ability1Damage { get { return Ability1; } set { Ability1 = value; } }
    public float ability2Damage { get { return Ability2; } set { Ability2 = value; } }

    private AttackStates atkState;


    //dictionary to call functions from keys
    Dictionary<string, Action<float, bool>> dmgFunctions;

    void Start()
    {
        dmgFunctions =
                new Dictionary<string, Action<float, bool>>() { {"light", affectLightDamage}, {"heavy", affectHeavyDamage},
                                                            {"ability1", affectAbility1Damage}, {"ability2", affectAbility2Damage},
                                                            {"stack", affectStackDamage} };
    }

    // Exectutes the function associated with key 'type'
    public void affectDamage(string type, float factor, float time, bool forever = false, bool multiply = false)
    {
        Action<float, bool> funcType;

        if (dmgFunctions.TryGetValue(type, out funcType))
        {
            funcType(factor, multiply);
        }
        else
        {
            Debug.Log("Incorrect key passed: " + type + ". Expected: light, heavy, ability1, ability2, stack");
        }

        if (!forever)
        {
            StartCoroutine(EffectTime(time, factor, multiply, type));
        }
    }
    public void affectAllDamage(float factor, float time, bool forever = false, bool multiply = false)
    {
        Debug.Log("DAMAGE BUFF");
        foreach(KeyValuePair<string, Action<float, bool>> funcType in dmgFunctions) { 
            funcType.Value(factor, multiply);
        }

        if (!forever)
        {
            StartCoroutine(EffectTime(time, factor, multiply));
        }
    }

    public void setDamage(AttackStates type, float dmg, float time, bool forever = false)
    {
        Debug.Log("Setting damage " + dmg + " for " + type.ToString());

        switch (type)
        {
            case DamageManager.AttackStates.Light:
                Light = dmg;
                break;
            case DamageManager.AttackStates.Heavy:
                Heavy = dmg;
                break;
            case DamageManager.AttackStates.Ability1:
                Ability1 = dmg;
                break;
            case DamageManager.AttackStates.Ability2:
                Ability2 = dmg;
                break;
            case DamageManager.AttackStates.Stacked:
                stacked = dmg;
                break;
            default:
                Debug.Log("Unknown attack type " + type + ". Expected: light, heavy, ability1, ability2, or stack");
                break;
        }

        if (!forever)
        {
            StartCoroutine(EffectTime(time, dmg, false));
        }
    }

    public float getAttackValue(AttackStates type) {

        switch (type)
        {
            case AttackStates.Light:
                atkState = AttackStates.Light;
                return Light;
            case AttackStates.Heavy:
                atkState = AttackStates.Heavy;
                return Heavy;
            case AttackStates.Ability1:
                atkState = AttackStates.Ability1;
                return Ability1;
            case AttackStates.Ability2:
                atkState = AttackStates.Ability2;
                return Ability2;
            default:
                atkState = AttackStates.NoDamage;
                Debug.Log("Unknown attack type " + type + ". Expected: light, heavy, ability1, or ability2");
                return 0;
        }

    }
    public float getBuffValue(AttackStates type)
    {

        switch (type)
        {
            case AttackStates.Light:
                atkState = AttackStates.Light;
                return LightBuff;
            case AttackStates.Heavy:
                atkState = AttackStates.Heavy;
                return HeavyBuff;
            case AttackStates.Ability1:
                atkState = AttackStates.Ability1;
                return A1Buff;
            case AttackStates.Ability2:
                atkState = AttackStates.Ability2;
                return A2Buff;
            default:
                atkState = AttackStates.NoDamage;
                Debug.Log("Unknown attack type " + type + ". Expected: light, heavy, ability1, or ability2");
                return 1;
        }

    }

    public AttackStates getCurrentAttack() 
    {
        return atkState;
    }

    public float applyStackDamage() 
    {
        float dmg = stacked;
        stacked = 0;
        return dmg;
    }

    IEnumerator EffectTime(float seconds, float damage, bool wasMultiplied, string type = "")
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("BUFF OVER");
        if (type != "")
        {
            SingleReset(type, damage, wasMultiplied);
        }
        else
        {
            ResetAll(damage, wasMultiplied);
        }

    }

    private void affectLightDamage(float damage, bool multiply)
    {
        if (!multiply)
            Light += damage;
        else
            LightBuff *= damage;
    }

    private void affectHeavyDamage(float damage, bool multiply)
    {
        if (!multiply)
            Heavy += damage;
        else
            HeavyBuff *= damage;
    }
    private void affectAbility1Damage(float damage, bool multiply)
    {
        if (!multiply)
            Ability1 += damage;
        else
            A1Buff *= damage;
    }

    private void affectAbility2Damage(float damage, bool multiply)
    {
        if (!multiply)
            Ability2 += damage;
        else
            A2Buff *= damage;
    }

    private void affectStackDamage(float damage, bool multiply)
    {
        if (!multiply)
            stacked += damage;
        else
            StackBuff *= damage;
    }

    private void SingleReset(string type, float damage, bool wasMultiplied)
    {
        if (wasMultiplied)
            dmgFunctions[type](1 / damage, true); //reciprocal of damage
        else
            dmgFunctions[type](-damage, false); //(-) symbol for reverting the effects of the damage
    }

    private void ResetAll(float damage, bool wasMultiplied)
    {
        foreach (KeyValuePair<string, Action<float, bool>> funcType in dmgFunctions)
        {
            if (wasMultiplied)
            {
                //Debug.Log(damage + ", " +  1 / damage);
                funcType.Value(1 / damage, true); //reciprocal of damage
                //Debug.Log(funcType);
            }
            else
                funcType.Value(-damage, false); //(-) symbol for reverting the effects of the damage
        }    
    }
   
}
