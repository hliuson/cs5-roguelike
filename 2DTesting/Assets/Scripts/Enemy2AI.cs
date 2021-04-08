using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy2AI : Enemy
{
    [SerializeField]
    private GameObject projectile;

    private Tracker tracker;
    private int internalCount;

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

    //Use fixed update because Update will override what is in Enemy.cs
    private void FixedUpdate()
    {
        checkAggression();
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
        }
        else
        {
            //Stop the coroutine
            tracker.stopFollowing();
        }
    }


    public override void attack()
    {
        //Get positions of everything
        float selfX = transform.position.x;
        float selfY = transform.position.y;
        float targetX = currentTarget.transform.position.x;
        float targetY = currentTarget.transform.position.y;
        float x = targetX - selfX;//gets the distance between object and target position for x
        float y = targetY - selfY;//gets the distance between object and target position for y 
        //Create a clone with the correct angle
        var projectileInst = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(-y, -x) * Mathf.Rad2Deg)));
        //Fire the projectile at a constant speed
        Vector2 vel = new Vector2(x, y).normalized;

        Projectile proj = projectileInst.GetComponent<Projectile>();

        projectileInst.GetComponent<Rigidbody2D>().velocity = vel * proj.speed;
        proj.source = this;


    }

    public override void onDeath(Combatable source)
    {
        die();
    }
}
