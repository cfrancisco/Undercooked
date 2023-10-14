using UnityEngine;
using Undercooked.Data;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Undercooked.Managers;
using UnityEngine.Assertions;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorial")]
	public GameObject[] popUps;
    public GameObject backgroundScene;
    [SerializeField] private int popUpIndex;
    public float waitTime = 2f;
    static public bool isOrderShown = false;
    static public bool isTomateSettedOnTable = false;
    static public bool isTomateCutting = false; 
    static public bool wasCutted = false;
    static public bool isCutting = false;
    static public int isCooking = 0;
    static public bool isReady = false;
    static public bool isPlateOnTable = false;
    static public bool isDelivering = false;
    static public bool wasDeliver = false;

 
    [Header("Managers")]
    [SerializeField] private OrderManager orderManager;

    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    private InputAction _moveAction; 

    private void Awake()
    {
         #if UNITY_EDITOR
            Assert.IsNotNull(orderManager);
            Assert.IsNotNull(popUps);
            Assert.IsNotNull(backgroundScene);
        #endif
        _moveAction = playerInput.currentActionMap["Move"];
          
    }

    private void Start()
    {

        TutorialManager.isTomateSettedOnTable = false;
        TutorialManager.isTomateCutting = false; 
        TutorialManager.wasCutted = false;
        TutorialManager.isCutting = false;
        TutorialManager.isReady = false;
        TutorialManager.isPlateOnTable = false;
        TutorialManager.isDelivering = false;
        TutorialManager.wasDeliver = false;
        TutorialManager.isCooking = 0;
        this.popUpIndex = 0;
        this.waitTime = 8f;

        LevelData currentLevel = LevelManager.GetInstance().getCurrentLevel();   
        _moveAction.performed += checkArrows;

        if (currentLevel.levelIndex == 0)
        {
            this.backgroundScene.SetActive(true);
            popUps[this.popUpIndex].SetActive(true); 
        }
        else
        {
            _moveAction.performed -= checkArrows;
        }

    }


    public void Init()
    {   
        LevelData currentLevel = LevelManager.GetInstance().getCurrentLevel();   

        if (currentLevel.levelIndex == 0)
        {
            this.orderManager.TrySpawnFirstOrder();
            TutorialManager.isOrderShown = true;
        }
    }


    void Update()
    {
        LevelData currentLevel = LevelManager.GetInstance().getCurrentLevel();   
        if (currentLevel.levelIndex != 0)
            return;

        switch (this.popUpIndex)
        {
            
            case 0:
                this.wasOrderShown();
                break;
            case 1:
                //this.checkArrows();
                break;
            case 2:
                this.isTomateOverTable();
                break;
            case 3:
                this.isTomateBeingCutted();
                break;  
            case 4:
                this.cutTomatoCutted();
                break;
            case 5:
                this.addSlicesToCook();
                break;
            case 6:
                this.doItAgain();
                break;
            case 7:
                this.waitingBeMealReady();
                break;
            case 8:
               this.takeOffMealFromOven();
                break;
            case 9:
               this.requestAssistantToDeliverMeal();
                break;
            case 10:
                this.waitingDelivery();
                break;
            case 11:
               this.waitTime = 4f; // FIX THIS
               this.congratz();
                break;
            case 12:
               this.letsPlay();
                break;
            default:
                break;
        }
    }


    public void wasOrderShown()
    {
        if (TutorialManager.isOrderShown)
        {
            if(this.waitTime <= 0)
                this.NextPopup();
            else
                this.waitTime -= Time.deltaTime;
        }
    }


    public void isTomateOverTable()
    {
        if (TutorialManager.isTomateSettedOnTable)
        {
            this.NextPopup();
        }
    }

    public void isTomateBeingCutted()
    {
        if (TutorialManager.isTomateCutting)
        {
            this.NextPopup();
        }
    }

    public void cutTomatoCutted()
    {
        if (TutorialManager.wasCutted)
        {
            this.NextPopup();
        }
    }

    public void doItAgain()
    {
        if (TutorialManager.isCooking == 2)
        {
            this.NextPopup();
        }
    }

    public void addSlicesToCook()
    {
        if (TutorialManager.isCooking == 1)
        {
            this.NextPopup();
        }
    }

    public void waitingBeMealReady()
    {
        if (TutorialManager.isReady)
        {
            this.NextPopup();
        }
    }

    public void takeOffMealFromOven()
    {
        if (TutorialManager.isPlateOnTable && TutorialManager.isReady)
        {
            this.NextPopup();
        }
    }


    public void requestAssistantToDeliverMeal()
    {
        if (TutorialManager.isDelivering)
        {
            this.NextPopup();
        }
    }

    
     public void waitingDelivery()
    {
        if (TutorialManager.wasDeliver)
        {
            this.NextPopup();
        }
    }
    public void congratz()
    {
        if(this.waitTime <= 0)
            this.NextPopup();
        else
            this.waitTime -= Time.deltaTime;
    }

    public void letsPlay()
    {
      this.backgroundScene.SetActive(false);
    }

    public void waitingAssistentAction()
    {
         if(waitTime <= 0)
                this.NextPopup();
            else
                waitTime -= Time.deltaTime;
    }

    public void checkArrows(InputAction.CallbackContext context)
    {
        if (this.popUpIndex == 1)
        {
            Debug.Log("checkArrows");
            _moveAction.performed -= checkArrows;
            this.NextPopup();
        }

/*       if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A)|| Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.W))
       {
        if(waitTime <= 0)
        {
            _moveAction.performed -= checkArrows;
            this.NextPopup();
        }
        else
            waitTime -= Time.deltaTime;*/
       //}
    }

    public void NextPopup()
    {
        popUps[this.popUpIndex].SetActive(false);
        this.popUpIndex++;
        if (popUps[this.popUpIndex])
            popUps[this.popUpIndex].SetActive(true);
    }


}