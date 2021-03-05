using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Transform player;
    public Transform self;
    public Camera currentCam;
    public Rigidbody2D projectile;
    public Vector2 projectileMovement = new Vector2(50, 50);
    public float speed = 10.0f;
    Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!(gameObject.name.Contains("(Clone)")))
        {

            //Making sure it's not a clone
            if (Input.GetMouseButtonDown(0))
            {
                float selfX = transform.position.x;
                float selfY = transform.position.y;
                mousePos = Input.mousePosition;//gets mouse postion
                mousePos = currentCam.ScreenToWorldPoint(mousePos);
                float mouseX = mousePos.x - selfX;//gets the distance between object and mouse position for x
                float mouseY = mousePos.y - selfY;//gets the distance between object and mouse position for y 
                float x = (mouseX - selfX);
                float y = (mouseY - selfY);

                var projectileInst = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(y, x) * Mathf.Rad2Deg)));
                projectileInst.velocity = new Vector2(speed * x, speed * y);
            }
        }
    }


    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
