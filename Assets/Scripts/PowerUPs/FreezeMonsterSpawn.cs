using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FreezeMonsterSpawn : MonoBehaviour
{
    [SerializeField] private float spawnSuspendTime;
    [Space]
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionScale = 5f;
    [SerializeField] private float explosionTime = 1f;
    [Space] 
    [SerializeField] private UnityEvent onFreeze;
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Projectile")) return;

        StartCoroutine(Explosion(explosionTime));
    }
    
    
    private IEnumerator Explosion(float time)
    {
        onFreeze.Invoke();
        while (explosion.transform.localScale.x < explosionScale)
        {
            Vector3 scale = explosion.transform.localScale;
            scale.x += Time.deltaTime * explosionScale / time;
            scale.y += Time.deltaTime * explosionScale / time;
            scale.z += Time.deltaTime * explosionScale / time;

            explosion.transform.localScale = scale;
            yield return null;
        }
        
        FindObjectOfType<MonsterSpawner>()?.SuspendSpawnForSeconds(spawnSuspendTime);
        FindObjectOfType<PowerupSpawner>()?.powerups.Remove(gameObject);
        Destroy(gameObject);
    }
}
