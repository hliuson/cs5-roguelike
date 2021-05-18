using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemiesDeadPowerUpDropper : MonoBehaviour
{
    public GameObject[] powerUpList;

    [SerializeField]
    public int offset = 5;

    private System.Random rand = new System.Random();

    // Start is called before the first frame update
    void Start()
    {
        powerUpList = Resources.LoadAll<GameObject>("Prefabs/PowerUps");
        EnemyCounter.onEnemiesDied += dropPowerUp;
    }

    private void dropPowerUp()
    {
        int index = rand.Next(powerUpList.Length);
        Instantiate(powerUpList[index], transform.position + new Vector3(0, -7, 0), Quaternion.Euler(0, 0, 0));
    }

    void OnDestroy()
    {
        EnemyCounter.onEnemiesDied -= dropPowerUp;
    }
}
