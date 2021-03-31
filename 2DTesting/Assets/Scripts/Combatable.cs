using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Creates a standard framework for enemies and players.
public abstract class Combatable : MonoBehaviour
{
    [SerializeField]
    public float health;

    [SerializeField]
    public float maxHealth;

    [SerializeField]
    public Team team;
    public abstract void attack();

    public Rigidbody2D body;

    protected virtual void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void takeDamage(float damage, float knockbackMagnitude, Vector2 knockbackDirection, Combatable source)
    {
        this.health = this.health - damage;
        Vector2 knockback = knockbackDirection.normalized*knockbackMagnitude;
        body.velocity = body.velocity + knockback;
        if (this.health <= 0)
        {
            onDeath(source);
        }
    }
    //onDeath should call die(). But we've left this implementation for later
    //in case we want to have an enemy that say, ressurects the first time it dies.
    public abstract void onDeath(Combatable source);
    public void die()
    {
        Destroy(this.gameObject);
    }

    public Team getTeam()
    {
        return team;
    }
}

public enum Team
{
    Player,
    Enemy
}