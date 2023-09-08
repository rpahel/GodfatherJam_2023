using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RoundManager : MonoBehaviour
{
    private bool isRunning = false;
    private bool isTransition = false;
    
    public float roundDuration;
    private float clockTimeRemaining;
    private float transitionTimeRemaining;
    public float transitionDuration;
    private bool gameEnd = false;


    public int[] sceneRefs;
    private int nextSceneIndex = 0;

    float minutes;
    float seconds;

    int clawScore = 0;
    int robotScore = 0;

    private GameObject go_player;


    public Text timeText;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        go_player = GameObject.Find("PlayerCharacter");
        ResetTimer();
        DontDestroyOnLoad(this.gameObject);
    }
    
    void ResetTimer()
    {
        clockTimeRemaining = roundDuration;
        isRunning = true;
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        if (timeToDisplay < 0)
            timeToDisplay = 0;
        minutes = Mathf.FloorToInt(clockTimeRemaining / 60);
        seconds = Mathf.FloorToInt(clockTimeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void LoadNextScene()
    {
        isTransition = false;
        // reset timer
        Time.timeScale = 1;
        ResetTimer();
        // load next scene
        nextSceneIndex += 1;
        SceneManager.LoadScene(sceneRefs[nextSceneIndex], LoadSceneMode.Single);
    }

    void DrawScores()
    {
        scoreText.text = string.Format("P1:{0:0000}   P2:{1:0000}", clawScore, robotScore);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameEnd == false)
        {
            if (go_player.GetComponent<CPlayerMovements>().isPlayerDead)
            {
                clawScore += (int)clockTimeRemaining;
                robotScore += (int)roundDuration - (int)clockTimeRemaining;
                if (robotScore < 0)
                    robotScore = 0;
                DrawScores();
                clockTimeRemaining = 0;
            }

            if (isTransition == true) // if transition from one scene to the next
            {
                if (transitionTimeRemaining > 0) // decrement the transition time
                {
                    transitionTimeRemaining -= Time.unscaledDeltaTime;
                } else { // once its over start loading the next scene
                    LoadNextScene();
                }
            }
            if (isRunning) { // if the timer itself is running
                if (clockTimeRemaining > 0) { // decrement if time is not 0
                    clockTimeRemaining -= Time.deltaTime;
                    DisplayTime(clockTimeRemaining);
                } else {
                    if (!go_player.GetComponent<CPlayerMovements>().isPlayerDead)
                        robotScore += (int)roundDuration;
                    DrawScores();
                    if (nextSceneIndex == sceneRefs.Length - 1) { // if we have reached the end of our scenes
                        gameEnd = true;
                        Debug.Log("Game end");
                        Time.timeScale = 0.0F;
                        clockTimeRemaining = 0;
                        DisplayTime(clockTimeRemaining);
                    } else { // else prepare for transitionning the level
                        Debug.Log("End Round");
                        isRunning = false;
                        // setup for transition
                        Time.timeScale = 0.0F;
                        DisplayTime(clockTimeRemaining);
                        clockTimeRemaining = 0;
                        isTransition = true;
                        transitionTimeRemaining = transitionDuration;
                    }
                }
            }
        }
    }
}
