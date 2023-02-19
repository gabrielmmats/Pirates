using NavMeshPlus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaserEnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float rotationSpeed = 50f;

    [Header("AI")]
    [SerializeField] 
    float maxPursuitDistance = 5.5f;
    [SerializeField]
    float alertDistance = 3f;

    [Header("Explosion")]
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField] 
    float explosionDamage=40f;

    [Header("Destruction")]
    [SerializeField]
    int scorePoints = 5;

    GameObject target;
    NavMeshAgent agent;
    Vector3 lastPosition;
    Vector3 displacement;
    bool isDestroyed = false;
    Collider2D col2D;
    Collider2D playerCollider;
    Rigidbody2D body2D;
    private float fadeTime;

    int id = -1;

    void Start()
    {
        fadeTime = ConfigManager.Instance.GetRespawnTime();
        agent = gameObject.GetComponent<NavMeshAgent>();
        col2D = gameObject.GetComponent<Collider2D>();
        body2D = gameObject.GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player");
        if (target != null)
            playerCollider = target.GetComponent<Collider2D>();
        agent.updateRotation = false;
        agent.updateUpAxis= false;
        lastPosition = transform.position;
    }

    void Update()
    {
        if (target == null || playerCollider.enabled == false || isDestroyed) {
            if (agent.hasPath)
                agent.ResetPath();
            return;
        } 
        if (agent.hasPath)
        {
            if ((target.transform.position - transform.position).magnitude < maxPursuitDistance)
            {
                agent.SetDestination(target.transform.position);
            }
            else
            {
                agent.ResetPath();
            }
        }
        else if ((target.transform.position - transform.position).magnitude < alertDistance)
        {
            agent.SetDestination(target.transform.position);
        }
        displacement = (transform.position - lastPosition).normalized;
        lastPosition = transform.position;
        if (displacement.magnitude > 0 )
        {
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, displacement);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        GameObject colliderObject = col.gameObject;
        if (playerLayer == (playerLayer | (1 << colliderObject.layer)))
        {
            colliderObject.SendMessage("TakeDamage", explosionDamage, SendMessageOptions.DontRequireReceiver);
            gameObject.SendMessage("OnDeath");
        }
    }

    void OnDestroy()
    {
        GameObject manager = GameObject.Find("MatchManager");
        if (manager != null)
            manager.SendMessage("SetSpawnFree", new Tuple<string, int>("Chaser", id), SendMessageOptions.DontRequireReceiver);
    }

    public void SetId(int number)
    {
        id = number;
    }

    public void DestroyShip(int hp)
    {
        isDestroyed = true;
        Destroy(gameObject, fadeTime);
        col2D.enabled = false;
        body2D.velocity = Vector3.zero;
        body2D.angularVelocity = 0;
        body2D.isKinematic = true;
        if (hp <= 0)
        {
            GameObject manager = GameObject.Find("MatchManager");
            if (manager != null)
                manager.SendMessage("IncreaseScore", scorePoints, SendMessageOptions.DontRequireReceiver);
        }
    }
}
