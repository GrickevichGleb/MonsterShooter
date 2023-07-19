using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    [SerializeField] private int nMonstersLimit = 10;
    [SerializeField] private GameObject[] monstersPrefabs;

    public bool isSpawning = true;
    public int monstersLevel = 1;
    public float startTrySpawnInterval = 3f;
    public float spawnProbability = 1f;
    public float trySpawnInterval;
    public List<GameObject> monsters = new List<GameObject>();
    
    
    private float _monsterSpawnTimer = 0f;

    private Coroutine _currentSuspendSpawnCoroutine;


    private void Start()
    {
        trySpawnInterval = startTrySpawnInterval;
    }


    void Update()
    {
        TimersUpdate();
        
        SpawnMonster();
    }
    
    // Powerups Interaction
    public void SuspendSpawnForSeconds(float seconds)
    {
        _currentSuspendSpawnCoroutine = StartCoroutine(SuspendSpawn(seconds));
    }

    public void KillAllMonsters()
    {
        foreach (GameObject monster in monsters)
        {
            monster.GetComponent<MonsterController>()?.TakeDamage(500);
        }
        
    }
    

    private void SpawnMonster()
    {
        if (monstersPrefabs.Length == 0) return;
        if (!isSpawning) return;
        
        if (_monsterSpawnTimer >= trySpawnInterval)
        {
            _monsterSpawnTimer = 0f;

            if (Random.Range(0f, 1f) >= spawnProbability) return;
            
            if (monsters.Count >= nMonstersLimit)
                return;
            
            GameObject monsterInstance = 
                Instantiate(monstersPrefabs[Random.Range(0, monstersPrefabs.Length)],
                     GetRandSpawnPos(),
                            Quaternion.identity);
            monsterInstance.GetComponent<MonsterController>().
                SetMonsterLevel(monstersLevel);
            monsterInstance.GetComponent<MonsterController>().OnMonsterDeath += MonsterDeathHandler;
            
            monsters.Add(monsterInstance);
            
            GameController.instance.SetNumOfMonstersOnScreen(monsters.Count);
        }
    }


    private void MonsterDeathHandler(GameObject monster)
    {
        monster.GetComponent<MonsterController>().OnMonsterDeath -= MonsterDeathHandler;
        monsters.Remove(monster);
        Destroy(monster);
        
        GameController.instance.MonstersKilledIncr();
        GameController.instance.SetNumOfMonstersOnScreen(monsters.Count);
        
        Debug.Log("Monster is dead");
    }

    private Vector3 GetRandSpawnPos()
    {
        return GameController.instance.GetRandFieldPos();
    }
    
    private void TimersUpdate()
    {
        _monsterSpawnTimer += Time.deltaTime;
    }

    private IEnumerator SuspendSpawn(float seconds)
    {
        if(_currentSuspendSpawnCoroutine != null)
            StopCoroutine(_currentSuspendSpawnCoroutine);
        
        isSpawning = false;
        yield return new WaitForSeconds(seconds);
        isSpawning = true;
    }
    
}
