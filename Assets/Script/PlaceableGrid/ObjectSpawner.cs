using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner instance;
    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void PlaceObj(Vector3Int spawnPos, GameObject placeObject, Transform parent)
    {
        if (GridCellManager.instance.IsPlaceableArea(spawnPos))
        {
            placeObject.GetComponent<Collider2D>().enabled = true;
            placeObject.GetComponent<Rigidbody2D>().simulated = true;
            placeObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

            if (GameManager.instance.currentDragIndex != -1)
            {
                if (GameManager.instance.currentLevelObjs[GameManager.instance.currentDragIndex].quantity > 0)
                {
                    Vector3 objPos = GridCellManager.instance.PositonToSpawn(spawnPos);
                    GameObject obj = Instantiate(placeObject, parent);
                    obj.transform.position = objPos;
                    GameManager.instance.currentLevelObjs[GameManager.instance.currentDragIndex].quantity--;
                    GameManager.instance.gameScene.ChangeObjQuantity(GameManager.instance.currentDragIndex, GameManager.instance.currentLevelObjs[GameManager.instance.currentDragIndex].quantity);
                    GameManager.instance.currentDragIndex = -1;
                }
            }
        }
    }
}
