using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    Animator anim;

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        base.Update();
        AddVelocity(Vector2.ClampMagnitude(input, 1f) * movementSpeed);

        anim.SetFloat("vel", velocityRaw.magnitude);

        if (vehicle != null)
        {
            if (vehicle.driver != this)
            {
                GameManager.main.AggroPolice();
                vehicle.Enter(gameObject);
            }

            vehicle.Drive(input);

            velocity = vehicle.velocity;
            invincible = true;

            if (Input.GetButtonDown("Jump"))
            {
                vehicle.Exit();
                vehicle = null;
            }
        }
        else invincible = false;
    }

    protected override void Death()
    {
        GameManager.main.Particle(transform.position, 0);
        GameManager.main.deathpanel.SetActive(true);

        base.Death();
    }
}
