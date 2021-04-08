using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : Combatable
{
    public Combatable spawnPrefab;
    public int spawningRadius;
    private float time;
    private System.Random rand = new System.Random();
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        time = attackCooldownMs;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        time--;
        if(time < 0)
        {
            time = attackCooldownMs;
            attack();
        }
    }

    public override void attack()
    {
        float spawnX = transform.position.x + spawningRadius * (float)(2 * rand.NextDouble() - 1);
        float spawnY = transform.position.y + spawningRadius * (float)(2 * rand.NextDouble() - 1);
        Vector3 position = new Vector3(spawnX, spawnY, 0);
        Instantiate(spawnPrefab, position, Quaternion.identity);
    }

    public override void onDeath(Combatable source)
    {
        die();
    }
}
