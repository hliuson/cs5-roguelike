using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Health healthBar; 

    void OnTriggerEnter2D(Collider2D collidedObject)
    {
        GameObject otherObject = collidedObject.gameObject;
        if(otherObject.name == "potion" || otherObject.name == "potion(Clone)")
        {
            healthBar.addHealth(otherObject.GetComponent<PotionScript>().healNum);
            Destroy(otherObject);
        }
        

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
