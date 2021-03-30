using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffect
{
    public float duration;
    public abstract void inflictEffect();
    public abstract void effectExpires();
}
