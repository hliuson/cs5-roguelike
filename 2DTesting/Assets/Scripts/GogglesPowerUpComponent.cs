using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GogglesPowerUpComponent : PowerUpComponent
{
    public override PowerUp getPowerUp()
    {
        return new GogglesPowerUp();
    }
}
