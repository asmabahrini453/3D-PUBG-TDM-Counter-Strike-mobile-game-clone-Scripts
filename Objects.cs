using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float objectHealth = 100f;
    public void objectHitDamage(float amount)
    {
        objectHealth -= amount;
        if(objectHealth <= 0)
        {
            Die();

        }
    }


    void Die()
    {
        Destroy(gameObject);
    }
}
