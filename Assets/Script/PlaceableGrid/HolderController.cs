using UnityEngine;
using UnityEngine.UI;

public class HolderController : MonoBehaviour
{
    public static HolderController instance;
    [SerializeField]
    private Transform holdersContainer;
    [SerializeField]
    private Transform holderPrefab;

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

    public void HolderInitialize(GameObject spawnObj, int numberOfObj, string holderName)
    {
        Transform holder = Instantiate(holderPrefab, holdersContainer);
        holder.name = holderName;
        Text objLeft = holder.GetChild(0).GetComponent<Text>();
        Image objImage = holder.GetChild(1).GetComponent<Image>();
        SetHolderIn4(holder, objLeft, objImage, spawnObj, numberOfObj);
    }

    private void SetHolderIn4(Transform holder, Text objLeft, Image objImage, GameObject spawnObj, int numberOfObj)
    {
        holder.GetComponent<ObjectHolder>().CurrentObj = spawnObj;
        objLeft.text = numberOfObj.ToString();
        objImage.sprite = spawnObj.GetComponent<SpriteRenderer>().sprite;
    }


}
