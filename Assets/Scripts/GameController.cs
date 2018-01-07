using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController _instance;

    private float timeLeft;
    public Text timeText;

    private int score;
    public Text scoreText;
    public Text highScoreText;

    [HideInInspector]
    public List<TargetBehaviour> targets = new List<TargetBehaviour>();

    void Awake()
    {
        _instance = this;

        timeLeft = 50;
        timeText.text = timeLeft.ToString();
    }

	void Start () 
    {
        iTween.ValueTo(gameObject, iTween.Hash(
                        "from", timeLeft,
                        "to", 0,
                        "time", timeLeft,
                        "onupdatetarget", gameObject,
                        "onupdate", "tweenUpdate",
                        "oncomplete", "GameComplete"
                        ));

        StartCoroutine("SpawnTargets");

        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore").ToString();
        score = 0;
	}

    public void IncreaseScore()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();

        if(score > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", score);
            highScoreText.text = "High Score: " + score.ToString();
        }
    }

    void GameComplete()
    {
        StopCoroutine("SpawnTargets");
        timeText.color = Color.black;
        timeText.text = "GAME OVER";
    }

    void tweenUpdate(float newValue)
    {
        timeLeft = newValue;
        if(timeLeft > 10)
        {
            timeText.text = timeLeft.ToString("#");
        } 
        else
        {
            timeText.color = Color.red;
            timeText.text = timeLeft.ToString("#.0");
        }
    }

    void SpawnTarget()
    {
        int index = Random.Range(0, targets.Count);

        TargetBehaviour target = targets[index];

        target.ShowTarget();
    }

    IEnumerator SpawnTargets()
    {
        yield return new WaitForSeconds(1.0f);

        while(true)
        {
            int numOfTargets = Random.Range(1, 4);

            for (int i = 0; i < numOfTargets; i++)
            {
                SpawnTarget();
            }

            yield return new WaitForSeconds(Random.Range(0.5f * numOfTargets, 2.5f));
        }
    }
}
