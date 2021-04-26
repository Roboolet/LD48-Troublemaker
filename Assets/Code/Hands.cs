using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hands : MonoBehaviour
{
    [SerializeField] protected Transform body;
    [SerializeField] Transform[] handsIn;
    [SerializeField] float punchRadius;
    [SerializeField] float punchForce;
    [SerializeField] float punchInterval;
    [SerializeField] ContactFilter2D lm;

    protected Hand[] hand;
    [HideInInspector] public Vector3 bodyVelocity = Vector3.zero;

    protected int lastAttack = 0;
    protected float lastAttackTime = 0;

    protected virtual void Awake()
    {
        hand = new Hand[handsIn.Length];
        for (int i = 0; i < handsIn.Length; i++) 
        {
            hand[i] = new Hand(handsIn[i], handsIn[i].localPosition);
        }
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {       
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].tf.position = Vector2.Lerp(hand[i].tf.position, body.position + hand[i].offset, 0.12f);
        }

        if(!body.gameObject.activeInHierarchy)
        {
            Destroy(gameObject);
            this.enabled = false;
        }
    }

    public virtual void Punch(Transform body ,Vector3 target)
    {
        if (Time.time >= lastAttackTime + punchInterval)
        {
            lastAttackTime = Time.time;
            lastAttack++;
            if (lastAttack >= hand.Length) lastAttack = 0;


            Vector2 direction = target - body.position;
            hand[lastAttack].tf.Translate(direction.normalized * punchRadius);

            RaycastHit2D[] results = new RaycastHit2D[3];
            Physics2D.Raycast(body.position, direction, lm, results, punchRadius + 0.75f);
            foreach (RaycastHit2D hit in results)
            {
                if (hit.transform != null && hit.transform != body)
                {
                    Entity hitE = hit.transform.gameObject.GetComponent<Entity>();
                    hitE.TakeDamage(1);
                    hitE.AddVelocity(direction.normalized * punchForce);
                }
            }

        }
    }

    protected struct Hand
    {
        public Transform tf;
        public Vector3 offset;
        public Hand(Transform tfin, Vector3 offsetin)
        {
            tf = tfin;
            offset = offsetin;
        }
    }
}

