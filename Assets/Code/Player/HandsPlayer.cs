using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsPlayer : Hands
{
    Player plr;
    Vector3 mousepos;

    void Start()
    {
        plr = body.GetComponent<Player>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousepos.z = body.position.z;

        bodyVelocity = plr.velocity;

        if (Input.GetButton("Fire1") && !plr.vehicle)
        {
            Punch(body, mousepos);
        }
    }
}
