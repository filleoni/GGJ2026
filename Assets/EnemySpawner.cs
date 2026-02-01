using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<EnemyWave> waves;
    [SerializeField] float startWait = 5;

    [SerializeField] List<Transform> spawnPos;

    int currentWaveIndex = 0;
    Coroutine currentWave;

    void Update()
    {
        if (startWait > 0)
            startWait = Mathf.Max(startWait - Time.deltaTime, 0);
        else
        {
            if (currentWave == null)
                currentWave = StartCoroutine(ProcessWave(waves[Mathf.Clamp(currentWaveIndex, 0, waves.Count - 1)]));
        }
    }

    IEnumerator ProcessWave(EnemyWave wave)
    {
        // print("what");

        int currentEnemy = 0;
        while (currentEnemy < wave.enemies.Count)
        {
            // spawn here
            print(wave.name + ", " + wave.enemies[currentEnemy].name);

            GameObject enemy = Instantiate(wave.enemies[currentEnemy], transform);
            enemy.transform.SetParent(null);

            Vector3 pos = spawnPos[Random.Range(0, spawnPos.Count)].transform.position;
            enemy.transform.position = pos;

            yield return new WaitForSeconds(wave.timeBetweenSpawn);
            currentEnemy++;
        }
        yield return new WaitForSeconds(wave.initialWait);

        currentWaveIndex++;
        currentWave = null;
        yield return false;
    }
}
