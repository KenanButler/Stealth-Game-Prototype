using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region singleton
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<GameManager>();
            }

            if (!instance)
            {
                throw new System.Exception("NO GAME MANAGER FOUND");
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (!instance)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
    }
    #endregion

    [SerializeField] GameObject PlayerBody;
    public GameObject playerBody => PlayerBody;

    private TankBody tankBody;
    public TankBody TankBody => tankBody;

    private TankPlayerController controller;
    public TankPlayerController Controller => controller;

    [SerializeField] Transform PlayerPoint;
    public Transform playerPoint => PlayerPoint;




    #region SceneManagment

   

    [SerializeField] private string[] levelNames;

    private bool isLoading = false;
    public bool IsLoading => isLoading;
    private string currLevelName;
    //private int currLevel = 0;

    public SaveData saveData = new SaveData();
    
    public SaveData musicData = new SaveData();
    
    




    IEnumerator LoadLevel(string levelName)
    {
        
        isLoading = true;
        playerBody.SetActive(false);

        loadScreen.gameObject.SetActive(true);
        yield return StartCoroutine(loadScreen.FakeLoad(1f));

        if(!string.IsNullOrEmpty(currLevelName))
        {
            yield return AudioManager.Instance.StartCoroutine(AudioManager.Instance.FadeOut());
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currLevelName);

            while(!asyncUnload.isDone)
            {
                yield return null;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));

        currLevelName = levelName;

        MovePlayerToSpawn();
        
        playerBody.SetActive(true);
        AudioManager.Instance.LoadLevelComplete();
        loadScreen.gameObject.SetActive(false);
        if(levelName == "MainMenu")
        {
            playerBody.SetActive(false);
            controller.inMenu = true;
        }
        else
        {
            controller.inMenu= false;
        }
        isLoading = false;
        
    }

    public void LevelComplete()
    {
        pauseScreen.canPause = false;
        saveData.currLevel++;
        
        if (saveData.currLevel < levelNames.Length)
        {
            FileHandler.Save(saveData, FileHandler.SaveType.Json);
            StartCoroutine(LoadLevel(levelNames[saveData.currLevel]));
        }
        else
        {
            FileHandler.type = FileHandler.SaveType.PlayerPrefs;
            FileHandler.ClearSaveData();
            tankBody.gameObject.SetActive(false);
            vicScreen.SetActive(true);
        }
    }

    #endregion

    #region MenuManagement

    [SerializeField] string mainMenu = "MainMenu";
    [SerializeField] LoadScreen loadScreen;
    [SerializeField] GameOverScreen gameOver;
    [SerializeField] GameObject vicScreen;
    [SerializeField] PauseScreen pauseScreen;
    [SerializeField] OptionsScreen optionsScreen;
    bool optionsOpen = false;


    public void StartNewGame()
    {
        FileHandler.ClearSaveData();
        saveData.currLevel = 0;

        StartCoroutine(LoadLevel(levelNames[saveData.currLevel]));
    }

    public void Continue()
    {
        if (saveData.currLevel != 0)
        {
            StartCoroutine(LoadLevel(levelNames[saveData.currLevel]));
        }
        
    }

    public void LoadMainMenu()
    {
        
        StartCoroutine(LoadLevel(mainMenu));
        tankBody.ResetGame();
        ObjectPoolManager.Instance.ResetGame();
        
        pauseScreen.canPause = false;
    }
    #endregion

    private void Start()
    {
        tankBody = playerBody.GetComponent<TankBody>();
        controller = playerBody.GetComponentInParent<TankPlayerController>();

        
        
        

        saveData = FileHandler.Load(FileHandler.SaveType.Json);

        

        musicData = FileHandler.Load(FileHandler.SaveType.Binary);

       



        AudioManager.Instance.updateMixer();
        
        LoadMainMenu();

    }

    public void PlayerDied()
    {
        pauseScreen.canPause = false;
        gameOver.gameObject.SetActive(true);
    }

    public void MovePlayerToSpawn()
    {

        if (LevelManager.Instance)
        {
            pauseScreen.canPause = true;
            playerBody.transform.position = LevelManager.Instance.Spawn.position;
            playerBody.transform.rotation = LevelManager.Instance.Spawn.rotation;
        }
    }

    public void PlayerRespawn()
    {
        tankBody.ResetGame();
        MovePlayerToSpawn();
        ObjectPoolManager.Instance.ResetGame();
        LevelManager.Instance.ResetGame();
        pauseScreen.canPause = true;

    }

    public void OpenOptions()
    {
        optionsScreen.gameObject.SetActive(true);
        optionsOpen = true;
    }
    
    public void ExitOption()
    {
        optionsOpen = false;
        optionsScreen.gameObject.SetActive(false); 
    }
        

    

}
