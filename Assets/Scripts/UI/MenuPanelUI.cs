using DaltonLima.Core;
using Lean.Transition;
using TMPro;
using Undercooked.Data;
using Undercooked.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Undercooked.UI
{
    public class MenuPanelUI : Singleton<MenuPanelUI>
    {
       // [Header("Notification UI")]
       // [SerializeField] private GameObject notificationUi;
      

        [Header("Title Animation")]
        [SerializeField] private GameObject titleMenu;
      
        [Header("InitialMenu")]
        [SerializeField] private GameObject initialMenu;
        /*
        [SerializeField] private Button mainQuitButton;
        [SerializeField] private Button startTrainButton;
        [SerializeField] private Button startGameButton;
        */
        private CanvasGroup _initalMenuCanvasGroup;
        [Space]
        
        [Header("PauseMenu")]
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject feedbackMenu;
        [SerializeField] private GameObject gameOverMenu;

        private CanvasGroup _pauseMenuCanvasGroup;
        
        [SerializeField] private GameObject PanasPanel;

        [Header("Buttons")]
        [SerializeField] private GameObject firstSelectedPauseMenu;
        [SerializeField] private Button quitButton_Pause;
        [Space]
        


        [Header("GameOverMenu")]
        private CanvasGroup _gameOverMenuCanvasGroup;
        [SerializeField] private GameObject firstSelectedGameOverMenu;
        [SerializeField] private AudioClip successClip;
        
        [Header("Buttons")]
        [SerializeField] private Button restartButton_GameOver;
        [SerializeField] private Button quitButton_GameOver;

        [Header("GameOver Stars")]
        [SerializeField] private Image star1;
        [SerializeField] private Image star2;
        [SerializeField] private Image star3;
        [SerializeField] private TextMeshProUGUI scoreStar1Text;
        [SerializeField] private TextMeshProUGUI scoreStar2Text;
        [SerializeField] private TextMeshProUGUI scoreStar3Text;
        [SerializeField] private TextMeshProUGUI scoreText;

        public delegate void ButtonPressed();
        public static ButtonPressed OnResumeButton;
        public static ButtonPressed OnRestartButton;
        public static ButtonPressed OnQuitButton;
        
        
        private void Awake()
        {
            _initalMenuCanvasGroup = initialMenu.GetComponent<CanvasGroup>();
            _pauseMenuCanvasGroup = pauseMenu.GetComponent<CanvasGroup>();
            _gameOverMenuCanvasGroup = gameOverMenu.GetComponent<CanvasGroup>();
            
            #if UNITY_EDITOR
                Assert.IsNotNull(initialMenu);
                Assert.IsNotNull(pauseMenu);
                Assert.IsNotNull(gameOverMenu);
                Assert.IsNotNull(feedbackMenu);
                Assert.IsNotNull(_initalMenuCanvasGroup);
                Assert.IsNotNull(_gameOverMenuCanvasGroup);
                Assert.IsNotNull(_pauseMenuCanvasGroup);
                Assert.IsNotNull(star1);
                Assert.IsNotNull(star2);
                Assert.IsNotNull(star3);
                Assert.IsNotNull(scoreStar1Text);
                Assert.IsNotNull(scoreStar2Text);
                Assert.IsNotNull(scoreStar3Text);
                Assert.IsNotNull(scoreText);
            #endif
            
           // initialMenu.SetActive(false);
         //   pauseMenu.SetActive(false);
         //   gameOverMenu.SetActive(false);
            //_initalMenuCanvasGroup.alpha = 0f;
           // _pauseMenuCanvasGroup.alpha = 0f;
           // _gameOverMenuCanvasGroup.alpha = 0f;
        }
        public void AnimationUp()
        {
            Vector3 _inputDirection = new Vector3(0.5f, 0.5f, 0.5f);

            titleMenu.transform.localScale =_inputDirection;
            titleMenu.transform
                .localScaleTransition(Vector3.one, 2f, LeanEase.Bounce);
        }

        /*
        private void AddButtonListeners()
        {
            // For Main Menu.
            mainQuitButton.onClick.AddListener(HandleQuitButton);
            startTrainButton.onClick.AddListener(HandleStartTrainButton);
            startGameButton.onClick.AddListener(HandleStartGameButton);
        }

        private void RemoveButtonListeners()
        {
           // restartButton_GameOver.onClick.RemoveAllListeners();
            mainQuitButton.onClick.RemoveAllListeners();
            startTrainButton.onClick.RemoveAllListeners();
            startGameButton.onClick.RemoveAllListeners();
        }
        */

        public static void HandleResumeButton()
        {
            Debug.Log("HandleResumeButton");
         //   OnResumeButton?.Invoke();
            MenuPanelUI.Unpause();
        }

        public static void HandleRestartButton()
        {
         
            LevelManager.GetInstance().ReloadGameScene();
            //GameOverMenu();
            //OnRestartButton?.Invoke();
        }

        public static void HandleQuitButton()
        {
            Application.Quit();
        }
        
        public static void OpenColectMoodBox()
        {
            Instance.feedbackMenu.SetActive(true);
        }


        public static void HandleStartTrainButton()
        { 
            MenuPanelUI.OpenColectMoodBox();
        }


        public static void SendEmojigridToDatabase()
        {
        
            LevelData currentLevel = LevelManager.GetInstance().getCurrentLevel();   
            Debug.Log("currentLevel.levelIndex: "+currentLevel.levelIndex);
            /*if (currentLevel.levelIndex == 0)
            {
                DatabaseToCsv.GetInstance().setCurrentLevel(-1); // -1 is before tutorial
            }*/
            Vector3 mousePos = Input.mousePosition;
            DatabaseToCsv.GetInstance().setEmotionFeedback(mousePos.x, mousePos.y);
            // Save data in CSV
            DatabaseToCsv.GetInstance().writeCSV();
      

            
          //  if (goToLoadAssistantScene)
            if (currentLevel.levelIndex == 0)
            {
                MenuPanelUI.StartTrainButton();
            }
            else
            {
                MenuPanelUI.HandleStartGameButton();    
            }
        }

        public static void StartTrainButton()
        {
            Debug.Log("StartTrainButton");
            LevelManager.GetInstance().LoadTutorialScene();
        }

        public static void HandleOpenTCLE()
        {
            Debug.Log("HandleOpenTCLE");
        }


        public static void HandleStartGameButton()
        {
            Debug.Log("HandleStartGameButton");
            LevelManager.GetInstance().LoadAssistantScene();
        }

        public static void HandleGoToMenu()
        {
            Debug.Log("HandleGoToMenu");
            Instance.PanasPanel?.SetActive(false);
            SceneManager.LoadScene(LevelManager.MenuSceneName, LoadSceneMode.Single);
        }

        public static void HandleGoToMenuScene()
        {
            Debug.Log("HandleGoToMenuScene");
            SceneManager.LoadScene(LevelManager.MenuSceneName, LoadSceneMode.Single);
        }

        public static void HandleGoToFeedbackMenu()
        {
            Instance.gameOverMenu.SetActive(false);
            Instance.feedbackMenu.SetActive(true);
        }

        public static void HandleGoToPanas()
        {
            Instance.initialMenu.SetActive(false);
            Instance.PanasPanel.SetActive(true);
        }
        
/*
        public static void InitialMenuSetActive(bool active)
        {
            Instance.initialMenu.SetActive(active);
         //   Instance._initalMenuCanvasGroup.alphaTransition(active ? 1f : 0f, 2f);
        }
        */
        public static void Unpause()
        {
            Instance.pauseMenu.SetActive(false);
            Instance._pauseMenuCanvasGroup
                    .alphaTransition(0f, .5f)
                    .JoinTransition()
                    .EventTransition(() => Instance.pauseMenu.SetActive(false))
                    .EventTransition(() => Time.timeScale = 1);
        }

        public static void OpenPause()
        {
            if (Instance.pauseMenu.activeInHierarchy == false)
            {
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Instance.firstSelectedPauseMenu);
                
                Instance.pauseMenu.SetActive(true);
                Instance._pauseMenuCanvasGroup.alphaTransition(1f, .5f);
            }
        }


        public static void PauseUnpause()
        {
            if (Instance.pauseMenu.activeInHierarchy == false)
            {
                MenuPanelUI.OpenPause();
            }
            else
            {
                MenuPanelUI.Unpause();
            }
        }

        public static void GameOverMenu()
        { 
            if (Instance.gameOverMenu.activeInHierarchy == false)
            {
                if (Instance.pauseMenu.activeInHierarchy)
                {
                    PauseUnpause();
                }

                Instance.gameOverMenu.SetActive(true);
                Time.timeScale = 0;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(Instance.firstSelectedGameOverMenu);   
                
                Instance.gameOverMenu.SetActive(true);
                Instance._gameOverMenuCanvasGroup.alphaTransition(1f, .5f);

                UpdateStars();
            }
            else
            {
                Instance.gameOverMenu.SetActive(false);
                Instance._gameOverMenuCanvasGroup
                    .alphaTransition(0f, .5f)
                    .JoinTransition()
                    .EventTransition(() => Instance.gameOverMenu.SetActive(false))
                    .EventTransition(() => Time.timeScale = 1);
            }
        }

        /*  TODO: remove this to someelse place */
        private static void UpdateStars()
        {
            int score = GameManager.Score;
            LevelData levelData = GameManager.LevelData;
            int star1Score = levelData.star1Score;
            int star2Score = levelData.star2Score;
            int star3Score = levelData.star3Score;
            Instance.scoreStar1Text.text = star1Score.ToString();
            Instance.scoreStar2Text.text = star2Score.ToString();
            Instance.scoreStar3Text.text = star3Score.ToString();
            Instance.scoreText.text = $"Score {score.ToString()}";
            
            Instance.star1.gameObject.transform.localScale = Vector3.zero;
            Instance.star2.gameObject.transform.localScale = Vector3.zero;
            Instance.star3.gameObject.transform.localScale = Vector3.zero;
            
            if (score < star1Score) return;
            
            if (score < star2Score)
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);
            }
            else if (score < star3Score)
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star2.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);
            }
            else
            {
                Instance.star1.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star2.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce)
                    .JoinTransition();
                Instance.star3.gameObject.transform
                    .localScaleTransition(Vector3.one, 1f, LeanEase.Bounce);
            }
        }
    }
    
}
