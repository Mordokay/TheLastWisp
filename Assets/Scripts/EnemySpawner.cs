﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{

    public List<GameObject> enemies;

    public float timeBetweenSpawns;
    public float timeSinceLastSpawn;
    GameObject map;
    GameObject gm;
    int mapX;
    int mapY;
    public int enemyLimit;

    private void Start()
    {
        map = GameObject.FindGameObjectWithTag("Map");
        gm = GameObject.FindGameObjectWithTag("GameManager");
        mapX = map.GetComponent<MapTerrainGenerator>().sizeX;
        mapY = map.GetComponent<MapTerrainGenerator>().sizeY;
        timeSinceLastSpawn = Time.time;
    }

    Vector3 getSpawn() {
        Vector3 spawnPos = Vector3.zero;
        switch (Random.Range(0, 5))
        {
            case 0:
                spawnPos = new Vector3((mapX / 2) - 2, 0.0f, Random.Range(-(mapY / 2), (mapY / 2)));
                break;
            case 1:
                spawnPos = new Vector3(-((mapX / 2) - 2), 0.0f, Random.Range(-(mapY / 2), (mapY / 2)));
                break;
            case 2:
                spawnPos = new Vector3(Random.Range(-(mapX / 2), (mapX / 2)), 0.0f, -((mapY / 2) - 2));
                break;
            case 3:
                spawnPos = new Vector3(Random.Range(-(mapX / 2), (mapX / 2)), 0.0f, ((mapY / 2) - 2));
                break;
        }
        return spawnPos;
    }

    void Update()
    {
        if (gm.GetComponent<PlayerStats>().enemyCount < enemyLimit)
        {
            timeSinceLastSpawn += Time.deltaTime;
            if (timeSinceLastSpawn > timeBetweenSpawns)
            {
                timeSinceLastSpawn = 0;

                Vector3 spawnPos = getSpawn();
                Debug.Log("Spawn pos: " + spawnPos);
                NavMeshHit hit;
                int numTries = 0;
                bool blocked = NavMesh.Raycast(spawnPos, spawnPos + Vector3.down * 40, out hit, NavMesh.AllAreas);
                while (!(!blocked && spawnPos.x < (map.GetComponent<MapTerrainGenerator>().sizeX / 2) &&
                    spawnPos.z < (map.GetComponent<MapTerrainGenerator>().sizeY / 2) &&
                    spawnPos.z > (-map.GetComponent<MapTerrainGenerator>().sizeY / 2) &&
                    spawnPos.x > (-map.GetComponent<MapTerrainGenerator>().sizeX / 2)) && numTries < 5)
                {
                    spawnPos = getSpawn();
                    numTries += 1;
                }
                Instantiate(enemies[Random.Range(0, enemies.Count)], new Vector3(spawnPos.x, 1.0f, spawnPos.z), Quaternion.identity);
                gm.GetComponent<PlayerStats>().enemyCount += 1;
            }
        }
    }
}