using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesDeadPowerUpDropper : MonoBehaviour
{
    public UnityAction dropPowerUpAction;
    // Start is called before the first frame update
    void Start()
    {
        dropPowerUpAction = new UnityAction(dropPowerUp);
        EnemyCounter.enemiesDead.addListener(dropPowerUpAction);
    }

    private void dropPowerUp()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
