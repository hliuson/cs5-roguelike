using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public float damage;
    public float knockback;
    public float speed;
    public Team team;
    public StatusEffect[] effects;
    public void onCollision(Combatable entity)
    {

    }

    public void FixedUpdate()
    {

    }
}
