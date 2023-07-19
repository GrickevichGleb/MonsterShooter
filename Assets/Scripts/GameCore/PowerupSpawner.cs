using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    [SerializeField] private int maxPowerupsOnScreen = 1;
    [SerializeField] private float trySpawnInterval = 10;
    [SerializeField] private float spawnProbability = 0.5f;
    [SerializeField] private GameObject[] powerupsPrefabs;

    public bool isSpawning = true;
    public List<GameObject> powerups = new List<GameObject>();

    private float _powerupSpawnTimer;

    // Start is called before the first frame update
    void Start()
    {
    }


    void Update()
    {
        TimersUpdate();

        SpawnPowerUp();
    }


    private void SpawnPowerUp()
    {
        if (!isSpawning) return;
        if (powerupsPrefabs.Length == 0) return;
        if (powerups.Count >= maxPowerupsOnScreen) return;
        
        if (_powerupSpawnTimer >= trySpawnInterval)
        {
            if (Random.Range(0f, 1f) >= spawnProbability)
                return;
            
            _powerupSpawnTimer = 0f;

            GameObject powerup = Instantiate(
                powerupsPrefabs[Random.Range(0, powerupsPrefabs.Length)],
                GameController.instance.GetRandFieldPos(),
                Quaternion.identity
            );
            
            powerups.Add(powerup);
        }
    }

    private void TimersUpdate()
    {
        _powerupSpawnTimer += Time.deltaTime;
    }
}