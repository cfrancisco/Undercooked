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

        [SerializeField] public LevelData[] levels = new LevelData[11];

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
            LevelData lvTreino = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/LevelTreino.asset", typeof(LevelData));
            LevelData lv1 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level1.asset", typeof(LevelData));
            LevelData lv2 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level2.asset", typeof(LevelData));
            LevelData lv3 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level3.asset", typeof(LevelData));
            LevelData lv4 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level4.asset", typeof(LevelData));
            LevelData lv5 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level5.asset", typeof(LevelData));
            LevelData lv6 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level6.asset", typeof(LevelData));
            LevelData lv7 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level7.asset", typeof(LevelData));
            LevelData lv8 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level8.asset", typeof(LevelData));
            LevelData lv9 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level9.asset", typeof(LevelData));
            LevelData lv10 = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/Levels/Level10.asset", typeof(LevelData));
            LevelData lvBase = (LevelData)AssetDatabase.LoadAssetAtPath("Assets/Data/LevelSample.asset", typeof(LevelData));
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
     

        public void LoadNextLevel()
        {
            LevelManager.currentLevelIndex++;
            LevelManager.currentLevel = levels[LevelManager.currentLevelIndex];
            this.LoadGameScene();
        }
        
        public void LoadAssistantScene()
        {
            SceneManager.LoadScene(LevelManager.AssistantSceneName, LoadSceneMode.Single);
        }


        public void LoadMenuScene()
        {
            SceneManager.LoadScene(LevelManager.MenuSceneName, LoadSceneMode.Single);
        }

        public void LoadTutorialScene()
        {
            LevelManager.currentLevelIndex = -1; 
            SceneManager.LoadScene(LevelManager.TutorialSceneName, LoadSceneMode.Single);
        }

        public void LoadGameScene()
        {
            Debug.Log("Starting level "+LevelManager.currentLevelIndex );

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
            Debug.Log("ReloadGameScene");
            this.LoadGameScene();
        }



        public void setCurrentLevel(LevelData cl)
        {
            Debug.Log("Starting level "+cl.levelName);
            LevelManager.currentLevel = cl;
        }


        public LevelData getCurrentLevel()
        {
            return  this.levels[LevelManager.currentLevelIndex];;
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