using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 mousePositionStart;
    private Vector3 mousePositionEnd;
    private Vector3 currentBrickPosition;
    private Vector3 startPosition;
    private bool isMoving;
    private float tileSize = 1f;
    private enum Direction {Left, Right, Up, Down};
    private Direction dir;
    public bool gameEnd;
    public float speed;
    [SerializeField] private LayerMask brickLayer;
    [SerializeField] private LayerMask finishLayer;
    [SerializeField] private LayerMask pushLayer;
    [SerializeField] private LayerMask teleporterLayer;
    void OnInit(){
        isMoving = false; 
        startPosition = transform.position;
        gameEnd = false;
    }

    void Start(){
        OnInit();
    }

    void Update(){
        if (isMoving){
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentBrickPosition.x, transform.position.y, currentBrickPosition.z), Time.deltaTime * speed);
            if(Vector3.Distance(transform.position, new Vector3(currentBrickPosition.x, transform.position.y, currentBrickPosition.z)) < 0.1f){
                if (CheckDropBrick() || CheckPush()){
                    ChangeDirection();
                }else if (CheckTeleporter()){
                    Teleport();
                }else if (CheckFinish()){
                    Finish();
                }else{
                    isMoving = false;
                }
            }
        }
        if (gameEnd) return;
        if (Input.GetMouseButtonDown(0)){
            mousePositionStart = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)){
            if (!isMoving){
                isMoving = true;
                mousePositionEnd = Input.mousePosition;
                float deltaX = mousePositionEnd.x - mousePositionStart.x;
                float deltaY = mousePositionEnd.y - mousePositionStart.y;

                if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY) ){
                    // Move horizontal
                    if (deltaX > 0){
                        currentBrickPosition = GetBrickAtDirection(Direction.Right);
                    }else{
                        currentBrickPosition = GetBrickAtDirection(Direction.Left);
                    }
                }else{
                    if (deltaY > 0){
                        currentBrickPosition = GetBrickAtDirection(Direction.Up);
                    }else{
                        currentBrickPosition = GetBrickAtDirection(Direction.Down);
                    }
                mousePositionStart = Vector3.zero;
                mousePositionEnd = Vector3.zero;
                }
            }
        }
    }

    Vector3 GetBrickAtDirection(Direction dir)
    {
        this.dir = dir;
        RaycastHit brick;
        Vector3 brickPosition = Vector3.zero;
        Vector3 direction = Vector3.zero;
        switch(dir){
            case Direction.Left:
                direction = Vector3.left; 
                break;
            case Direction.Right:
                direction = Vector3.right;
                break;
            case Direction.Up:
                direction = Vector3.forward; 
                break;
            case Direction.Down:
                direction = Vector3.back; 
                break;
        }
        int tileNum = 1;
        while(Physics.Raycast(transform.position + direction * tileNum + Vector3.up, Vector3.down, out brick, Mathf.Infinity, brickLayer)){
            brickPosition = brick.transform.position;
            tileNum++;
        }
        if (tileNum > 1) {
            return brickPosition;
        } else {
            return transform.position;
        }
   }
   bool CheckDropBrick(){
        RaycastHit brick;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out brick ,Mathf.Infinity, brickLayer)){
            if (brick.transform.gameObject.tag == "DropBrickMesh"){
                return true;
            }
        }
        return false;
   }
   void ChangeDirection(){
        if (this.dir == Direction.Right || this.dir == Direction.Left){
            if(CheckBrickAtDirection(Direction.Up)){
                this.dir = Direction.Up;
                currentBrickPosition = GetBrickAtDirection(Direction.Up);
            }else if (CheckBrickAtDirection(Direction.Down)){
                this.dir = Direction.Down;
                currentBrickPosition = GetBrickAtDirection(Direction.Down);
            }else{
                isMoving = false;
            }
        }else if (this.dir == Direction.Up || this.dir == Direction.Down){
            if(CheckBrickAtDirection(Direction.Left)){
                this.dir = Direction.Left;
                currentBrickPosition = GetBrickAtDirection(Direction.Left);
            }else if (CheckBrickAtDirection(Direction.Right)){
                this.dir = Direction.Right;
                currentBrickPosition = GetBrickAtDirection(Direction.Right);
            }else{
                isMoving = false;
            }
        }
   }

   bool CheckBrickAtDirection(Direction dir){
        Vector3 brickPosition = Vector3.zero;
        Vector3 direction = Vector3.zero;
        switch(dir){
            case Direction.Left:
                direction = Vector3.left; 
                break;
            case Direction.Right:
                direction = Vector3.right;
                break;
            case Direction.Up:
                direction = Vector3.forward; 
                break;
            case Direction.Down:
                direction = Vector3.back; 
                break;
        }
        if (Physics.Raycast(transform.position + direction + Vector3.up, Vector3.down, Mathf.Infinity, brickLayer)){
            return true;
        }
        return false;

   }

   bool CheckPush(){
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, Mathf.Infinity, pushLayer)){
            return true;
        }
        return false;
   }

   bool CheckTeleporter(){
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, Mathf.Infinity, teleporterLayer)){
            return true;
        }
        return false;
   }
   void Teleport(){
        RaycastHit teleporter;
        Vector3 otherTeleporterPosition = Vector3.zero;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out teleporter, Mathf.Infinity, teleporterLayer)){
            otherTeleporterPosition = teleporter.transform.GetComponent<Teleporter>().otherTeleporter.position;
        }
        transform.position = new Vector3(otherTeleporterPosition.x, transform.position.y, otherTeleporterPosition.z); 
        currentBrickPosition = GetBrickAtDirection(this.dir);
   }
   bool CheckFinish(){
        RaycastHit finish;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out finish, Mathf.Infinity, brickLayer)){
            if(finish.transform.gameObject.tag == "Finish"){
                return true;
            }
            return false;
        }
        return false;
   }
   public void Finish(){
        RaycastHit finish;
        Vector3 direction = Vector3.zero;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out finish, Mathf.Infinity, brickLayer)){
            if(finish.transform.gameObject.tag == "Finish"){
                switch(dir){
                    case Direction.Left:
                        direction = Vector3.left; 
                        break;
                    case Direction.Right:
                        direction = Vector3.right;
                        break;
                    case Direction.Up:
                        direction = Vector3.forward; 
                        break;
                    case Direction.Down:
                        direction = Vector3.back; 
                        break;
                }
                currentBrickPosition = transform.position + direction * 7;
                WinPosBehaviour winPosBehaviour = new WinPosBehaviour();
                winPosBehaviour.EndGame();
            }
        }
   }
} 
