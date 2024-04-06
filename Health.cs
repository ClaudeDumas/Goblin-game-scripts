using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 100;
    public float immunityFrames = 0;
    [SerializeField] float maxImmunityFrames = 50;
    [SerializeField] float maxHealth;
    [SerializeField] float regeneration;
    void Damage(float damage)
    {
        if (immunityFrames == 0)
        {
            health -= damage;
            immunityFrames = maxImmunityFrames;
            if (health <= 0){
                Destroy(transform.gameObject);
            }
        }
    }
    void Heal(float healing){
        if ((health + healing) > maxHealth){
            health += healing;
        }
    }
    void FixedUpdate(){
        if (immunityFrames > 0){
            immunityFrames -= 1;
        }
        if (health < maxHealth){
            health += regeneration;
            if (health > maxHealth){
                health = maxHealth;
            }
        }
    }
}
