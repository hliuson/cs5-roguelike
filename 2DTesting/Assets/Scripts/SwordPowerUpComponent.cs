using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordPowerUpComponent : PowerUpComponent
{
    public override PowerUp getPowerUp()
    {
        return new SwordPowerUp();
    }
}
