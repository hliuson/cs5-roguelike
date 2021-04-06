using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : PowerUp
{
    
    public int healNum = 2;

    public Potion()
    {
        this.rarity = Rarity.Common;
    }
    public override void onPickup(PlayerController player)
    {
        player.health += healNum;
        if(player.health > player.maxHealth)
        {
            player.health = player.maxHealth;
        }
        player.removePowerUp(this);    
        //Increase health
        //I just use on collision instead
    }

    public override void onRemoval(PlayerController player)
    {
        return;
    }

    public override void onAttack(PlayerController player, Projectile projectile)
    {
        return;
    }
}
