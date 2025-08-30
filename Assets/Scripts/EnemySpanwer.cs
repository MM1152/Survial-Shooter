using System.Collections.Generic;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    public Enemy[] enemys;
    public Transform[] spawnPoints;
    public List<KeyValuePair<float, Enemy>> spawnTable = new List<KeyValuePair<float , Enemy>>();

    public float spawnInterval = 3f;
    public float maxSpawnCount = 20;

    public float timer = 0f;

    private float weight;

    private void Awake()
    {
        timer = 0f;

        foreach(var enemy in enemys)
        {
            weight += enemy.data.spawnPosibility;
        }

        for(int i = 0; i < enemys.Length; i++)
        {
            spawnTable.Add(new KeyValuePair<float, Enemy>(enemys[i].data.spawnPosibility / weight, enemys[i]));
        }
        spawnTable.Sort((x , y) => y.Key.CompareTo(x.Key));
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >  spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();
        }
    }

    private Enemy SpawnEnemyInfo()
    {
        float rand = Random.Range(0f, 1f);
        float accumulate = 0f;

        foreach(var posibility in spawnTable)
        {
            accumulate += posibility.Key;
            if(rand <= accumulate)
            {
                return posibility.Value;
            }
        }

        return null;
    } 

    private void SpawnEnemy()
    {
        Enemy spawnEnemy = SpawnEnemyInfo();
        if (spawnEnemy == null) return;

        int randSpawnPoint = Random.Range(0, spawnPoints.Length);
        Instantiate(spawnEnemy, spawnPoints[randSpawnPoint].position, Quaternion.identity);
    }
}
