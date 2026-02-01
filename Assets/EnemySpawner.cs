using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<EnemyWave> waves;
    [SerializeField] float startWait = 5;

    int currentWaveIndex = 0;
    IEnumerator currentWave;

    void Update()
    {
        if (startWait > 0)
            startWait = Mathf.Max(startWait - Time.deltaTime, 0);
        else
        {
            if (currentWave == null || (currentWave != null && currentWave.MoveNext() == false))
                currentWave = ProcessWave(waves[Mathf.Clamp(currentWaveIndex, 0, waves.Count - 1)]);
            print(currentWave.MoveNext());
        }
    }

    IEnumerator ProcessWave(EnemyWave wave)
    {
        // print("what");

        yield return new WaitForSeconds(wave.initialWait);

        int currentEnemy = 0;
        while (currentEnemy < wave.enemies.Count)
        {
            // spawn here
            print(wave.name + ", " + wave.enemies[currentEnemy].name);

            GameObject enemy = Instantiate(wave.enemies[currentEnemy], transform);
            enemy.transform.SetParent(null);
            enemy.transform.position += Vector3.right * 4;

            yield return new WaitForSeconds(wave.timeBetweenSpawn);
        }

        currentWaveIndex++;
        yield return false;
    }
}
