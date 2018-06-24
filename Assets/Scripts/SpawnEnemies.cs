using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnEnemies : MonoBehaviour {

    public Transform spawnPoint;
    public GameObject ennemy;
    public float spawnMinInterval = 1;
    public float spawnMaxInterval = 2;
    public EZObjectPools.EZObjectPool popupTextPool;
    public bool isSpawning = false;

    private float spawnInterval = 0;
    private float timer = 0.0f;

	// Use this for initialization
	void Start ()
    {
        timer = spawnMaxInterval;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isSpawning)
        {
            timer += Time.deltaTime;

            if (timer >= spawnInterval)
            {
                timer = 0.0f;
                spawnInterval = Random.Range(spawnMinInterval, Mathf.Max(spawnMinInterval,spawnMaxInterval-PlayerScore.Score/2000));
                var obj = Instantiate(ennemy, spawnPoint.position, spawnPoint.rotation);

                float speed = Random.Range(1.25f, 3.0f);
                float scale = Random.Range(0.5f, 1.3f);

                obj.GetComponent<EnemyHealth>().floatingTextPool = popupTextPool;
                obj.GetComponent<NavMeshAgent>().speed = speed * 0.8f;
                obj.GetComponent<Animation>()["walk"].speed = speed;

                obj.transform.localScale *= scale;
            }
        }
	}
}
