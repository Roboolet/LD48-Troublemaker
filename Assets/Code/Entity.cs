using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float hp = 5;
    protected float friction = 0.15f;
    [HideInInspector] public Rigidbody2D rb;

    [SerializeField] protected float movementSpeed;
    public Vector2 velocityRaw { get; private set; }
    [HideInInspector] public Vector2 velocity;

    public Vehicle vehicle;
    SpriteRenderer spr;
    bool dead;
    protected bool invincible;

    protected virtual void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        velocityRaw = Vector2.Lerp(velocityRaw, Vector2.zero, friction);
        velocity = velocityRaw * Time.deltaTime;
        //transform.Translate(velocity, Space.World);       
        rb.velocity = velocityRaw;

        //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);

        FlipSprite();
        //dibble
    }

    public virtual void TakeDamage(float damage)
    {
        if (!invincible)
        {
            hp -= damage;

            if (hp <= 0 && !dead)
            {
                Death();
            }
        }
    }

    protected virtual void FlipSprite()
    {
        if (velocity.x < 0)
        {
            spr.flipX = true;
        }
        else if (velocity.x > 0)
        {
            spr.flipX = false;
        }
    }

    protected virtual void Death()
    {        
        dead = true;
        movementSpeed = 0;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Add raw velocity
    /// </summary>
    /// <param name="vel"></param>
    public void AddVelocity(Vector2 vel)
    {
        velocityRaw += vel;
    }
}
