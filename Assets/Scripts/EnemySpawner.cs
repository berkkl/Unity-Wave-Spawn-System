using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int waveNumber;
        public List<Pool> pools;

        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }
    }

    #region Singleton & Awake

    public static EnemySpawner Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Wave> waves;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;

    //public List<EntityManager> entityType;
    public List<Transform> spawnPoints;
    public float timeToNextSpawn;
    private float _timerForNextSpawn;
    public int totalEntityCount;
    public int currentWave = 0;
    public int waveCount;

    public float entityHealth;
    public float entityMovementSpeed;
    public float entityAttackSpeed;
    public float entityPower;

    public float timeBetweenWaves = 10f;
    public float timeToNextWave;
    public bool isSpawningWave;

    private void Start()
    {
        InitializeObjectPooling();
        InitializeSpawnManager();
    }

    private void InitializeObjectPooling()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Wave wave in waves)
        {
            foreach (Wave.Pool pool in wave.pools)
            {
                if (!PoolDictionary.ContainsKey(pool.tag))
                {
                    Queue<GameObject> objectPool = new Queue<GameObject>();

                    for (int j = 0; j < pool.size; j++)
                    {
                        GameObject obj = Instantiate(pool.prefab);
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                    }
                    PoolDictionary.Add(pool.tag, objectPool);
                }
                else
                {
                    Queue<GameObject> objectPool = PoolDictionary[pool.tag];

                    for (int j = 0; j < pool.size; j++)
                    {
                        GameObject obj = Instantiate(pool.prefab);
                        obj.SetActive(false);
                        objectPool.Enqueue(obj);
                    }
                }
            }
        }
    }

    private void InitializeSpawnManager()
    {
        waveCount = waves.Count;
        currentWave = 0;

        /*for (int i = 0; i < entityType.Count; i++)
        {
            entityHealth = entityType[i].GetComponent<EntityManager>().maxHealth;
            entityMovementSpeed = entityType[i].GetComponent<EntityManager>().movementSpeed;
            entityAttackSpeed = entityType[i].GetComponent<EntityManager>().attackSpeed;
            entityPower = entityType[i].GetComponent<EntityManager>().attackDamage;
        }*/

        timeToNextWave = 0f;
        isSpawningWave = false;
    }

    private void Update()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.Log("Please add Spawn Points");
        }

        _timerForNextSpawn -= Time.deltaTime;

        if (!isSpawningWave && timeToNextWave <= 0)
        {
            StartCoroutine(SpawnWave());
        }

        if (timeToNextWave > 0)
        {
            timeToNextWave -= Time.deltaTime;
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!PoolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " does not exist");
            return null;
        }

        GameObject objectToSpawn = null;

        foreach (GameObject obj in PoolDictionary[tag])
        {
            if (!obj.activeInHierarchy)
            {
                objectToSpawn = obj;
                break;
            }
        }

        if (objectToSpawn == null)
        {
            Debug.LogWarning("No inactive object found with tag " + tag);
            return null;
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    private IEnumerator SpawnWave()
    {
        isSpawningWave = true;

        while (currentWave < waveCount && waves[currentWave].pools.Count > 0)
        {
            int randomNum = Random.Range(0, spawnPoints.Count);
            int randomEntityIndex = Random.Range(0, waves[currentWave].pools.Count);
            string poolTag = waves[currentWave].pools[randomEntityIndex].tag;

            SpawnFromPool(poolTag, spawnPoints[randomNum].position, Quaternion.identity);
            waves[currentWave].pools[randomEntityIndex].size -= 1;

            if (waves[currentWave].pools[randomEntityIndex].size <= 0)
            {
                waves[currentWave].pools.RemoveAt(randomEntityIndex);
            }

            yield return new WaitForSeconds(timeToNextSpawn);
        }

        if (currentWave < waveCount)
        {
            currentWave++;
        }
    
        Debug.Log("Wave spawn completed");
        timeToNextWave = timeBetweenWaves;
        isSpawningWave = false;
    }
}


