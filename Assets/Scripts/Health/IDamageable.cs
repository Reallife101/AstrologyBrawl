using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage);
    void TakeDamage(float damage, Vector2 launchVector, float hitStunValue = 0.25f);
}