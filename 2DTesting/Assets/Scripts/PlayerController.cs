using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : Combatable {

    public Health healthbar;
    private GameObject tempOver;
    private Canvas gameOver;

    public Animator animator;

    float horizontalPress, verticalPress;

    float horizontalSpeed = 0, verticalSpeed = 0;

    private const float diagonalLimit = 0.707f;
    private float speed = 10.0f;
    public float acceleration = 2.0f;
    public const float regSpeed = 20.0f;
    private const float diagSpeed = diagonalLimit * regSpeed;

    public const float dashMax = 3.0f;
    private float dashMulti = 1.0f;
    public const float dashSpeed = 30.0f;
    public const int dashDuration = 20;

    private float speedMultiplier = 1f;
    private float cooldownMultiplier = 1f;

    int dashTime = 0;
    List<PowerUp> powerUpList;

    [SerializeField]
    public GameObject projectile;

    public float attackTime;
    public float startTimeAttack;
    public float attackRange;
    public float meleeDamage;
    public float knockback;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        healthbar = (Health)FindObjectOfType(typeof(Health));
        tempOver = GameObject.Find("Game Over Canvas");
        tempOver.SetActive(false);
        this.powerUpList = new List<PowerUp>();
        this.loadData();
    }

    private void FixedUpdate() {
        int horizontalDirection = Math.Sign(horizontalSpeed);//(int)((horizontalSpeed / Math.Abs(horizontalSpeed)));
        int verticalDirection = Math.Sign(verticalSpeed);//(int)((verticalSpeed / Math.Abs(verticalSpeed)));

        if (horizontalPress != 0 && verticalPress != 0) {
            //horizontalSpeed *= diagonalLimit;
            //verticalSpeed *= diagonalLimit;
            speed = diagSpeed;
        } else if (horizontalPress == 0 && verticalPress == 0)
        {
            horizontalSpeed = (float)Math.Floor(horizontalSpeed);
            verticalSpeed = (float)Math.Floor(verticalSpeed);
            //Because we can't have nice things, and also I can't do math right now
            if (Math.Abs(horizontalSpeed) == 1) { horizontalSpeed = 0; }
            if (Math.Abs(verticalSpeed) == 1) { verticalSpeed = 0; }
        } else
        {
            speed = regSpeed;
        }

        dashTime--;
        if (dashTime >= 0)
        {
            speed = dashSpeed;
            dashMulti = dashMax;
        } else
        {
            dashMulti = 1.0f;
        }

        if (horizontalSpeed > speed)
        {
            horizontalSpeed = speed;
        }

        if (verticalSpeed > speed)
        {
            verticalSpeed = speed;
        }

        if ((horizontalPress == 0) && Math.Abs(horizontalSpeed) > 0)
        {
            horizontalSpeed -= horizontalDirection * acceleration;
        }
        else if ((horizontalPress == -horizontalDirection) && Math.Abs(horizontalSpeed) > 0)
        {
            horizontalSpeed -= horizontalDirection * 2 * acceleration;
        }
        else if (Math.Abs(horizontalSpeed) < speed)
        {
            horizontalSpeed += horizontalPress * acceleration;
        }

        if (verticalPress == 0 && Math.Abs(verticalSpeed) > 0)
        {
            verticalSpeed -= verticalDirection * acceleration;
        }
        else if ((verticalPress == -verticalDirection) && Math.Abs(verticalSpeed) > 0)
        {
            verticalSpeed -= verticalDirection * 2 * acceleration;
        }
        else if (Math.Abs(verticalSpeed) < speed)
        {
            verticalSpeed += verticalPress * acceleration;
        }

         this.body.velocity = new Vector2(horizontalSpeed * dashMulti * speedMultiplier, verticalSpeed*dashMulti* speedMultiplier);
    }

    // Update is called once per frame
    void Update() {
        horizontalPress = Input.GetAxisRaw("Horizontal");
        verticalPress = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Speed", horizontalPress);
        animator.SetFloat("VSpeed", verticalPress);

        this.body.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (Input.GetKey("space") && dashTime < -50)
        {
            dashTime = dashDuration;
        }

        //if (Input.GetMouseButtonDown(0))
        if(Input.GetMouseButton(0)) //Allows users to hold mouse down to attack
        {
            this.tryAttack();
        }


        //Melee attack stuff
        if (Input.GetMouseButtonDown(1))
        {
            this.tryAttack2();
            animator.SetBool("MeleeAttack", true);
        }
        else
        {
            animator.SetBool("MeleeAttack", false);
        }

        healthbar.health = (int)health;
    }

    public override void onDeath(Combatable source)
    {
        Debug.Log("Died");
        SceneChangeData.reset();
        Time.timeScale = 0.0f;
        tempOver.SetActive(true);
    }

    public override void attack2()
    {
        Collider2D[] damage = Physics2D.OverlapCircleAll(body.transform.position, attackRange);

        for (int i = 0; i < damage.Length; i++)
        {
            Combatable entity = damage[i].gameObject.GetComponent<Combatable>();
            if (entity != null && entity.getTeam() != this.team)
            {
                print("Attacked");
                Vector2 knockbackDirection = entity.transform.position - transform.position; ;
                entity.takeDamage(meleeDamage, knockback, knockbackDirection, this);
            }
        }
    }

    public override void attack()
    {
        Transform transform = this.transform;

        Camera currentCam = transform.Find("PlayerCamera").gameObject.GetComponent<Camera>();
        float selfX = transform.position.x;
        float selfY = transform.position.y;
        Vector2 mousePos = Input.mousePosition;//gets mouse postion
        mousePos = currentCam.ScreenToWorldPoint(mousePos);
        float x = mousePos.x - selfX;//gets the distance between object and mouse position for x
        float y = mousePos.y - selfY;//gets the distance between object and mouse position for y

        var projectileInst = Instantiate(projectile, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(y, x) * Mathf.Rad2Deg)));
        Vector2 vel = new Vector2(x, y).normalized;

        Projectile proj = projectileInst.GetComponent<Projectile>();


        projectileInst.GetComponent<Rigidbody2D>().velocity = vel * proj.speed;
        proj.source = this;

        foreach (PowerUp p in this.powerUpList)
        {
            p.onAttack(this, proj);
        }
    }

    public void backupData()
    {
        SceneChangeData.playerPowerUps = this.powerUpList;
        SceneChangeData.maxHp = this.maxHealth;
        SceneChangeData.hp = this.health;
    }

    public void loadData()
    {
        if(SceneChangeData.playerPowerUps == null)
        {
            return;
        }
        this.powerUpList = SceneChangeData.playerPowerUps;
        foreach (PowerUp p in this.powerUpList)
        {
            p.onPickup(this);
        }
        this.maxHealth = SceneChangeData.maxHp;
        this.health = SceneChangeData.hp;
    }

    public void removePowerUp(PowerUp powerUp)
    {
        powerUp.onRemoval(this);
        this.powerUpList.Remove(powerUp);
    }
    public void gainPowerUp(PowerUp powerUp)
    {
        this.powerUpList.Add(powerUp);
        powerUp.onPickup(this);
    }

    public void incrementSpeedMultiplier(float increment)
    {
        this.speedMultiplier += increment;
    }

    public override float attackCooldown()
    {
        return this.attackCooldownMs * this.cooldownMultiplier;
    }

    public void multiplyCooldownMultiplier(float multiplier)
    {
        this.cooldownMultiplier *= multiplier;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
