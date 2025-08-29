using System;
using UnityEngine;

public class LivingEntity : MonoBehaviour , IDamageAble
{
    protected float maxHp;

    public float hp;
    public event Action OnDeath;

    protected virtual void OnEnable()
    {
        hp = maxHp;
    }

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }
    protected virtual void Die()
    {
        OnDeath?.Invoke();
        Destroy(gameObject, 3);
    }
}
