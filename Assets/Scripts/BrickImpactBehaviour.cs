using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BrickImpactBehaviour: MonoBehaviour
{
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask finishLayer;
    [SerializeField] private LayerMask lineLayer;
    [SerializeField] private float brickHeight;
    [SerializeField] private GameObject brickMesh;
    [SerializeField] private GameObject brickDrop;
    [SerializeField] private float speed;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private Animator animator;
    [SerializeField] private Camera camera;
    [SerializeField] private WinPosBehaviour winPos;
    private Player player;
    private int brickNum;
    private List<GameObject> bricks;
    private Color[] lineColor = {Color.yellow, Color.red, Color.magenta, Color.blue, Color.cyan, Color.green};
    private int lineIndex;
    private GameObject currentLine;
    // Start is called before the first frame update
    void Start()
    {
        bricks = new List<GameObject>();
        brickNum = 0;
        uiManager.UpdateScore(brickNum);
        player = GetComponent<Player>();
        lineIndex = 0;
        currentLine = null;
    }

    // Update is called once per frame
    void Update()
    {
        TakeBrick(); 
        DropBrick();
        ClearBrick();
        ChangeLineColor();
    }


    void TakeBrick()
    {
        RaycastHit brick;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out brick ,Mathf.Infinity, brickLayer)){
            if (brick.transform.gameObject.tag == "Brick"){
                brickNum = bricks.Count;
                brick.transform.gameObject.SetActive(false);
                transform.position += Vector3.up * brickHeight;
                var currentBrickMesh = Instantiate(brickMesh, new Vector3(transform.position.x, transform.position.y - (brickHeight * brickNum ) - brickHeight, transform.position.z), Quaternion.Euler(-90, 0, 0), transform);
                bricks.Add(currentBrickMesh);
                animator.SetBool("take_brick", true);
                uiManager.UpdateScore(bricks.Count);
            } else {
                animator.SetBool("take_brick", false);
            }
        }
   }
   void DropBrick(){
        RaycastHit brick;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out brick ,Mathf.Infinity, brickLayer)){
            if(brick.transform.gameObject.tag == "DropBrick"){
                if (bricks.Count > 0){
                    Destroy(bricks[bricks.Count - 1]);
                    bricks.RemoveAt(bricks.Count - 1);
                    transform.position += Vector3.down * brickHeight;
                    Instantiate(brickDrop, new Vector3(brick.transform.position.x, 0, brick.transform.position.z), Quaternion.Euler(-90, 0, 0), brick.transform);
                    uiManager.UpdateScore(bricks.Count);
                }
            }
        }
   }
   void ClearBrick(){
        RaycastHit finish;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out finish, Mathf.Infinity, brickLayer)){
            if(finish.transform.gameObject.tag == "Finish"){
                string currentLevel = SceneManager.GetActiveScene().name;
                uiManager.ShowScore(bricks.Count);
                if (bricks.Count > PlayerPrefs.GetInt(currentLevel)){
                    PlayerPrefs.SetInt(currentLevel, bricks.Count);
                }

                uiManager.ShowHighScore(PlayerPrefs.GetInt(currentLevel));
                foreach (var brick in bricks){
                    Destroy(brick);
                }
                transform.position = new Vector3(transform.position.x, brickHeight, transform.position.z);
                Instantiate(brickDrop, new Vector3(finish.transform.position.x, 0, finish.transform.position.z), Quaternion.Euler(-90, 0, 0), finish.transform);
                bricks.Clear();
                GetComponent<Player>().Finish();
                animator.SetTrigger("finish_level");
                winPos.EndGame();
                player.gameEnd = true;
                Invoke(nameof(EndGame), 3.0f);
            }
        }
   }
   void ChangeLineColor(){
        RaycastHit line;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out line, Mathf.Infinity, lineLayer)){
            camera.transform.GetComponent<CameraController>().onEndLine = true;
            GetComponent<Player>().speed = 15.0f;
            if (line.transform.gameObject != currentLine){
                currentLine = line.transform.gameObject;
                line.transform.gameObject.GetComponent<MeshRenderer>().material.color = lineColor[lineIndex];
                lineIndex++;
            }
        }
   }
   void EndGame(){
        uiManager.EndLevelPopUp();
   }
}
