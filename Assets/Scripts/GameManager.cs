using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public SpawnEnemies enemiesManager;
    public GameObject triggerArea;
    public GameObject gun;
    public Transform gunSpawnPoint;
    public float timeToReleaseGun = 3.0f;

    private bool isGameRunning = false;
    private bool isGameStopping = false;
    private float releaseGunTimer;

    // Use this for initialization
    void Start()
    {
        triggerArea.GetComponent<EnemyAttack>().RaisePlayerHealthReachZero += HandlePlayerHealthReachZero;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStopping)
        {
            releaseGunTimer -= Time.deltaTime;
            if(releaseGunTimer <= 0.0f)
            {
                // Release gun and move it back to its original position
                gun.GetComponent<GunBehavior>().ForceRelease();
                gun.transform.position = gunSpawnPoint.position;
                gun.transform.localRotation = gunSpawnPoint.localRotation;
                isGameStopping = false;
            }
        }
    }

    public void StartGame()
    {
        if(!isGameStopping)
        {
            enemiesManager.isSpawning = true;
            triggerArea.GetComponent<EnemyAttack>().enabled = true;
            triggerArea.GetComponent<Light>().enabled = true;
            if (PlayerScore.PlayerHealth <= 0) { PlayerScore.Reset(); }
            isGameRunning = true;
        }
    }

    public void StopGame()
    {
        if (isGameRunning && !isGameStopping)
        {
            isGameRunning = false;
            isGameStopping = true;

            // Stop spawning enemies
            if (enemiesManager != null) { enemiesManager.isSpawning = false; }

            // Disable trigger area
            if (triggerArea != null)
            {
                triggerArea.GetComponent<EnemyAttack>().enabled = false;
                triggerArea.GetComponent<Light>().enabled = false;
            }

            // Remove remaining spiders
            foreach (var obj in GameObject.FindGameObjectsWithTag("spider"))
            {
                Destroy(obj);
            }

            releaseGunTimer = timeToReleaseGun;
        }

    }

    private void HandlePlayerHealthReachZero(object sender, EventArgs args)
    {
        StopGame();
    }
}
