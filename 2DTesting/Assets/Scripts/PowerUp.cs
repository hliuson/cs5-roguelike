using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp
{
    public PowerUp()
    {
    }
    public Rarity rarity;

    public virtual void onPickup(PlayerController player)
    {

    }

    public virtual void onRemoval(PlayerController player)
    {

    }

    public virtual void onAttack(PlayerController player, Projectile projectile)
    {

    }
}


public enum Rarity
{
    Common,
    Rare,
    Legendary
}
