using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PowerUp : MonoBehaviour
{
    public static Rarity rarity;
    public static string name;
    public abstract void onPickup();
}

public enum Rarity
{
    Common,
    Rare,
    Legendary
}