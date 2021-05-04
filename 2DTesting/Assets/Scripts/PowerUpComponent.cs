using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PowerUpComponent : MonoBehaviour
{
    public PowerUp powerUp;
    public Rigidbody2D body;
    void Start()
    {
        this.powerUp = this.getPowerUp();
        this.body = GetComponent<Rigidbody2D>();
    }

    public abstract PowerUp getPowerUp();

    public void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController player = col.gameObject.GetComponent<PlayerController>();
        if (player == null)
        {
            return;
        }
        player.gainPowerUp(this.powerUp);
        this.gameObject.SetActive(false);
    }
}
