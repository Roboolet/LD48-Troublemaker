using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtCollider : MonoBehaviour
{
    [SerializeField] Vehicle self;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8 && Mathf.Abs(self.speed) > 50)
        {
            Entity e = collision.gameObject.GetComponent<Entity>();

            float damage = Mathf.Abs(self.rb.velocity.magnitude) * 0.1f;
            e.TakeDamage(damage);
            self.TakeDamage(damage * 0.2f);
        }
    }
}
