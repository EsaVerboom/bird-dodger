using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HUDController : MonoBehaviour
{
    [Header("UI Document")]
    public UIDocument uiDocument;

    // UI elements
    private Label scoreLabel;       // Gameplay score & final score
    private Label gameOverLabel;
    private Label highScoreLabel;
    private Button restartButton;

    private int currentScore = 0;
    private bool isGameOver = false;

    void Awake()
    {
        if (uiDocument == null)
        {
            Debug.LogError("[HUDController] UIDocument not assigned!");
            return;
        }

        var root = uiDocument.rootVisualElement;

        // Find UI elements
        scoreLabel = root.Q<Label>("scoreLabel");
        gameOverLabel = root.Q<Label>("gameOverLabel");
        highScoreLabel = root.Q<Label>("highScoreLabel");
        restartButton = root.Q<Button>("restartButton");

        if (scoreLabel == null || gameOverLabel == null || highScoreLabel == null || restartButton == null)
        {
            Debug.LogError("[HUDController] One or more UI elements not found! Check names in UI Builder.");
        }

        // Button
        restartButton.clicked += RestartGame;

        // Hide Game Over UI initially
        gameOverLabel.style.display = DisplayStyle.None;
        highScoreLabel.style.display = DisplayStyle.None;
        restartButton.style.display = DisplayStyle.None;

        Debug.Log("[HUDController] Awake complete, UI initialized.");

        // Set gameplay score style
        UpdateScoreLabel();
    }

    /// <summary>
    /// Increment score
    /// </summary>
    public void AddScore()
    {
        if (isGameOver) return;

        currentScore++;
        Debug.Log("[HUDController] Score increased: " + currentScore);
        UpdateScoreLabel();
    }

    /// <summary>
    /// Game over UI
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("[HUDController] GameOver called! CurrentScore: " + currentScore);

        Time.timeScale = 0f;

        // ----- Game Over Label -----
        gameOverLabel.style.display = DisplayStyle.Flex;
        gameOverLabel.style.position = Position.Absolute;
        gameOverLabel.style.left = Length.Percent(42f);
        gameOverLabel.style.top = Length.Percent(25f);
        gameOverLabel.style.fontSize = 64;
        gameOverLabel.style.color = new StyleColor(Color.red);
        gameOverLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        gameOverLabel.style.translate = new Translate(-50f, -50f, 0f);

        // ----- High Score -----
        highScoreLabel.style.display = DisplayStyle.Flex;
        highScoreLabel.style.position = Position.Absolute;
        highScoreLabel.style.left = Length.Percent(45f);
        highScoreLabel.style.top = Length.Percent(40f);
        highScoreLabel.style.fontSize = 40;
        highScoreLabel.style.color = new StyleColor(Color.yellow);
        highScoreLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        highScoreLabel.style.translate = new Translate(-50f, -50f, 0f);

        // Get high score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.Save();
            highScore = currentScore;
            Debug.Log("[HUDController] New HighScore: " + highScore);
        }
        highScoreLabel.text = "High Score: " + highScore;

        // ----- Final Score (current score) -----
        scoreLabel.style.display = DisplayStyle.Flex;   
        scoreLabel.style.position = Position.Absolute;
        scoreLabel.style.left = Length.Percent(45f);
        scoreLabel.style.top = Length.Percent(50f);
        scoreLabel.style.fontSize = 36;
        
        scoreLabel.style.color = new StyleColor(Color.white);
        scoreLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
        scoreLabel.style.translate = new Translate(-50f, -50f, 0f);
        scoreLabel.text = "Your Score: " + currentScore;

        // ----- Restart Button -----
        restartButton.style.display = DisplayStyle.Flex;
        restartButton.style.position = Position.Absolute;
        restartButton.style.left = Length.Percent(45f);
        restartButton.style.top = Length.Percent(70f);
        restartButton.style.width = 220;
        restartButton.style.height = 70;
        restartButton.style.fontSize = 28;
        restartButton.style.justifyContent = Justify.Center;
        restartButton.style.alignItems = Align.Center;
        restartButton.style.translate = new Translate(-50f, -50f, 0f);
    }

    /// <summary>
    /// Update gameplay score (bottom-left)
    /// </summary>
    private void UpdateScoreLabel()
    {
        scoreLabel.text = "Score: " + currentScore;

        if (!isGameOver)
        {
            scoreLabel.style.position = Position.Absolute;
            scoreLabel.style.left = 20;
            scoreLabel.style.bottom = 20;
            scoreLabel.style.fontSize = 72; // bigger for gameplay
            scoreLabel.style.unityTextAlign = TextAnchor.LowerLeft;
            scoreLabel.style.color = new StyleColor(Color.white);
            scoreLabel.style.translate = new Translate(0f, 0f, 0f);
        }
    }

    /// <summary>
    /// Restart scene
    /// </summary>
    private void RestartGame()
    {
        Debug.Log("[HUDController] Restart button clicked, reloading scene...");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
