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

    private Combatable[] getCombatablesInScene()
    {
        List<Combatable> temp = new List<Combatable>();
        GameObject[] allObjects = (GameObject[])GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject g in allObjects)
        {
            if(g.GetComponent<Combatable>() != null)
            {
                temp.Add(g.GetComponent<Combatable>());
            }
        }
        return temp.ToArray();
    }
    void Update()
    {
        targets = getCombatablesInScene(); //GetComponentsInChildren<Combatable>(); WHY DOESN'T THIS ONE WORK?
    }
}
