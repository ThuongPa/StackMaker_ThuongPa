using UnityEngine;

public class BrickBuilder : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject brickTileMap;
    [SerializeField] private GameObject baseTileMap;
    private void Awake() {
        int childCount = baseTileMap.transform.childCount;    
        for (int i = 0; i < childCount; i++){
            Transform baseTile = baseTileMap.transform.GetChild(i);
            Instantiate(brickPrefab, baseTile.position, Quaternion.Euler(-90, 0, 0), brickTileMap.transform);
        }
    }
}
