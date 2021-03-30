using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Combatable
{
    //For two players or anything helping player
    public Combatable[] targets;
    public float aggressionRadius;
    public Combatable currentTarget;
    public float speed;
    public int stoppingDistance; //How far away you want the enemy to stop from the player, to attack or other things
    protected void checkAggression()
    {
        Combatable closestEnemy = GetClosestEnemy(targets);
        if (Vector2.Distance(closestEnemy.transform.position, transform.position)  < aggressionRadius)
        {
            currentTarget = closestEnemy;
        } else
        {
            currentTarget = null;
        }
    }

    private Combatable GetClosestEnemy(Combatable[] enemies)
    {
        Combatable closest = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        foreach (Combatable c in enemies)
        {
            float dist = Vector2.Distance(c.transform.position, currentPos);
            if (dist < minDist && dist > 0)
            {
                closest = c;
                minDist = dist;
            }
        }
        return closest;
    }

    private void getAllCombatables()
    {
        //Get all objects in the scene
        Object[] allObjects = FindObjectsOfType(typeof(Combatable));
        Combatable[] tempArray = new Combatable[allObjects.Length];
        for (int g = 0; g < allObjects.Length; g++)
        {
            tempArray[g] = ((Combatable)(allObjects[g])).GetComponent<Combatable>();
        }
        targets = tempArray;
    }

    void Update()
    {
        getAllCombatables();
    }
}
