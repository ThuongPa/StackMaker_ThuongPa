using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject ingameUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private TMP_Text inGameScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text endGameScoreText;
    void Start(){
        ingameUI.SetActive(true);
        endGameUI.SetActive(false);
    }
    public void EndLevelPopUp(){
        ingameUI.SetActive(false);
        endGameUI.SetActive(true);
    }

   public void UpdateScore(int score){
        inGameScoreText.text = "Bricks: " + score.ToString();
   }
    public void ShowScore(int Score){
        endGameScoreText.text = "Score: " + Score.ToString();
    }
    public void ShowHighScore(int score){
        highScoreText.text = "High Score: " + score.ToString();
    }
    public void BackToMainMenu(){
        SceneManager.LoadScene(0);
    }
}
