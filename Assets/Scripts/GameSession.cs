using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour{
    [SerializeField] private int playerLives = 3;
    [SerializeField] private int score;

    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake(){
        var numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1){
            Destroy(gameObject);
        }
        else{
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start(){
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath(){
        if (playerLives > 1){
            TakeLife();
        }
        else{
            ResetGameSession();
        }
    }

    public void AddToScore(int pointsToAdd){
        score += pointsToAdd;
        scoreText.text = score.ToString();
    }

    private void TakeLife(){
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    private void ResetGameSession(){
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
}