using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public void RestartLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextLevel(){
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;    
        if (nextLevel > SceneManager.sceneCountInBuildSettings- 1){
            nextLevel = 1;
        }
        SceneManager.LoadScene(nextLevel);
    }
}
