using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] spawns;
    public GameObject enemy;
    public float spawnTime;
    int i = 0;

    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        ResetSpawnTimer();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            Instantiate(enemy, spawns[i].transform.position, Quaternion.identity);
            i++;

            if (i > 3)
            {
                i = 0;
            }
            ResetSpawnTimer();
        }
    }

    void ResetSpawnTimer()
    {
        spawnTimer = spawnTime;
    }
}
