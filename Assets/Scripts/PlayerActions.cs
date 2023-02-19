using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    GameObject cannonPrefab;

    [Header("Projectile Spawn")]
    [SerializeField]
    GameObject singleShot;
    [SerializeField]
    GameObject tripleShot1, tripleShot2, tripleShot3;

    [Header("Cooldown")]
    [SerializeField]
    float singleShotCooldown;
    [SerializeField]
    float tripleShotCooldown;

    float timerSingleShot = 0f;
    float timerTripleShot = 0f;
    Collider2D col2D;

    private void Start()
    {
        col2D = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (timerSingleShot > 0)
        {
            timerSingleShot = Mathf.Max(0, timerSingleShot - Time.deltaTime);
        }

        if (timerTripleShot > 0)
        {
            timerTripleShot = Mathf.Max(0, timerTripleShot - Time.deltaTime);
        }

    }

    public void SingleShot()
    {
        if(timerSingleShot <= 0)
        {
            Shoot(singleShot, "forward");
            timerSingleShot = singleShotCooldown;
        }
    }

    public void TripleShot()
    {
        if (timerTripleShot <= 0)
        {
            Shoot(tripleShot1, "left");
            Shoot(tripleShot2, "left");
            Shoot(tripleShot3, "left");
            timerTripleShot = tripleShotCooldown;
        }
    }

    void Shoot(GameObject shot, string direction)
    {
        Quaternion rotation = transform.rotation;
        if(direction == "left")
            rotation *= Quaternion.Euler(0, 0, 90);
        GameObject projectile = Instantiate(cannonPrefab, shot.transform.position, rotation);
        Physics2D.IgnoreCollision(col2D, projectile.GetComponent<Collider2D>());
        shot.GetComponent<Animator>().SetTrigger("Shoot");
    }

}
