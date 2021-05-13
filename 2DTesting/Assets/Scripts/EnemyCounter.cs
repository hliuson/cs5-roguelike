using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EnemyCounter
{
    static int enemies = 0;
    public delegate void enemiesDied();
    public static event enemiesDied onEnemiesDied;

    public static void newLevel()
    {
        enemies = 0;
    }

    public static void increment()
    {
        enemies += 1;
    }

    public static void decrement()
    {
        enemies += -1;
        if(enemies == 0)
        {
            onEnemiesDied();
        }
    }
    public static bool areEnemiesAlive()
    {
        return enemies != 0;
    }
}
