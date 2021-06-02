using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GogglesPowerUp : PowerUp
{
    public float speedMultiplier = 0.5f;
    public override void onAttack(PlayerController player, Projectile projectile)
    {
        projectile.incrementSpeedMultiplier(speedMultiplier);
    }
}
