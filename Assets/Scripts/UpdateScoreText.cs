using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScoreText : MonoBehaviour {

    private Text scoreText;
    private Text healthText;

    // Use this for initialization
    void Start ()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        healthText = transform.Find("HealthText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        scoreText.text = ((int)PlayerScore.Score).ToString();
        healthText.text = ((int)PlayerScore.PlayerHealth).ToString();
    }
}
