using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionComponent : PowerUpComponent
{
    public override PowerUp getPowerUp()
    {
        return new Potion();
    }
}
