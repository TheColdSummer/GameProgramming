using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject player;
    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        List<Transform> spawnPoints = new List<Transform>();
        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }
        if (spawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            Transform spawnPoint = spawnPoints[randomIndex];
            player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y, player.transform.position.z);
        }
        else
        {
            Debug.LogError("No spawn points found for player.");
        }
    }
}
