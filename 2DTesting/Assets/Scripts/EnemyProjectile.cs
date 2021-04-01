using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{

    public Enemy enemy;
    public Transform self;
    // Start is called before the first frame update
    void Start()
    {
        team = Team.Enemy;
    }

    // Update is called once per frame
    void Update()
    {
        self.position = enemy.transform.position;
    }

    public void shootProjectile()
    {
        if (!(gameObject.name.Contains("(Clone)")))
        {
            float selfX = transform.position.x;
            float selfY = transform.position.y;
            float targetX = enemy.currentTarget.transform.position.x;
            float targetY = enemy.currentTarget.transform.position.y;
            float x = targetX - selfX;//gets the distance between object and target position for x
            float y = targetY - selfY;//gets the distance between object and target position for y 

            var projectileInst = Instantiate(body, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(y, x) * Mathf.Rad2Deg)));
            //projectileInst.velocity = new Vector2(speed * x, speed * y);

        }
    }
}
