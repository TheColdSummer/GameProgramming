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
            spawnPoints.RemoveAt(randomIndex);
            for (int i = 0; i < 3; i++)
            {
                int extractionIndex = Random.Range(0, spawnPoints.Count);
                GameObject ep = Resources.Load<GameObject>("ExtractionPoint");
                Vector3 p = new Vector3(spawnPoints[i].position.x, spawnPoints[i].position.y, ep.transform.position.z);
                GameObject extractionPoint = Instantiate(Resources.Load<GameObject>("ExtractionPoint"), p, Quaternion.identity);
                extractionPoint.transform.SetParent(GameObject.Find("Map/ExtractionPoint").transform);
                spawnPoints.RemoveAt(extractionIndex);
            }
        }
        else
        {
            Debug.LogError("No spawn points found for player.");
        }
    }
}
