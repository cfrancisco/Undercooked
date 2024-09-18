using UnityEngine;
using UnityEngine.SceneManagement;
using DaltonLima.Core;
using Undercooked.Data;
using UnityEditor;

namespace Undercooked.Managers
{

    // NB: ScriptableObject Singleton needed for Serialization
    // even during play mode recompiles
    public class LevelManager : ScriptableObject
    {

        [SerializeField] public LevelData[] levels = new LevelData[12];

        public static string AssistantSceneName = "SelectAssistant";
        public static string TutorialSceneName = "TutorialPhase";
        public static string MenuSceneName = "Menu";
        public static string TrainingSceneName = "LevelTraining";
        public static string GameSceneName = "LevelTraining";


        public static int currentLevelIndex = 0;
        public static LevelData currentLevel;
        public static LevelManager _instance;

        private void StartLevels()
        {
            LevelManager.currentLevelIndex = 0;

            // Loading Levels
            LevelData lvTreino = Resources.Load<LevelData>("Levels/LevelTreino");
            DebugUndercooked.DumpToConsole("lvtreino: ", lvTreino);
            LevelData lv1 = Resources.Load<LevelData>("Levels/Level1");
            LevelData lv2 = Resources.Load<LevelData>("Levels/Level2");
            LevelData lv3 = Resources.Load<LevelData>("Levels/Level3");
            LevelData lv4 = Resources.Load<LevelData>("Levels/Level4");
            LevelData lv5 = Resources.Load<LevelData>("Levels/Level5");
            LevelData lv6 = Resources.Load<LevelData>("Levels/Level6");
            LevelData lv7 = Resources.Load<LevelData>("Levels/Level7");
            LevelData lv8 = Resources.Load<LevelData>("Levels/Level8");
            LevelData lv9 = Resources.Load<LevelData>("Levels/Level9");
            LevelData lv10 = Resources.Load<LevelData>("Levels/Level10");
            //LevelData lvBase = (LevelData)Resources.Load("Data/LevelSample.asset");
            //lvBase.levelIndex = 0;  
            //lvBase.levelName = "Treino";
            this.levels[0] = lvTreino;
            this.levels[1] = lv1;
            this.levels[2] = lv2;
            this.levels[3] = lv3;
            this.levels[4] = lv4;
            this.levels[5] = lv5;
            this.levels[6] = lv6;
            this.levels[7] = lv7;
            this.levels[8] = lv8;
            this.levels[9] = lv9;
            this.levels[10] = lv10;
            this.levels[11] = lvTreino;
        }
        /*
                private void Awake()
                {

                    // start of new code
                    if (_instance != null)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    // end of new code

                    _instance = this;
                    DontDestroyOnLoad(gameObject);
                }
        */
        public static LevelManager GetInstance()
        {
            if (!_instance)
            {
                // NB: FindObjectOfType is used to retrieve the instance
                // when play mode recompiles
                _instance = FindObjectOfType<LevelManager>();
            }
            if (!_instance)
            {

                // NB: create the Singleton, and initialise its values
                _instance = CreateInstance<LevelManager>();
                _instance.StartLevels();
            }
            return _instance;
        }

        public void UpToNextLevel()
        {
            // Setting the Score in last levelData; 
            Debug.Log("[LevelManager] UpToNextLevel.");
            this.levels[LevelManager.currentLevelIndex].setLastScore(GameManager.Score);

            LevelManager.currentLevelIndex++;
            LevelManager.currentLevel = levels[LevelManager.currentLevelIndex];
        }


        public void LoadAssistantScene()
        {
            Debug.Log("[Level Manager] Loading Assistant Selection Scene. ");
            SceneManager.LoadScene(LevelManager.AssistantSceneName, LoadSceneMode.Single);
        }


        public void LoadMenuScene()
        {
            Debug.Log("[Level Manager] Loading Main Menu Scene. ");
            SceneManager.LoadScene(LevelManager.MenuSceneName, LoadSceneMode.Single);
        }
        public void LoadTutorialScene()
        {
            Debug.Log("[Level Manager] Loading Written Tutorial. ");
            LevelManager.currentLevelIndex = 0;

            SceneManager.LoadScene(LevelManager.TutorialSceneName, LoadSceneMode.Single);
        }

        public void LoadGameScene()
        {
            Debug.Log("[Level Manager] currentLevelIndex: " + LevelManager.currentLevelIndex);

            Debug.Log(">> Starting level " + LevelManager.currentLevelIndex);
            DatabaseToCsv.GetInstance().CleanLevel();

            DatabaseToCsv.GetInstance().setCurrentLevel(LevelManager.currentLevelIndex);


            if (LevelManager.currentLevelIndex == 0)
            {
                // Load Training
                SceneManager.LoadScene(LevelManager.TrainingSceneName, LoadSceneMode.Single);
            }
            else
            {
                // load game scene
                SceneManager.LoadScene(LevelManager.GameSceneName, LoadSceneMode.Single);
            }

        }


        public void ReloadGameScene()
        {
            Debug.Log("[Level Manager] Reload Game Scene.");

            this.LoadGameScene();
        }



        public void setCurrentLevel(LevelData cl)
        {
            Debug.Log(">> Starting level " + cl.levelName);
            LevelManager.currentLevel = cl;
        }


        public LevelData getCurrentLevel()
        {
            return this.levels[LevelManager.currentLevelIndex];
        }


        public LevelData getLevelN(int levelIndex)
        {
            return this.levels[levelIndex];
        }

        public void goToLevelN(int _currentLevelIndex)
        {
            LevelManager.currentLevelIndex = _currentLevelIndex;
            LevelManager.currentLevel = this.levels[_currentLevelIndex];
        }

        public void goToNextLevel()
        {
            if (currentLevelIndex == 10)
            {
                this.goToEndGame();
            }

            LevelManager.currentLevelIndex++;
            LevelManager.currentLevel = this.levels[LevelManager.currentLevelIndex];
        }
        public void goToEndGame()
        {
            Debug.Log("GoToEndGame");
        }
    }
}