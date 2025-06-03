using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemySpawnPoints;
    void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        foreach (Transform spawnPoint in enemySpawnPoints.transform)
        {
            GameObject enemy = Instantiate(Resources.Load<GameObject>("Enemy"));
            if (enemy != null)
            {
                enemy.transform.position = spawnPoint.position;
                enemy.transform.SetParent(spawnPoint.transform);
                Transform rangeTransform = spawnPoint.Find("Range");
                if (rangeTransform != null)
                {
                    enemy.GetComponent<Enemy>().SetIdleRange(rangeTransform.gameObject);
                }
                else
                {
                    Debug.LogWarning("Range GameObject not found in spawn point.");
                }
            }
            else
            {
                Debug.LogError("Enemy prefab not found in Resources folder.");
            }
        }
    }
}
