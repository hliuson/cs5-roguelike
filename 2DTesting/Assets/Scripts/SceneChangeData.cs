using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Stores scene change data in static variables so that they remain constant across classes
public static class SceneChangeData
{
    public static List<PowerUp> playerPowerUps = null;
    public static float maxHp = -1;
    public static float hp = -1;
    public static int level = 1;

    public static void reset()
    {
        playerPowerUps = null;
        maxHp = -1;
        hp = -1;
        level = 1;
    }
}
