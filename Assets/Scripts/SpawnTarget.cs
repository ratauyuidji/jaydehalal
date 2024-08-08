using System.Collections;
using UnityEngine;

public class SpawnTarget : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 4f;

    private bool canSpawn = true;

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0 && canSpawn)
        {
            if (enemyPrefab != null && spawnPoint != null)
            {
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                canSpawn = false;
                StartCoroutine(WaitForNextSpawn());
            }
        }
    }
    private IEnumerator WaitForNextSpawn()
    {
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
