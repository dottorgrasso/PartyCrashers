﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SpawnEnemy : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    public int limitedSpawnTotalNum = 4;
    public int limitedSpawnCurrentNum = 0;
    public bool infiniteSpawn;
    public int infiniteSpawnInMapNum = 5;
    public int infiniteSpawnCurrentNum;
    GameObject target;
    NavMeshAgent agent;
    GameObject[] players;
    float x;
    float y;
    float z;
    Vector3 RandomLocation;
    public float SpawnRange = 5f;
    public float GodRadius = 3f;
    //---------------------------------------------------
    public GameObject enemyPrefab;
    public float timer;
    public float range;
    public float spawnTime = 5.0f;
    public float activedRange = 10f;
    // run away
    public float m_Distance;
    Vector3 MoveDirection;
    Vector3 RunAwayDirection;
    public float RunAwayRange = 5f;
    public float RunSpeed = 0.01f;

    EnemyAI enemyAi;
    EnemyEffect enemyEffect;

    void Start()
    {
        players = GameManager.m_Instance.m_Players;
        timer = spawnTime;
        agent = gameObject.GetComponent<NavMeshAgent>();
        enemyAi = gameObject.GetComponent<EnemyAI>();
        enemyEffect = gameObject.GetComponent<EnemyEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null)
            {
                enemies.Remove(enemy);
                infiniteSpawnCurrentNum--;
            }
        }
        // run away ----------------------------------------------------------
        for (int i = 0; i < players.Length; i++)
        {
            if (!enemyEffect.isStun)
            {
                m_Distance = Vector3.Distance(players[i].transform.position, transform.position);
                target = players[i];
                enemyAi.look(target.transform);
                MoveDirection = transform.position - players[i].transform.position;
                RunAwayDirection = transform.position + MoveDirection;
                if (m_Distance <= RunAwayRange)
                {
                    transform.position = Vector3.Lerp(transform.position, RunAwayDirection, RunSpeed);
                    Debug.Log("Running away!");
                }
            }
            else
            {
                agent.Stop();
            }
        }
        // run away finish ---------------------------------------------------

        timer -= Time.deltaTime;
        range = Vector3.Distance(GameObject.FindWithTag("Player").transform.position, transform.position);
        if (infiniteSpawn == true)
        {
            if (infiniteSpawnCurrentNum < infiniteSpawnInMapNum)
            {
                if (timer <= 0 && range <= activedRange)
                {
                    GameObject enemy = EnemySpawner();

                    enemies.Add(enemy);
                    infiniteSpawnCurrentNum++;
                    timer = spawnTime;
                }
            }
        }
        else
        {
            if (limitedSpawnCurrentNum < limitedSpawnTotalNum)
            {
                if (timer <= 0 && range <= activedRange)
                {
                    Instantiate(enemyPrefab, GetRandomLocationForEnemy(), transform.rotation);
                    limitedSpawnCurrentNum++;
                    timer = spawnTime;
                }
            }
        }
    }


    public Vector3 GetRandomLocationForEnemy()
    {
        x = Random.Range(transform.position.x - SpawnRange, transform.position.x + SpawnRange);
        y = 1;
        z = Random.Range(transform.position.z - SpawnRange, transform.position.z + SpawnRange);

        for (int i = 0; i < players.Length; i++)
        {
            while (x <= players[i].transform.position.x + GodRadius && x >= players[i].transform.position.x - GodRadius && z <= players[i].transform.position.z + GodRadius && z >= players[i].transform.position.z - GodRadius)
            {
                x = Random.Range(transform.position.x - SpawnRange, transform.position.x + SpawnRange);
                z = Random.Range(transform.position.z - SpawnRange, transform.position.z + SpawnRange);
            }
        }

        RandomLocation = new Vector3(x, y, z);
        return RandomLocation;
    }

    GameObject EnemySpawner()
    {
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, GetRandomLocationForEnemy(), transform.rotation);

        return enemy;
    }
}