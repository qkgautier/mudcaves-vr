using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public static float Score = 0.0f;
    public static float PlayerHealth = 0.0f;
    public static float PlayerStartHealth = 100.0f;

    // Use this for initialization
    void Start ()
    {
    }
	
	//// Update is called once per frame
	//void Update ()
 //   {
 //   }

    public static void Reset()
    {
        Score = 0.0f;
        PlayerHealth = PlayerStartHealth;
    }
}
