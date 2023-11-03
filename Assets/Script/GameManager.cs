using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;
    #region Game status
    [SerializeField]
    private bool isGameWin = false;
    [SerializeField]
    private bool isLose = false;

    [SerializeField]
    public int achivement = 0;
    private const int MAX_ACHIVE = 3;

    [SerializeField]
    public List<LevelObject> currentLevelObjs;
    [SerializeField]
    public int currentDragIndex;
    #endregion

    private void Start()
    {
        currentLevelObjs = new List<LevelObject>();
        Level currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);

        currentLevelData.objects.ForEach(obj =>
        {
            currentLevelObjs.Add(new LevelObject(obj.spawnObject, obj.quantity));
        });

        IngameHolderInit(currentLevelData);

        Time.timeScale = 1;
    }

    private void IngameHolderInit(Level currentLevelData)
    {
        for (int i = 0; i < currentLevelData.objects.Count; i++)
        {
            HolderController.instance.HolderInitialize(currentLevelData.objects[i].spawnObject, currentLevelData.objects[i].quantity, i.ToString());
        }
    }

    public void Win()
    {
        if (LevelManager.instance.levelData.GetLevels().Count > LevelManager.instance.currentLevelIndex + 1)
        {
            if (LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex + 1).isPlayable == false)
            {
                LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex + 1, true, false, 0);
            }
        }
        SetAchivement();
        if (achivement > LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).achivement)
        {
            LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true, achivement);
        }
        else
        {
            LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, true, true, LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).achivement);
        }
        isGameWin = true;

        gameScene.ShowWinPanel();
        Time.timeScale = 0;
        LevelManager.instance.levelData.SaveDataJSON();
    }

    private void SetAchivement()
    {
        int totalObj = LevelManager.instance.levelData.GetLevelTotalObj(LevelManager.instance.currentLevelIndex);
        int objLeft = 0;
        currentLevelObjs.ForEach(obj =>
        {
            objLeft += obj.quantity;
        });

        achivement = (int)((float)(totalObj - objLeft) / (float)totalObj * 3);
    }

    public void Lose()
    {
        isLose = true;
        gameScene.ShowLosePanel();
        Time.timeScale = 0;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool isGameLose()
    {
        return isLose;
    }
}

