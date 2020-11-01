using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] enemyObjects;
    public Transform[] spawnPoints;

    public float currSpawnDelay;
    public float maxSpawnDelay;

    void Update()
    {
        currSpawnDelay += Time.deltaTime;
        if (currSpawnDelay > maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 3f);
            currSpawnDelay = 0;
        }
    }

    void SpawnEnemy()
    {
        int randomEnemy = Random.Range(0, 3);
        int randomPoint = Random.Range(0, 5);
        Instantiate(
                enemyObjects[randomEnemy], 
                spawnPoints[randomPoint].position, 
                spawnPoints[randomPoint].rotation
            );
    }
}