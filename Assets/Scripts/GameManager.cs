using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject pauseUI;
    public GameObject gameUI;
    public bool isGameOver = false;
    public bool isGamePaused = false;
 
    Ball_ ball;

    [SerializeField] private TMP_Text comboCountText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text scoreText;
    public int comboCount = 0;
    int score = 0;
    int baseBrickScore = 10;


    private void Awake()
    {
        var existingManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None);
        if (existingManagers.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        score = 0;
        comboCount = 0;
        highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }
    private void Start()
    {
        Time.timeScale = 1f;
        ball = FindAnyObjectByType<Ball_>();      
    }

    private void Update()
    {
        if (ball.ConsumePaddleHit())
        {
            comboCount = 0;
            comboCountText.text = comboCount.ToString();
        }

        Debug.Log(score);
    }

    public void GameOver()
    {
        if (isGameOver )
            return;
        isGameOver = true;
        gameOverUI.SetActive(true);
        pauseUI.SetActive(false);
        gameUI.SetActive(false);
        Time.timeScale = 0f;    
    }

    public void Restart()
    {
        Brick.OnBrickDestroyed -= HandleBrickDestroyed;
        gameUI.SetActive(true);
        pauseUI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            gameUI.SetActive(false);
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            gameUI.SetActive(true);
        }   
        
        
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable()
    {
        Brick.OnBrickDestroyed += HandleBrickDestroyed;
    }

    private void OnDisable()
    {
        Brick.OnBrickDestroyed -= HandleBrickDestroyed;
    }

    void HandleBrickDestroyed(Brick brick)
    {



        if (!ball.isTouchedThePaddle)
        {
            comboCount++;
            comboCountText.text = comboCount.ToString();
            int multiplier = (int)Mathf.Pow(2, comboCount - 1);
            int gainedScore = baseBrickScore * multiplier;

            score += gainedScore; 
            scoreText.text = score.ToString();
        }

        if(score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.Save();
            highScoreText.text = score.ToString();
        }
       
       
    }


    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScoreText.text = "0";
    }

  
}


