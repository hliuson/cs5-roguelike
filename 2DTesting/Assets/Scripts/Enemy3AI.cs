using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy3AI : Enemy
{
    [SerializeField]
    private GameObject projectile;

    private Tracker tracker;
    private int internalCount;

    [SerializeField]
    private int difficultyPts = 4;

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

    public override void attack2()
    {
        //Implimented because it may be used later
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
        GameObject projectileInst;
        for (int i = 0; i < 16; i++)
        {
            float angle = i * Mathf.PI / 8;
            projectileInst = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, (angle * Mathf.Rad2Deg + 180.0f))));
            float velx = Mathf.Cos(angle);
            float vely = Mathf.Sin(angle);
            Vector2 vel = new Vector2(velx, vely).normalized;

            Projectile proj = projectileInst.GetComponent<Projectile>();

            projectileInst.GetComponent<Rigidbody2D>().velocity = vel * proj.speed;
            projectileInst.GetComponent<Rigidbody2D>().freezeRotation = true;
            proj.source = this;
        }
    }

    public override void onDeath(Combatable source)
    {
        die();
    }

    public override int difficulty()
    {
        return this.difficultyPts;
    }
}
