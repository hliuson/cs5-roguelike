using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPowerUp : PowerUp
{
    private float multiplier = 0.5f;
    public override void onPickup(PlayerController player)
    {
        player.multiplyCooldownMultiplier(multiplier);
    }

    public override void onRemoval(PlayerController player)
    {
        player.multiplyCooldownMultiplier(1 / multiplier);
    }
}
