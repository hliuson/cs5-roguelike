using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpeedPowerUpComponent : PowerUpComponent
{
    public override PowerUp getPowerUp()
    {
        return new AttackSpeedPowerUp();
    }
}
