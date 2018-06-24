using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public event EventHandler RaisePlayerHealthReachZero;

    //// Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "spider")
        {
            // Destroy enemy
            other.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            other.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(other.gameObject, 0.0f);

            // Decrease player's health
            PlayerScore.PlayerHealth -= other.GetComponent<EnemyHealth>().attackValue;

            if (PlayerScore.PlayerHealth <= 0)
            {
                if (RaisePlayerHealthReachZero != null)
                {
                    RaisePlayerHealthReachZero(this, EventArgs.Empty);
                }

            }
        }
    }
}
