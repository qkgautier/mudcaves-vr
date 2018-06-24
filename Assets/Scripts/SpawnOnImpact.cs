using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;


public class SpawnOnImpact : MonoBehaviour {

    public GameObject spawnObject;
    //public InstantiateSpider instantiateScript;
    private bool collisionEnabled = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(EnableCollision());
	}

    IEnumerator EnableCollision()
    {
        yield return new WaitForSeconds(3);

        collisionEnabled = true;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collisionEnabled && GetComponent<Rigidbody>().isKinematic == false && collision.relativeVelocity.magnitude > 2)
        {
            collisionEnabled = false;
            var headsetTransform = VRTK_DeviceFinder.HeadsetTransform();
            Instantiate(spawnObject, headsetTransform.position, headsetTransform.rotation);
            StartCoroutine(EnableCollision());
        }
    }
}
