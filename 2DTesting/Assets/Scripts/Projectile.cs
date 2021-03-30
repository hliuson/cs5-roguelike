using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    public float damage;

    [SerializeField]
    public float knockback;

    [SerializeField]
    public float speed;

    [SerializeField]
    public Team team;

    public StatusEffect[] effects;
    public void onCollision(Combatable entity)
    {

    }

    public void FixedUpdate()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {

        Combatable entity = col.gameObject.GetComponent<Combatable>();
        if (entity == null)
        {
            Destroy(this.gameObject);
            return;
        }

        if (entity.getTeam() == this.team)
        {
            return;
        }

        //TODO: Inflict Damage, knockback, status effects
        Destroy(this.gameObject);
    }

}
