using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float rotationSpeed = 150f;

    [Header("AI")]
    [SerializeField]
    float alertDistance = 4f;
    [SerializeField]
    float maxShootingDistance = 7f;

    [Header("Shooting")]
    [SerializeField]
    LayerMask obstacleLayer;
    [SerializeField]
    float shotAngleThreshold = 0.3f, shotCooldown = 1.5f, lineCastOfffset = 0.1f;
    [SerializeField]
    GameObject cannonPrefab;
    [SerializeField]
    Transform shotPosition;
    

    [Header("Destruction")]
    [SerializeField]
    int scorePoints = 10;

    GameObject target;
    bool hasTarget = false;
    float timerShot = 0;
    Collider2D col2D;
    Collider2D playerCollider;
    Rigidbody2D body2D;
    bool isDestroyed = false;
    float fadeTime;

    int id = -1;


    void Start()
    {
        fadeTime = ConfigManager.Instance.GetRespawnTime();
        target = GameObject.Find("Player");
        if (target != null)
            playerCollider = target.GetComponent<Collider2D>();
        col2D = GetComponent<Collider2D>();
        body2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (target == null || playerCollider.enabled == false || isDestroyed)
            return;
        Vector3 distance = (target.transform.position - transform.position);
        if (!hasTarget && (distance.magnitude < alertDistance))
        {
            hasTarget = true;            
        }
        else if (hasTarget && (distance.magnitude > maxShootingDistance))
        {
            hasTarget = false;
        }
        if (hasTarget)
        {
            float diff = (distance.normalized - transform.up).magnitude;
            if (diff < shotAngleThreshold && timerShot <=0 && !Physics2D.Linecast(shotPosition.position + lineCastOfffset * transform.up, target.transform.position, obstacleLayer))
            {
                Shoot(shotPosition.position);
                timerShot = shotCooldown;
            }
            else
            {
                Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, distance);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            }
        }
        if (timerShot > 0)
        {
            timerShot = Mathf.Max(0, timerShot - Time.deltaTime);
        }
    }

    void OnDestroy()
    {
        GameObject manager = GameObject.Find("MatchManager");
        if (manager != null)
            manager.SendMessage("SetSpawnFree", new Tuple<string, int>("Shooter", id), SendMessageOptions.DontRequireReceiver);
    }

    void Shoot(Vector3 position)
    {     
        GameObject projectile = Instantiate(cannonPrefab, position, transform.rotation);
        Physics2D.IgnoreCollision(col2D, projectile.GetComponent<Collider2D>());
    }

    public void SetId(int number)
    {
        id = number;
    }

    public void DestroyShip()
    {
        isDestroyed = true;
        Destroy(gameObject, fadeTime);
        col2D.enabled = false;
        body2D.angularVelocity = 0;
        body2D.isKinematic = true;
        GameObject manager = GameObject.Find("MatchManager");
        if (manager != null)
            manager.SendMessage("IncreaseScore", scorePoints, SendMessageOptions.DontRequireReceiver);
    }


}
