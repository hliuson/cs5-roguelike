using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy1AI : Enemy
{
    private int internalCount;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        internalCount = 100;
        body = GetComponent<Rigidbody2D>();
        speed = 10.0f;
    }

    //Use fixed update because Update will override what is in Enemy.cs
    private void FixedUpdate()
    {
        //checkAggression();
        if (currentTarget != null)
        {
            internalCount--;
            if (internalCount > 0)
            {
                internalCount = 100;
            }
            /*
            float targetX = 0;
            float targetY = 0;
            float distance = 0;
            if (currentTarget != null)
            {
                targetX = currentTarget.transform.position.x;
                targetY = currentTarget.transform.position.y;
                distance = Vector2.Distance(currentTarget.transform.position, transform.position);
            }
            */
            //body.velocity = new Vector2((transform.position.x - targetX)/10, (transform.position.y - targetY)/10);
            transform.position = Vector2.MoveTowards(body.position, currentTarget.transform.position, speed * Time.fixedDeltaTime);
        }
    } 

    public override void attack() 
    { 

    }

    public override void onDeath(Combatable source)
    {
        die();
    }
}
