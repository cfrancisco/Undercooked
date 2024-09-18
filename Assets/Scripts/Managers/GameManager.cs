using System.Collections;
using System.Threading.Tasks;
using DaltonLima.Core;
using Undercooked.Appliances;
using Undercooked.Data;
using Undercooked.Model;
using Undercooked.Player;
using Undercooked.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using TMPro;

namespace Undercooked.Managers
{
    [RequireComponent(typeof(OrderManager))]
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private DishTray dishTray;
        [SerializeField] private OrderManager orderManager;
        [SerializeField] private TutorialManager tutorialManager;
        [SerializeField] private LevelData level1;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private InputController inputController;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private GameObject notificationBox;
        [SerializeField] private TextMeshProUGUI textPhaseName;

        // [SerializeField] private DatabaseToCsv databaseToCsv;

        public const int BaseScorePerPlate = 20;
        public const int PenaltyExpiredScore = 10;
        public const float TimeToReturnPlateSeconds = 3f;

        private Coroutine _countdownCoroutine;
        private readonly WaitForSeconds _timeToReturnPlate = new WaitForSeconds(TimeToReturnPlateSeconds);
        private readonly WaitForSeconds _oneSecondWait = new WaitForSeconds(1f);
        private static int _score;
        public int TimeRemaining;

        public static int Score
        {
            get => _score;
            private set
            {
                var previous = _score;
                _score = value;
                OnScoreUpdate?.Invoke(_score, _score - previous);
            }
        }
        /*
                public int TimeRemaining
                {
                    get => TimeRemaining;
                    private set
                    {
                        TimeRemaining = value;
                        OnCountdownTick?.Invoke(TimeRemaining);
                    }
                }*/

        public static LevelData LevelData => Instance.level1;

        public delegate void CountdownTick(int TimeRemaining);
        public static event CountdownTick OnCountdownTick;
        public delegate void ScoreUpdate(int score, int delta);
        public static event ScoreUpdate OnScoreUpdate;
        public delegate void DisplayNotification(string textToDisplay, Color color, float timeToDisplay);
        public static event DisplayNotification OnDisplayNotification;
        public delegate void TimeIsOver();
        public static event TimeIsOver OnTimeIsOver;
        public delegate void LevelStart();
        public static event LevelStart OnLevelStart;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(dishTray);
            Assert.IsNotNull(orderManager);
            Assert.IsNotNull(tutorialManager);
            Assert.IsNotNull(level1);
            Assert.IsNotNull(cameraManager);
            Assert.IsNotNull(inputController);
            Assert.IsNotNull(textPhaseName);
#endif
        }

        private async void Start()
        {
            Time.timeScale = 1;
            await GameLoop();
        }

        private void OnDestroy()
        {
            //  _userPressedStart = true;
        }

        private async Task GameLoop()
        {
            cameraManager.SwitchDollyCamera();

            // MenuPanelUI.InitialMenuSetActive(true);
            await Task.Delay(3000);
            //await StartMainMenuAsync();  // maybe move this out to Start()
            LevelData currentLevel = LevelManager.GetInstance().getCurrentLevel();
            Debug.Log("----------------------------");
            DebugUndercooked.DumpToConsole("[GameManager] currentLevel", currentLevel);

            await StartLevelAsync(currentLevel);
        }

        //    private bool _userPressedStart;
        /*    
        private async Task StartMainMenuAsync()
        {
            await Task.Delay(1000);
            cameraManager.SwitchDollyCamera();
            
            // activate MenuControls
            inputController.EnableMenuControls();
        //    inputController.OnStartPressedAtMenu += HandleStartAtMenu;
            
           /* while (_userPressedStart == false)
            {
                await Task.Delay(1000);
            }

           // MenuPanelUI.InitialMenuSetActive(false);
            //inputController.OnStartPressedAtMenu -= HandleStartAtMenu;
        }
        */
        /*
            private void HandleStartAtMenu()
            {
                _userPressedStart = true;
            }
        */

        private async Task StartLevelAsync(LevelData levelData)
        {

            //_startAtMenuAction = playerInput.currentActionMap["Start@Menu"];
            // _startAtMenuAction.performed += HandleStart;
            textPhaseName.text = levelData.levelName;

            cameraManager.FocusFirstPlayer();

            Score = 0;
            TimeRemaining = levelData.durationTime;
            notificationBox.SetActive(true);

            await DisplayInitialNotifications();
            notificationBox.SetActive(false);

            orderManager.Init(levelData);
            tutorialManager.Init();

            OnLevelStart?.Invoke();

            // Unlock player movement
            // inputController.EnableGameplayControls();
            // inputController.EnableFirstPlayerController();

            inputController.OnStartPressedAtPlayer += HandlePausePressed;

            _countdownCoroutine = StartCoroutine(CountdownTimer(TimeRemaining));

            //TODO: handle pause (pause timer coroutine and restart)
        }

        private void HandlePausePressed()
        {
            MenuPanelUI.PauseUnpause();
        }

        private InputAction _startAtPlayerAction;
        private bool _hasSubscribedPlayerActions;
        private bool _hasSubscribedMenuActions;

        private static async Task DisplayInitialNotifications()
        {
            await NotificationUI.DisplayCenterNotificationAsync("Pronto?", new Color(.66f, .367f, .15f), 2f);
            await NotificationUI.DisplayCenterNotificationAsync("Vai!", new Color(.333f, .733f, .196f), 2f);
        }

        private void OnEnable()
        {
            DeliverCountertop.OnPlateDropped += HandlePlateDropped;
            OrderManager.OnOrderExpired += HandleOrderExpired;
            OrderManager.OnOrderDelivered += HandleOrderDelivered;
        }

        private void OnDisable()
        {
            DeliverCountertop.OnPlateDropped -= HandlePlateDropped;
            OrderManager.OnOrderExpired -= HandleOrderExpired;
            OrderManager.OnOrderDelivered -= HandleOrderDelivered;
        }

        private static void HandleResumeButton()
        {
            MenuPanelUI.PauseUnpause();
        }


        private void HandleOrderDelivered(Order order, int tipCalculated)
        {
            Score += BaseScorePerPlate + tipCalculated;
        }

        private void HandleOrderExpired(Order order)
        {
            Score -= PenaltyExpiredScore;
        }

        public void Pause()
        {
            // player controller ignore input
            MenuPanelUI.PauseUnpause();
            //StopCoroutine(_countdownCoroutine);
        }

        public void Unpause()
        {
            // restore player input (event?)
            //Time.timeScale = 1;
            MenuPanelUI.PauseUnpause();
            //_countdownCoroutine = StartCoroutine(CountdownTimer(TimeRemaining));
        }

        private void HandlePlateDropped(Plate plate)
        {
            /*
                    if (plate.IsEmpty() || plate.IsClean == false)
                    {
                        Debug.Log("[HandlePlateDropped] 1 ");
                        plate.RemoveAllIngredients();
                        StartCoroutine(ReturnPlateDirty(plate));
                        return;
                    }    */
            orderManager.CheckIngredientsMatchOrder(plate.Ingredients);
            plate.RemoveAllIngredients();
            StartCoroutine(ReturnPlateDirty(plate));
        }

        private IEnumerator ReturnPlateDirty(Plate plate)
        {
            plate.gameObject.SetActive(true);
            // For Now, we disable the Dirty/Clean mechanic.
            // plate.SetDirty();
            yield return _timeToReturnPlate;
            dishTray.AddDirtyPlate(plate);
        }

        private IEnumerator CountdownTimer(int timeInSeconds)
        {
            TimeRemaining = timeInSeconds;
            while (TimeRemaining > 0)
            {
                TimeRemaining--;
                yield return _oneSecondWait;
                OnCountdownTick?.Invoke(TimeRemaining);
            }

            TimeOver();
        }

        private async void TimeOver()
        {
            // Take control from player
            // inputController.DisableAllPlayerControllers();
            notificationBox.SetActive(true);

            await NotificationUI.DisplayCenterNotificationAsync("Tempo Encerrado!", new Color(.66f, .367f, .15f), 3f);
            notificationBox.SetActive(false);

            inputController.OnStartPressedAtMenu -= HandlePausePressed;
            //inputController.EnableMenuControls();

            // pause time?
            //Time.timeScale = 0;
            MenuPanelUI.GoFeedbackDifficultMenu();

            // Stop OrderManager
            orderManager.StopAndClear();


            // TODO: Show summary end screen - prompt to play again


        }
    }
}