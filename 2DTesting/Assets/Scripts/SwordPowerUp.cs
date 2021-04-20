using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPowerUp : PowerUp
{
    public override void onAttack(PlayerController player, Projectile projectile)
    {
        projectile.damage += 1; //TODO: make this with a method instead
    }
}
