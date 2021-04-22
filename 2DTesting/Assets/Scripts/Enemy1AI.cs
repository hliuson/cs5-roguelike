using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1AI : Enemy
{
    private Tracker tracker;
    private int internalCount;
    private float dashTime = 0.2f;

    [SerializeField]
    private int dashDamage;

    [SerializeField]
    private int dashKnockback;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        internalCount = (int)attackCooldownMs;
        speed = 10.0f; 
        tracker = GetComponent<Tracker>();
        tracker.setSpeed(speed);
        tracker.setBuffer(stoppingDistance);
    }

    public override void attack2()
    {
        //Implimented because it may be used later
    }

    //Use fixed update because Update will override what is in Enemy.cs
    private void FixedUpdate()
    {
        checkAggression();
        //Internal count is there for attack and other things
        if (currentTarget != null)
        {
            internalCount--;
            if (internalCount < 0)
            {
                internalCount = (int)attackCooldownMs;
                attack();
            }
            tracker.target = currentTarget.transform;
            if (!tracker.running)
            {
                tracker.startFollowing();
            }
            //transform.position = Vector2.MoveTowards(body.position, currentTarget.transform.position, speed * Time.fixedDeltaTime);
        } else
        {
            //Stop the coroutine
            tracker.stopFollowing();
        }
    }

    //Has to be done over multiple frames
    private IEnumerator PerformDash()
    {
        float timeElapsed = 0.0f;

        while (timeElapsed < dashTime)
        {
            this.body.position = Vector2.Lerp(body.position, currentTarget.transform.position, timeElapsed/dashTime);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        
        yield return null;
    }

    public override void attack() 
    {
        tracker.stopFollowing();
        StartCoroutine(PerformDash());
        //Dash attack try and hit the player
        tracker.startFollowing();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Combatable entity = collision.gameObject.GetComponent<Combatable>();
        if (entity == null)
        {
            return;
        }
        if (entity.getTeam() == this.team)
        {
            return;
        }
        print("Hit");
        Vector2 knockbackDirection = body.velocity;
        entity.takeDamage(dashDamage, dashKnockback, knockbackDirection, this);
    }

    public override void onDeath(Combatable source)
    {
        die();
    }
}
