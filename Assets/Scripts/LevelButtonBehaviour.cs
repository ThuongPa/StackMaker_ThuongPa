using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class LevelButtonBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text buttonText; 
    [SerializeField] private int level;
    private void Awake() {
        buttonText.text = name;    
    }
    public void ChangeLevel(){
        SceneManager.LoadScene(level);
    }
}
