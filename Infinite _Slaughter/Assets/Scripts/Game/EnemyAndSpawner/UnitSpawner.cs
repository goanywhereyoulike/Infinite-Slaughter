using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitSpawner : MonoBehaviour
{
    public GameObject UnitPrefab;
    public int numberOfWaves;
    public int enemiesPerWave;
    public int secondsBetweenWaves;
    public int secondsStartDelay;
    public int pathId;
    public Transform destination;

    private int _currentWave = 0;

    private WaypointManager.Path _path;

    void Start()
    {
        StartSpawner();
    }
    public void Init(WaypointManager.Path path)
    {
        _path = path;
    }

    public void StartSpawner()
    {
        StartCoroutine("BeginWaveSpawn");
    }

    private IEnumerator BeginWaveSpawn()
    {
        yield return new WaitForSeconds(secondsStartDelay);

        while (_currentWave < numberOfWaves)
        {
            yield return SpawnWave(_currentWave++);
            yield return new WaitForSeconds(secondsBetweenWaves);
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        ObjectPoolManager poolManager = ServiceLocator.Get<ObjectPoolManager>();

        for (int i = 0; i < enemiesPerWave; ++i)
        {
            GameObject unitGO = poolManager.GetObjectFromPool("Enemies");
            unitGO.SetActive(true);
            unitGO.transform.position = this.gameObject.transform.position;
            unitGO.transform.rotation = transform.rotation;

            //var Transfrom = FindObjectOfType<SpawnTransform>();
            //if (transform != null)
            //{
            //    unitGO.transform.position = Transfrom.gameObject.transform.position;
            //}
            unitGO.GetComponent<DestructibleObject>().CurrentHealth = 100;
            unitGO.GetComponent<Enemy>().UpdateHealthBar(100);
            //Instantiate(UnitPrefab, transform.position, Quaternion.LookRotation(destination.position));
            //unitGO.GetComponent<Enemy>().gameObject.AddComponent<NavMeshAgent>();
            unitGO.GetComponent<Enemy>().target = destination;
            yield return new WaitForSeconds(1f);
        }
    }
}
