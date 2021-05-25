using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelScript : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (EnemyCounter.areEnemiesAlive())
        {
            return;
        }
        PlayerController player = col.gameObject.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }
        player.backupData();
        SceneChangeData.level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
