using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Create custom event when some arguments needs to be passed with it
// Skip this if argument passing is not required and use UnityEvent instead
[System.Serializable]
public class EnemiesDeadEvent : UnityEvent
{
}
