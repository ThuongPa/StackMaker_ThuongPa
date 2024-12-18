using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float speed;
    [SerializeField] private float yOffset;
    [SerializeField] private float zOffset;
    public bool onEndLine;
    private void Start() {
        onEndLine = false;
    }
    void Update(){
        if (!onEndLine){
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, yOffset, player.transform.position.z + zOffset), speed * Time.deltaTime);
        }else{
            GetComponent<Camera>().fieldOfView = 45;
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x + 15, 5.5f, player.transform.position.z + zOffset - 20), speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, -22, transform.rotation.z), speed * Time.deltaTime);
        }
    }
}
