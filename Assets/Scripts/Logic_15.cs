using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logic_15 : MonoBehaviour
{
    public GameObject timerImage;
    public float time;
    private float timer;

    public bool gameOver;
    public bool gameCompleted;
    public bool isDrag;

    public int fixCounter = 0;
    public int needToFix = 0;

    private SpawnKit_15 spawnKit;

    [SerializeField] private Transform[] spawnPositions;
    [SerializeField] private Transform[] fixedPositions;

    public GameObject[] candies = new GameObject[8];

    public int CANDIE_COUNT = 8;
    public int[] spawnIndex = new int[8];

    [SerializeField] private Animation topAnimation;
    [SerializeField] private GameObject uiTitle;
    [SerializeField] private GameObject gameOverText;

    //private _Data _data;

    void Start()
    {
        //_data.LoadData();

        gameOver = false;

        spawnKit = GetComponent<SpawnKit_15>();
        
        SpawnCandies();

        timer = time;
    }


    void Update()
    {
        if (!gameOver && !gameCompleted)
        {
            UpdateTimer();
            if(fixCounter == needToFix)
            {
                gameCompleted = true;
                Win();
            }            
        }
        if (gameOver)
        {
            GameOver();
        }
    }

    private void UpdateTimer()
    {
        timer -= Time.deltaTime;
        timer = Mathf.Clamp(timer, 0, time);

        timerImage.transform.localScale = new Vector3(timer / time, 1f, 1f);

        if(timer == 0)
        {
            gameOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        FindObjectOfType<DragDrop>().canTouch = false;
        gameOverText.SetActive(true);
        //_data.SaveData();
    }

    private void Win()
    {
        Destroy(uiTitle);

        Instantiate(spawnKit.confetti, Vector3.up * 2f, Quaternion.identity);

        topAnimation.Play();

        Achievement.miniGame15++;

        //_data.SaveData();
    }

    // Find unic positions for each candie
    private Vector3 FindCandiePosition(int i)
    {
        int temp = Random.Range(0, spawnPositions.Length);
        if (i > 0)
            for (int j = 0; j < i; j++)
            {
                if (temp == spawnIndex[j])
                {
                    return FindCandiePosition(i);
                }
            }
        spawnIndex[i] = temp;
        return spawnPositions[temp].position;
    }

    // Spawn candies according to difficulty
    public void SpawnCandies()
    {
        if (Achievement.miniGame15 < CANDIE_COUNT - 3)
        {
            for (int i = 0; i < 3 + Achievement.miniGame15; i++)
            {
                candies[i] = Instantiate(spawnKit.candie[i], FindCandiePosition(i), spawnKit.candie[i].transform.rotation);
                candies[i].GetComponent<Candy>().fixedPosition = fixedPositions[i];
                candies[i].GetComponent<Candy>().spawnPosition = spawnPositions[spawnIndex[i]];

                needToFix++; // Count amount of candies to fix
                time = (float)needToFix * 1.5f + 2f; // Time = amount of candies * 1.5f + 2f (1.5 sec for candy + 2 sec extra)
            }
        }
        else
        {
            for (int i = 0; i < CANDIE_COUNT; i++)
            {
                candies[i] = Instantiate(spawnKit.candie[i], FindCandiePosition(i), spawnKit.candie[i].transform.rotation);
                candies[i].GetComponent<Candy>().fixedPosition = fixedPositions[i];
                candies[i].GetComponent<Candy>().spawnPosition = spawnPositions[spawnIndex[i]];
            }
            needToFix = CANDIE_COUNT; // Set max amount of candies

            time = (float)CANDIE_COUNT / Achievement.miniGame15 * 1.5f * (float)CANDIE_COUNT; // Time = 8 candies / difficulty * 1.5f (8 diff = 1.5 sec for candy; 15 diff = 0.8sec for candy, etc.)
        }
    }
}
