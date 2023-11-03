using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectHolder : MonoBehaviour, IBeginDragHandler, IDragHandler
{

    [SerializeField]
    private GameObject currentObj;
    [SerializeField]
    private Color draggingColor;
    public GameObject CurrentObj
    {
        get
        {
            return currentObj;
        }
        set
        {
            currentObj = value;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentObj != null)
        {
            SpawnObjectController.instance.draggingObj = Instantiate(currentObj, transform.position, Quaternion.identity);
            SpawnObjectController.instance.draggingObj.GetComponent<Collider2D>().enabled = false;
            SpawnObjectController.instance.draggingObj.GetComponent<Rigidbody2D>().simulated = false;
            SpawnObjectController.instance.draggingObj.GetComponent<SpriteRenderer>().color = draggingColor;
        }

        GameManager.instance.currentDragIndex = int.Parse(this.gameObject.name);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SpawnObjectController.instance.draggingObj != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SpawnObjectController.instance.draggingObj.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }


}
