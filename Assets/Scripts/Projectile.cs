using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float destroyDelay = 5f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * launchForce;
        
        Destroy(gameObject, destroyDelay);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(gameObject);
        }
        
        if (other.gameObject.TryGetComponent<MonsterController>(out MonsterController monster))
        {
            monster.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
