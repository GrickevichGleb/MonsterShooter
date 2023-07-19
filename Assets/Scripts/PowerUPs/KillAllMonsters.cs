using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillAllMonsters : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionScale = 5f;
    [SerializeField] private float explosionTime = 1f;
    [Space] 
    [SerializeField] private UnityEvent onExplosion;
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Projectile")) return;
        StartCoroutine(Explosion(explosionTime));
    }


    private IEnumerator Explosion(float time)
    {
        onExplosion.Invoke();
        while (explosion.transform.localScale.x < explosionScale)
        {
            Vector3 scale = explosion.transform.localScale;
            scale.x += Time.deltaTime * explosionScale / time;
            scale.y += Time.deltaTime * explosionScale / time;
            scale.z += Time.deltaTime * explosionScale / time;

            explosion.transform.localScale = scale;
            yield return null;
        }
        
        FindObjectOfType<MonsterSpawner>()?.KillAllMonsters();
        FindObjectOfType<PowerupSpawner>()?.powerups.Remove(gameObject);
        Destroy(gameObject);
    }
}
