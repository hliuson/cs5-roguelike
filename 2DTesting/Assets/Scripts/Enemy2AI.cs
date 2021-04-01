using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2AI : Enemy
{
    [SerializeField]
    private EnemyProjectile projectile;

    private Tracker tracker;
    private int internalCount;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        internalCount = 100;
        speed = 10.0f;
        tracker = GetComponent<Tracker>();
        tracker.setSpeed(speed);
        tracker.setBuffer(stoppingDistance);
    }

    //Use fixed update because Update will override what is in Enemy.cs
    private void FixedUpdate()
    {
        checkAggression();
        if (currentTarget != null)
        {
            internalCount--;
            if (internalCount < 0)
            {
                internalCount = 100;
                print("ATTACK");
                attack();
            }
            tracker.target = currentTarget.transform;
            if (!tracker.running)
            {
                tracker.startFollowing();
            }
            //transform.position = Vector2.MoveTowards(body.position, currentTarget.transform.position, speed * Time.fixedDeltaTime);
        }
        else
        {
            //Stop the coroutine
            tracker.stopFollowing();
        }
    }


    public override void attack()
    {
        projectile.shootProjectile();
    }

    public override void onDeath(Combatable source)
    {
        die();
    }
}
