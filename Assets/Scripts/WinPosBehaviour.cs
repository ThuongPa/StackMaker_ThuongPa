using UnityEngine;

public class WinPosBehaviour : MonoBehaviour
{
    [SerializeField] private ParticleSystem firework1;
    [SerializeField] private ParticleSystem firework2;
    [SerializeField] private GameObject closeChest;
    [SerializeField] private GameObject openChest;
    private bool endGame;
    private void Start() {
        endGame = false;
    }
    public void EndGame(){
        if (!endGame){
            endGame = true;
            firework1.Play();
            firework2.Play();
            closeChest.SetActive(false);
            openChest.SetActive(true);
        }
    }
}
