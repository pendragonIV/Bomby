using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    private Vector3Int mouseCellPosition;

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

    public bool IsPlaceableArea(Vector3Int mouseCellPos)
    {
        if (tileMap.GetTile(mouseCellPos) == null)
        {
            return false;
        }
        return true;
    }

    public Vector3Int GetMouseCell(Vector3 mousePos)
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseCellPosition = tileMap.WorldToCell(mousePos);
        return mouseCellPosition;
    }

    public Vector3 PositonToSpawn(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

}
