
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotCardJustice : TarotCard
{
    private void Start()
    {
        cardName = CardNames.JUSTICE;
    }

    public override void Effect(int actorNumber)
    {
        Debug.Log("wOEW ozers ?R !");

        Health[] Healths = FindObjectsOfType<Health>();

        float CummHealth = 0.0f;
        foreach (Health health in Healths)
        {
            CummHealth += health.currentHealth;
        }
        float NewHealth = CummHealth / Healths.Length;
        foreach (Health health in Healths)
        {
            health.setCurrentHealth(NewHealth);
        }
    }

    protected override void doEffect(int actorNumber)
    {
        throw new System.NotImplementedException();
    }
}
