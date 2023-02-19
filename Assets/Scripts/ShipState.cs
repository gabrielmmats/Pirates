using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipState : MonoBehaviour
{
    [Header("Health")]
    [SerializeField]
    float maxHP = 100f;
    [SerializeField]
    float damagedHPRatio = 0.6f, criticalHPRatio = 0.3f;
    [SerializeField]
    GameObject healthBar;

    [Header("Effects")]
    [SerializeField]
    GameObject flames;
    [SerializeField]
    GameObject explosion, cannon;

    float hp;
    Animator animator;

    void Start()
    {
        hp = maxHP;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(hp-damage, 0);
        if (hp <= 0)
        {            
            OnDeath();
        }
        else
        {
            healthBar.GetComponent<HealthBar>().SetBarSize(Mathf.Clamp01(hp / maxHP));
            if (hp <= criticalHPRatio * maxHP && hp + damage > criticalHPRatio * maxHP)
            {
                animator.SetTrigger("Critical");
                if (flames != null)
                {
                    flames.SetActive(true);
                }
            }
            else if (hp <= damagedHPRatio * maxHP && hp + damage > damagedHPRatio * maxHP)
            {
                animator.SetTrigger("Damaged");
            }
        }
    }

    public void OnDeath()
    {
        healthBar.SetActive(false);
        flames.SetActive(false);
        explosion.GetComponent<Animator>().SetTrigger("Explode");
        if (cannon != null)
            cannon.SetActive(false);
        animator.SetTrigger("Destroyed");
        gameObject.SendMessage("DestroyShip", hp, SendMessageOptions.DontRequireReceiver);
        hp = 0;

    }
}
