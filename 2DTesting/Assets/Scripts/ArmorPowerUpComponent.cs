using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPowerUpComponent : PowerUpComponent
{
    public override PowerUp getPowerUp()
    {
        return new ArmorPowerUp();
    }
}
