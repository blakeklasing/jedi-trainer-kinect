using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour {

    public GameObject scoreObject;
    private int score = 0;

	void Start () {
        scoreObject.GetComponent<Text>().text = "Score: " + score;
    }
	
    public void incrementScore(int value)
    {
        score += value;
        scoreObject.GetComponent<Text>().text = "Score: " + score;
    }
}
