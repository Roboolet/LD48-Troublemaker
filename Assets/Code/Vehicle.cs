using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vehicle : Entity
{
    public GameObject driver { get; private set; }
    [SerializeField] float accel;
    [SerializeField] float maxspeed;
    [HideInInspector] public float speed;
    [SerializeField] float maxTurnPower;

    [SerializeField] Transform exitPoint;

    protected override void Start()
    {
        base.Start();
        friction = 0.75f;
    }

    protected override void Update()
    {
        base.Update();

        if(driver == null)
        {
            Drive(Vector2.zero);
        }
    }

    protected override void Death()
    {
        if (driver.CompareTag("Player"))
        {
            GameManager.main.deathpanel.SetActive(true);
        }

        GameManager.main.Particle(transform.position, 1);
        GameManager.main.AddScore(240);

        base.Death();
    }

    protected override void FlipSprite() { }

    public virtual void Enter(GameObject newDriver)
    {
        if (driver == null || driver == newDriver)
        {
            driver = newDriver;
            driver.transform.position = transform.position;
            driver.transform.SetParent(transform);
            driver.GetComponent<Collider2D>().enabled = false;
        }
    }

    public virtual void Exit()
    {
        driver.transform.SetParent(transform.parent);
        driver.transform.position = exitPoint.position;
        driver.transform.rotation = Quaternion.identity;
        driver.GetComponent<Collider2D>().enabled = true;
        driver = null;
    }

    public virtual void Drive(Vector2 input)
    {
        if(rb.velocity.magnitude <= 6f && input.magnitude <= 0.1f)
        {
            speed = 0;
        }

        if(input.y == 0)
        {
            if (speed > 0) speed -= Time.deltaTime*10;
            else if (speed < 0) speed += Time.deltaTime*10;
        }
        speed += accel * input.y * Time.deltaTime;
        speed = Mathf.Clamp(speed, -maxspeed, maxspeed);

        float turnvel = Mathf.Clamp(-input.x * speed * 0.01f, -maxTurnPower, maxTurnPower);

        transform.Rotate(Vector3.forward * turnvel);
        AddVelocity(transform.right * speed);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != null && !collision.CompareTag("Vehicle"))
        {
            Entity e = collision.gameObject.GetComponent<Entity>();

            if (driver == null && e != null && speed < 20)
            {
                Enter(e.gameObject);
                e.vehicle = this;
            }
        }
    }
}
