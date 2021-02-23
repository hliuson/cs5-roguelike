using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Just hit another collider 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
    }

    //Hitting a collider 2D
    private void OnCollisionStay2D(Collision2D collision)
    {
        //Do something
    }

    //Just stop hitting a collider 2D
    private void OnCollisionExit2D(Collision2D collision)
    {
        //Do something
    }
}
