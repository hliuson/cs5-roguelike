﻿using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

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

    public abstract void attack2();

    public Rigidbody2D body;
    private Stopwatch attackStopwatch;

    [SerializeField]
    public float attackCooldownMs;

    public int flashWhiteDurationMs = 50;

    public float flashAmount = 0.9f;

    public int armorLevel = 0;

    protected virtual void Start()
    {
        this.attackStopwatch = new Stopwatch();
        this.attackStopwatch.Start();
        this.body = GetComponent<Rigidbody2D>();
        this.body.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(this.team == Team.Enemy)
        {
            EnemyCounter.increment();
        }
    }

    public void takeDamage(float damage, float knockbackMagnitude, Vector2 knockbackDirection, Combatable source)
    {
        this.flashWhite();
        if (this.armorLevel != 0)
        {
            System.Random rand = new System.Random();
            float threshold = calculateArmorThreshold(this.armorLevel);
            if(rand.NextDouble() < threshold)
            {
                return;
            }
        }
        this.health = this.health - damage;
        Vector2 knockback = knockbackDirection.normalized*knockbackMagnitude;
        this.body.AddForce(knockback, ForceMode2D.Impulse);
        
        if (this.health <= 0)
        {
            onDeath(source);
        }
    }

    public float calculateArmorThreshold(float x)
    {
        //Comes out to about .25 at 1, .46 at 2, .63 at 3, .76 at 4 and .85 at 5. Might need adjusting.
        Func<float, float> sigmoid = x => (float)(Math.Pow(Math.E, x)/(1 + Math.Pow(Math.E, x)));

        return 2 * sigmoid(x / 2) - 1;

    }
    //onDeath should call die(). But we've left this implementation for later
    //in case we want to have an enemy that say, ressurects the first time it dies.
    public abstract void onDeath(Combatable source);
    public void die()
    {
        if(this.team == Team.Enemy)
        {
            EnemyCounter.decrement();
        }
        Destroy(this.gameObject);
    }

    public Team getTeam()
    {
        return team;
    }

    protected void tryAttack()
    {
        if (attackStopwatch.ElapsedMilliseconds < attackCooldownMs)
        {
            return;
        }
        else
        {
            attackStopwatch.Reset();
            attackStopwatch.Start();
            attack();
        }
    }

    //Only for player right now
    protected void tryAttack2()
    {
        if (attackStopwatch.ElapsedMilliseconds < attackCooldown())
        {
            return;
        }
        else
        {
            attackStopwatch.Reset();
            attackStopwatch.Start();
            attack2();
        }
    }
    
    public virtual float attackCooldown()
    {
        return attackCooldownMs;
    }

    private async void flashWhite()
    {
        var spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_FlashAmount", this.flashAmount);
        await Task.Delay(this.flashWhiteDurationMs);
        if(spriteRenderer == null)
        {
            return;
        }
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }
}

public enum Team
{
    Player,
    Enemy
}