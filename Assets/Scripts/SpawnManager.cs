using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _powerUpPrefab;
    [SerializeField]
    private GameObject[] _powerUpsPrefab;
    [SerializeField]
    private GameObject _powerUpContainer;

    private bool _stopSpawning = false;


    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9.2f, 9.2f), 8f, 0), Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (!_stopSpawning)
        {
            int RandomPowerUpId = Random.Range(0, 3);

            GameObject powerUp = Instantiate(_powerUpsPrefab[RandomPowerUpId], new Vector3(Random.Range(-9.2f, 9.2f), 8f, 0), Quaternion.identity);

            powerUp.transform.parent = _powerUpContainer.transform;

            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
        }
    }

    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
