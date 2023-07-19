using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    
    
    [SerializeField] private GameObject field;
    [Space] 
    [SerializeField] private int killsToMonstersLevelUp;
    [SerializeField] private int monstersMaxLevel;
    [Space] 
    [SerializeField] private int addedDamagePerLevel = 5;
    [SerializeField] private float addedFireRatePerLevel = 2f;
    [Space]
    [SerializeField] private GameObject gameOverScreen;
    
    private MonsterSpawner _monsterSpawner;
    
    public int _monstersOnScreen = 0;
    public int _monstersKilledTotal = 0;
    public int _monstersKilledTimer = 0;
    
    public bool isGameOver = false;
    
    private const string resKey = "Result";
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _monsterSpawner = GetComponent<MonsterSpawner>();
    }


    private void Update()
    {
        if (isGameOver) return;
        
        if (_monstersOnScreen >= 10)
        {
            GameOver();
        }
        
        if (_monstersKilledTimer >= killsToMonstersLevelUp)
        {
            LevelUpGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
        }
    }


    public void SetNumOfMonstersOnScreen(int nMonsters)
    {
        _monstersOnScreen = nMonsters;
    }
    
    public void MonstersKilledIncr()
    {
        _monstersKilledTotal += 1;
        _monstersKilledTimer += 1;
    }
    

    public Vector3 GetRandFieldPos()
    {
        Vector3 fieldSize = field.GetComponent<BoxCollider>().size;
        if (fieldSize.x < 2 || fieldSize.z < 2)
            return field.transform.position;
        
        //Adding indent from borders
        Vector3 spawnAreaSize = new Vector3(fieldSize.x - 2, 0f, fieldSize.z - 2);
        
        float posX = field.transform.position.x + Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
        float posY = field.transform.position.y;
        float posZ = field.transform.position.z + Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2);

        return new Vector3(posX, posY, posZ);
    }


    private void LevelUpGame()
    {
        _monstersKilledTimer = 0;
        Debug.Log("Level Up Game");
        
        // Increasing monsters stats
        _monsterSpawner.monstersLevel = Mathf.Min(_monsterSpawner.monstersLevel + 1, monstersMaxLevel);
        _monsterSpawner.trySpawnInterval = Mathf.Max(
            _monsterSpawner.trySpawnInterval - _monsterSpawner.startTrySpawnInterval * 0.1f,
            1);// Reduce by 10% on each levelUp but cant be less than 1second

        // Increasing player stats
        GameObject.FindWithTag("Player").GetComponent<ProjectileLauncher>().fireRate += addedFireRatePerLevel;
        GameObject.FindWithTag("Player").GetComponent<ProjectileLauncher>().damage += addedDamagePerLevel;
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        isGameOver = true;
        SaveResults();
        gameOverScreen.SetActive(true);
        GetComponent<MonsterSpawner>().isSpawning = false;
        GetComponent<PowerupSpawner>().isSpawning = false;
    }


    private void SaveResults()
    {
        //Consider we will keep only 5 top results
        List<int> resultsList = new List<int>();
        // Filling up list
        for (int i = 0; i < 5; i++)
        {
            int result = PlayerPrefs.GetInt(resKey + i, 0);
            resultsList.Add(result);
            
            Debug.Log("Filled list with: " + result);
        }
        // Adding new result and sorting
        resultsList.Add(_monstersKilledTotal);
        resultsList.Sort();
        resultsList.Reverse();
        
        // Writing new top 5 values
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.SetInt(resKey+i, resultsList[i]);
            Debug.Log("Set "+ resKey+i + " " + resultsList[i]);
        }
        
    }
    
}
