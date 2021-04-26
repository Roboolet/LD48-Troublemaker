using Assets.Code.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policeman : Entity, AIBase
{
    Animator anim;
    public AIState ai { get; set; }
    [SerializeField] AIState startAi;
    Transform player;
    Vector2 dist;
    [SerializeField] Hands hands;

    protected override void Start()
    {
        base.Start();

        GameManager.main.police.Add(this);

        ai = startAi;
        anim = GetComponent<Animator>();

        friction = 0.05f;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Update()
    {
        base.Update();

        if (vehicle != null)
        {
            if (vehicle.driver != this)
            {
                vehicle.Enter(gameObject);
            }
            vehicle.Drive(Vector2.up);

            invincible = true;
        }
        else invincible = false;

        if (player != null)
        {
            dist = player.position - transform.position;
        }
        hands.bodyVelocity = velocity;

        switch (ai)
        {
            default: player = null;
                break;
            case AIState.idle: IdleBehaviour(); break;
            case AIState.aggro: AggroBehaviour(); break;
            case AIState.search: SearchBehaviour(); break;
        }

        anim.SetFloat("vel", velocityRaw.magnitude);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        ai = AIState.aggro;

    }

    protected override void Death()
    {
        ai = AIState.dead;
        GameManager.main.Particle(transform.position, 0);
        GameManager.main.AggroPolice();
        GameManager.main.RespawnPolice(this);
        GameManager.main.AddScore(356);

        base.Death();
    }

    public void IdleBehaviour()
    {
        if(dist.magnitude <= 4f)
        {
            AddVelocity(-dist.normalized * movementSpeed);
            if(dist.magnitude <= 3f)
            {
                ai = AIState.aggro;
            }
        }
    }

    public void AggroBehaviour()
    {

        if (dist.magnitude >= 3f)
        {
            AddVelocity(dist.normalized * movementSpeed);
        }
        else
        {
            hands.Punch(transform, player.position);
        }
    }

    public void SearchBehaviour()
    {
        IdleBehaviour();
    }
}
