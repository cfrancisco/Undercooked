using Undercooked.Model;
using Undercooked.Managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked.Requests
{

    public class RequestController : MonoBehaviour
    {
        [Header("Targets")]
        [SerializeField] public GameObject placeToPutElement1; 
        [SerializeField] public GameObject placeToPutElement2; 

        [Header("Current Request")]
        public Request currentRequest;
        public ResponseType _currentAssistantResponse;
        public int timestampStart;
        public int timestampEnd;


       // public List<Request> requestList = new List<Request>();
      
        [Header("Current Assistant")]
        public FollowAssistent assistentActions; 
        public GameObject _currentAssistant;
        public AudioClip songWheBusy;
        public AudioSource audioSource;
        [SerializeField] public bool _isIdle = true;

    


        private void Awake()
        {
            #if UNITY_EDITOR
                Assert.IsNotNull(placeToPutElement1);
                Assert.IsNotNull(placeToPutElement2);
            #endif
        }

        
        // Start is called before the first frame update
        void Start()
        {
        //    audioSource = GetComponent<AudioSource>();
            assistentActions = GetComponent<FollowAssistent>();
            _currentAssistant = GetComponent<LoadCharacter>().myCurrentAssistant;
        }

        public void Init()
        {
            this.currentRequest = null;
            assistentActions.newIdleState(true);
        }

        public void HandleAsking(Request request)
        {
            Debug.Log("Requisição: "+request._requestData.type);
            Debug.Log("_isIdle: "+_isIdle);
            if (!this._isIdle)
            {
                // I'm not idle
                this.triggerBusyBeep();
                return; 
            }

            bool resultBoard = this.CheckIfBoardIsFull(request);
            if (resultBoard)
            {
                Debug.Log("The board is full!!!");
                return; 
            }

            // i'm idle AND the board has empty space
            this.currentRequest = request;
        }

        public bool CheckIfBoardIsFull(Request request)
        {
            // always allow delivery order
            if (request._requestData.type == RequestType.DeliverOrder)
                return false;

            return !IsEmptyThisSlot(this.placeToPutElement1) && !IsEmptyThisSlot(this.placeToPutElement2);
        }

        private bool IsEmptyThisSlot(GameObject place)
        {
            var interactable =  place.GetComponentInChildren<Interactable>();
            interactable.CheckSlotOccupied();
            var _elementInteracable = interactable.CurrentPickable;
            if (_elementInteracable == null)
            {
                return true;
            }
            return false; 
        }

        public void triggerBusyBeep(){
            assistentActions.IAmBusy();
        }

        public void setIsIdle(bool state){
            var prevIdle = this._isIdle;
            this._isIdle = state;
            // prev false and current true 
            if (!prevIdle && this._isIdle)
            {
                this.SaveRequestInData();   
            }

            //  Clear fields
            if (this._isIdle == true)
            {
                this.currentRequest = null;
                this.timestampEnd = -1;
                this.timestampStart = -1;
            }
        }

        void Update()
        {
            if (this._isIdle && this.currentRequest != null)
            {
                assistentActions.newIdleState(false);

                this.StartRequest();
            }

        }

        public void StartRequest()
        {
            Request requestedAction = this.currentRequest;
        
            var assistantPersona = _currentAssistant.GetComponent<AssistentModel>();

            bool isAwake = assistantPersona.nextShouldHelp();

            this.timestampStart = this.gameObject.GetComponent<GameManager>().TimeRemaining;
     

            if (!isAwake) { // I'm sleeping
                assistentActions.BoringOperation(); 
    
                this._currentAssistantResponse =  ResponseType.Sleepy;

                return;
            }
            
            // I'm awake
            ResponseType response = assistantPersona.getFaceBasedPersona();

            this._currentAssistantResponse =  response;

            // Start operation
            // TODO boring Operation should be in assistent Action
            assistentActions.StartOperation(requestedAction, response);
        }
        
        public void SaveRequestInData()
        {
            this.timestampEnd = this.gameObject.GetComponent<GameManager>().TimeRemaining;

            // Save Data on CSV. This could be a Invoke. 
            RequestMade requestRealized = new RequestMade(this.timestampStart,this.timestampEnd,this._currentAssistantResponse, this.currentRequest.requestType);

            DebugUndercooked.DumpToConsole("Operation Realized: ", requestRealized);

        	DatabaseToCsv.GetInstance().setRequestMade(requestRealized);
        }
    }

}