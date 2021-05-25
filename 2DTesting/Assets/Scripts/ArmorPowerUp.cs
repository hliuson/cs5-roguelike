using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPowerUp : PowerUp
{
    public override void onPickup(PlayerController player)
    {
        player.armorLevel += 1;
    }

    public override void onRemoval(PlayerController player)
    {
        player.armorLevel += -1;
    }
}
