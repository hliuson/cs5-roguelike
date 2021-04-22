using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsPowerUp : PowerUp
{
    public float speedMultiplier = 1f;
    public override void onPickup(PlayerController player)
    {
        player.incrementSpeedMultiplier(speedMultiplier);
    }

    public override void onRemoval(PlayerController player)
    {
        player.incrementSpeedMultiplier(-speedMultiplier);
    }
}
