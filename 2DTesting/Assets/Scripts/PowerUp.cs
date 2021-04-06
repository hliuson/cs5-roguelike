using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp
{
    public PowerUp()
    {
    }
    public Rarity rarity;
    public abstract void onPickup(PlayerController player);
    public abstract void onRemoval(PlayerController player);
    public abstract void onAttack(PlayerController player, Projectile projectile);
}


public enum Rarity
{
    Common,
    Rare,
    Legendary
}
