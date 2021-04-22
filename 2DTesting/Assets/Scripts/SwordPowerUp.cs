using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPowerUp : PowerUp
{
    public float damageMultiplier = 1f;
    public override void onAttack(PlayerController player, Projectile projectile)
    {
        projectile.incrementDamageMultiplier(damageMultiplier);
    }
}
