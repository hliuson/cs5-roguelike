using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1AI : Enemy
{
    private Tracker tracker;
    private int internalCount;
    private Rigidbody2D body;
    private float dashTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        internalCount = 100;
        body = GetComponent<Rigidbody2D>();
        speed = 10.0f; 
        tracker = GetComponent<Tracker>();
        tracker.setSpeed(speed);
        tracker.setBuffer(stoppingDistance);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        } else
        {
            //Stop the coroutine
            tracker.stopFollowing();
        }
    }

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
