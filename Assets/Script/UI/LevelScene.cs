using UnityEngine;

public class LevelScene : MonoBehaviour
{
    public static LevelScene instance;

    private const int MAX_ACHIVE = 3;
    [SerializeField]
    private Transform levelHolderPrefab;
    [SerializeField]
    private Transform levelsContainer;
    [SerializeField]
    private Transform starPrefab;

    public Transform sceneTransition;

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

    void Start()
    {
        PrepareLevels();
    }
    public void PlayChangeScene()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
    }

    private void PrepareLevels()
    {
        for (int i = 0; i < LevelManager.instance.levelData.GetLevels().Count; i++)
        {
            Transform holder = Instantiate(levelHolderPrefab, levelsContainer);
            holder.name = i.ToString();
            Level level = LevelManager.instance.levelData.GetLevelAt(i);
            if (LevelManager.instance.levelData.GetLevelAt(i).isPlayable)
            {
                holder.GetComponent<LevelHolder>().EnableHolder();
            }
            else
            {
                holder.GetComponent<LevelHolder>().DisableHolder();
            }

            SetAchivement(holder, level);
        }
    }

    private void SetAchivement(Transform holder, Level levelData)
    {
        Transform achivementContainer = holder.GetChild(0);

        for (int i = 0; i < 3; i++)
        {
            if (i < levelData.achivement)
            {
                Transform starDisabler = achivementContainer.GetChild(i).GetChild(0);
                starDisabler.gameObject.SetActive(false);
            }
        }
    }



}
