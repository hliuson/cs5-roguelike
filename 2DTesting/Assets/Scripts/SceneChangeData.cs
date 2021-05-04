using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Stores scene change data in static variables so that they remain constant across classes
public static class SceneChangeData
{
    public static List<PowerUp> playerPowerUps = null;
    public static int maxHp = -1;
    public static int hp = -1;
}
