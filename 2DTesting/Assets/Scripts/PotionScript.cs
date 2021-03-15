using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : PowerUp
{
    [SerializeField]
    private const int healNum = 2; 

    public Health healthBar;

    public override void onPickup()
    {
        //Increase health
        //I just use on collision instead
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Hit");
        healthBar.addHealth(healNum);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
