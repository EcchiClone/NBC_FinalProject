using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    #region # Core
    DataManager _dataManager = new DataManager();
    ResourceManager _resouceManager = new ResourceManager();
    InputManager _inputManager = new InputManager();
    SceneManagerEx _sceneManager = new SceneManagerEx();
    UIManager _uiManager = new UIManager();

    public static DataManager Data => Instance?._dataManager;
    public static ResourceManager RM => Instance?._resouceManager;
    public static InputManager Input => Instance?._inputManager;
    public static SceneManagerEx Scene => Instance?._sceneManager;
    public static UIManager UI => Instance?._uiManager;
    #endregion

    #region # Contents
    StageActionManager _stageActionManager = new StageActionManager();
    ModuleActionManager _moduleActionManager = new ModuleActionManager();
    ActionManager _actionManager = new ActionManager();

    GameManager _gameManager = new GameManager();
    ModuleManager _module = new ModuleManager();    
    SpawnManager _spawnManager = new SpawnManager();    
    TutorialManager _tutorialManager = new TutorialManager();
    AchievementSystem _achievementSystem = new AchievementSystem();

    public static StageActionManager StageActionManager => Instance?._stageActionManager;
    public static ModuleActionManager ModuleActionManager => Instance?._moduleActionManager;
    public static ActionManager ActionManager => Instance?._actionManager;

    public static GameManager GameManager => Instance?._gameManager;
    public static ModuleManager Module => Instance?._module;    
    public static SpawnManager SpawnManager => Instance?._spawnManager;    
    public static TutorialManager Tutorial => Instance?._tutorialManager;
    public static AchievementSystem AchievementSystem => Instance?._achievementSystem;
    #endregion

    private static void Init()
    {
        if(s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            s_instance._gameManager.Init();
            s_instance._dataManager.Init();
            s_instance._module.Init();
            s_instance._achievementSystem.Init();
        }
    }

    /// <summary>
    /// 씬이 넘어갈 때 각 매니저의 초기화를 실행
    /// </summary>
    public static void Clear()
    {
        ActionManager.Clear();
        ModuleActionManager.Clear();
        StageActionManager.Clear();        
        SpawnManager.Clear();        
        Input.Clear();
        Module.Clear();
        Tutorial.Clear();
        CoroutineManager.StopAllCoroutine();
    }
}
