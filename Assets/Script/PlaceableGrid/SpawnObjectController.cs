using UnityEngine;

public class SpawnObjectController : MonoBehaviour
{
    public static SpawnObjectController instance;
    [SerializeField]
    private GameObject objectContainer;
    [SerializeField]
    public GameObject draggingObj;

    #region Obj initialized pos
    private Vector3Int mouseCellPos;
    private Vector3 mouseDownPos;
    private Vector3 mousePos;
    #endregion

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

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseCellPos = GridCellManager.instance.GetMouseCell(mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (draggingObj != null)
            {
                GameObject placeObj = draggingObj;
                ObjectSpawner.instance.PlaceObj(mouseCellPos, placeObj, objectContainer.transform);
                Destroy(draggingObj);
                draggingObj = null;
            }
        }
    }
}
