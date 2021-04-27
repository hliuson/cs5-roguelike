using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsPowerUpComponent : PowerUpComponent
{
    public override PowerUp getPowerUp()
    {
        return new BootsPowerUp();
    }
}
