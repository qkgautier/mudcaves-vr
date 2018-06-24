using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSpider : MonoBehaviour {

    public GameObject spiderObject;

    private void OnCollisionEnter(Collision collision)
    {
        var obj = collision.collider.gameObject;

        if(obj.tag == "spider" && collision.relativeVelocity.magnitude > 3)
        {
            Destroy(obj);

            if (GameObject.FindGameObjectsWithTag("spider").Length <= 1)
            {
                Transform spiderSpawnPoint = GameObject.Find("SpiderSpawnPoint").transform;
                Instantiate(spiderObject, spiderSpawnPoint.position, spiderSpawnPoint.rotation);
            }
        }
    }
}
