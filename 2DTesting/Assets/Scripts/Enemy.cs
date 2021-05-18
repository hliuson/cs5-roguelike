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

    protected override void Start()
    {
        float nodeRadius = ((NavGrid)FindObjectOfType(typeof(NavGrid))).nodeRadius;
        stoppingDistance = (int)(stoppingDistance / nodeRadius);
        base.Start();
    }
    protected void checkAggression()
    {
        Combatable closestEnemy = GetClosestEnemy(targets);
        if (closestEnemy != null)
        {
            if (Vector2.Distance(closestEnemy.transform.position, transform.position) < aggressionRadius)
            {
                currentTarget = closestEnemy;
            }
            else
            {
                currentTarget = null;
            }
        }
        else
        {
            currentTarget = null;
        }
    }

    private Combatable GetClosestEnemy(Combatable[] enemies)
    {
        Combatable closest = null;
        float minDist = Mathf.Infinity;
        Vector2 currentPos = transform.position;
        //Find closest enemy that isn't you
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

    protected void difficulty()
    {
        return 1;
    }

    private void getAllCombatables()
    {
        //Get all objects in the scene of type combatable
        Object[] allObjects = FindObjectsOfType(typeof(Combatable));
        Combatable[] tempArray = new Combatable[allObjects.Length];
        //Turn then into combatables
        for (int g = 0; g < allObjects.Length; g++)
        {
            tempArray[g] = ((Combatable)(allObjects[g])).GetComponent<Combatable>();
        }
        //Make sure they are on the other team
        List<Combatable> enemies = new List<Combatable>();
        foreach(Combatable c in tempArray)
        {
            if(c.team == Team.Player)
            {
                enemies.Add(c);
            }
        }
        targets = enemies.ToArray();
    }

    void Update()
    {
        getAllCombatables();
    }
}
