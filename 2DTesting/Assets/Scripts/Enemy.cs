using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Combatable
{
    public float aggressionRadius;
    public Combatable currentTarget;
    public float speed;
    public void checkAggression()
    {
        //check for player withing aggression radius
        //assign player as currentTarget
    }
}
