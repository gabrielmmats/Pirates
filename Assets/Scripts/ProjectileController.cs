using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField]
    LayerMask collisionLayers;
    [SerializeField]
    float initialImpulse = 10f, selfDestroyTime = 10f;

    [Header("Explosion")]
    [SerializeField]
    float damage = 20f;


    Rigidbody2D body2D;
    Collider2D col2D;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        col2D = GetComponent<Collider2D>();
        body2D = GetComponent<Rigidbody2D>();
        body2D.AddForce(transform.up * initialImpulse, ForceMode2D.Impulse);
        Destroy(gameObject, selfDestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        GameObject colliderObject = other.gameObject;
        if (collisionLayers == (collisionLayers | (1 << colliderObject.layer)))
        {
            colliderObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            col2D.enabled = false;
            body2D.velocity= Vector3.zero;
            body2D.angularVelocity = 0;
            body2D.isKinematic = true;
            anim.SetTrigger("destroy");
        }
    }

    void OnEndExplosionAnimation()
    {
        Destroy(gameObject);
    }

}