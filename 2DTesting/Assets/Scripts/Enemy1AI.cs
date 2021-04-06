using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1AI : Enemy
{
    private Tracker tracker;
    private int internalCount;
    private float dashTime = 0.2f;

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
        //Internal count is there for attack and other things
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
            body.position = Vector2.Lerp(body.position, currentTarget.transform.position, timeElapsed/dashTime);
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
        //Vector2 targetLocation = currentTarget.transform.position;
        // body.position = Vector2.MoveTowards(body.position, targetLocation, 10* Time.deltaTime);
        tracker.startFollowing();
    }

    public override void onDeath(Combatable source)
    {
        die();
    }
}
