using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public Rigidbody2D body;

    [SerializeField]
    public float damage;

    [SerializeField]
    public float knockback;

    [SerializeField]
    public float speed;

    [SerializeField]
    public Team team;

    public StatusEffect[] effects;
    public Combatable source;

    void Start()
    {
        this.body = GetComponent<Rigidbody2D>();
    }

    public void onCollision(Combatable entity)
    {

    }

    public void FixedUpdate()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        Projectile proj = col.gameObject.GetComponent<Projectile>();
        if (proj != null)
        {
            return;
        }

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
        if(this.team == Team.Enemy)
        {
            print(entity.getTeam());
        }
        //TODO: Inflict Damage, knockb/ack, status effects
        Vector2 knockbackDirection = body.velocity;
        entity.takeDamage(this.damage, this.knockback, knockbackDirection, this.source);
        Destroy(this.gameObject);
    }

}
