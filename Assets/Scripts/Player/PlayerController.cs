using System.Collections;
using Lean.Transition;
using Undercooked.Appliances;
using Undercooked.Model;
using Undercooked.Requests;
using Undercooked.Data;
using Undercooked.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Undercooked.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Physics")]
        [SerializeField] private Color playerColor;
        [SerializeField] private Transform selector;
        [SerializeField] private Material playerUniqueColorMaterial;
        [SerializeField] private Rigidbody playerRigidbody;

        [Header("Animation")]
        [SerializeField] private Animator animator;
        private readonly int _isCleaningHash = Animator.StringToHash("isCleaning");
        private readonly int _hasPickupHash = Animator.StringToHash("hasPickup");
        private readonly int _isChoppingHash = Animator.StringToHash("isChopping");
        private readonly int _velocityHash = Animator.StringToHash("velocity");

        [Header("Input")]
        [SerializeField] private PlayerInput playerInput;
        private InputAction _moveAction;
    //    private InputAction _dashAction;
        private InputAction _pickUpAction;
        private InputAction _interactAction;
        private InputAction _startAtPlayerAction;
        private InputAction _dialogForChoppingAction;
        private InputAction _pauseAction;

        [Header("Dialogs")]
        [SerializeField] private GameObject dialogChopping;
        [SerializeField] private AudioClip dialogAudio;
       // [SerializeField] private Camera cam;
       //  [SerializeField] private Transform playerTransform;
        
        public RequestController _requestController;
        

        // Dashing
        [SerializeField] private float dashForce = 900f;
        private bool _isDashing = false;
        private bool _isDashingPossible = true;
        private readonly WaitForSeconds _dashDuration = new WaitForSeconds(0.17f);
        private readonly WaitForSeconds _dashCooldown = new WaitForSeconds(0.07f);

        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 5f;

        private InteractableController _interactableController;
        private bool _isActive;
        private IPickable _currentPickable;
        private Vector3 _inputDirection;
        private bool _hasSubscribedControllerEvents;

        [SerializeField] private Transform slot;
        [SerializeField] private ParticleSystem dashParticle;
        [SerializeField] private Transform knife;

        private DialogueSystem dialogueSystem;

        [Header("Audio")]
        [SerializeField] private AudioClip dashAudio;
        [SerializeField] private AudioClip pickupAudio;
        [SerializeField] private AudioClip dropAudio;


        
        private void Awake()
        {
            dialogueSystem = FindObjectOfType<DialogueSystem>();
    
            _moveAction = playerInput.currentActionMap["Move"];
            _dialogForChoppingAction = playerInput.currentActionMap["Dialog"];
            _pickUpAction = playerInput.currentActionMap["PickUp"];
            _interactAction = playerInput.currentActionMap["Interact"];
            _startAtPlayerAction = playerInput.currentActionMap["Start@Player"];
            _pauseAction = playerInput.currentActionMap["PauseGame"];
           
            _interactableController = GetComponentInChildren<InteractableController>();
            knife.gameObject.SetActive(false);

            SetPlayerUniqueColor(playerColor);
        }

        private void Start() {
            this.ActivatePlayer();
        }

        private void SetPlayerUniqueColor(Color color)
        {
            selector.GetComponent<MeshRenderer>().material.color = color;
            playerUniqueColorMaterial.color = color;
        }

        public void ActivatePlayer()
        {
            _isActive = true;
            SubscribeControllerEvents();
            selector.gameObject.SetActive(true);
        }

        // generate a message when the game shuts down or switches to another Scene
        void OnDestroy()
        {
            DeactivatePlayer();
        }


        public void DeactivatePlayer()
        {
            _isActive = false;
            UnsubscribeControllerEvents();
            animator.SetFloat(_velocityHash, 0f);
            selector.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SubscribeInteractableEvents();
        }

        private void OnDisable()
        {
            UnsubscribeInteractableEvents();
        }

        private void SubscribeControllerEvents()
        {
            if (_hasSubscribedControllerEvents) return;
            _hasSubscribedControllerEvents = true;
            _moveAction.performed += HandleMove;
           // _dashAction.performed += HandleDash;
           // _dialogForChoppingAction.performed += HandleCountertop;
           // _pickUpAction.performed += HandlePickUp;
            _pickUpAction.performed += HandleChoices;
            _interactAction.performed += HandleInteract;
            _pauseAction.performed += HandlePause;
        }

        private void UnsubscribeControllerEvents()
        {
            if (_hasSubscribedControllerEvents == false) return;

            _hasSubscribedControllerEvents = false;
            _moveAction.performed -= HandleMove;
          //  _dashAction.performed -= HandleDash;
          //  _dialogForChoppingAction.performed -= HandleCountertop;
          //  _pickUpAction.performed -= HandlePickUp;
            _pickUpAction.performed -= HandleChoices;
            _interactAction.performed -= HandleInteract;
            _pauseAction.performed -= HandlePause;
        }

        private void SubscribeInteractableEvents()
        {
            ChoppingBoard.OnChoppingStart += HandleChoppingStart;
            ChoppingBoard.OnChoppingStop += HandleChoppingStop;
            Sink.OnCleanStart += HandleCleanStart;
            Sink.OnCleanStop += HandleCleanStop;
        }

        private void UnsubscribeInteractableEvents()
        {
            ChoppingBoard.OnChoppingStart -= HandleChoppingStart;
            ChoppingBoard.OnChoppingStop -= HandleChoppingStop;
            Sink.OnCleanStart -= HandleCleanStart;
            Sink.OnCleanStop -= HandleCleanStop;
        }

        private void HandleCleanStart(PlayerController playerController)
        {
            if (Equals(playerController) == false) return;

            animator.SetBool(_isCleaningHash, true);
        }

        private void HandleCleanStop(PlayerController playerController)
        {
            if (Equals(playerController) == false) return;

            animator.SetBool(_isCleaningHash, false);
        }

        private void HandleChoppingStart(PlayerController playerController)
        {
            if (Equals(playerController) == false) return;

            animator.SetBool(_isChoppingHash, true);
            knife.gameObject.SetActive(true);
        }

        private void HandleChoppingStop(PlayerController playerController)
        {
            if (Equals(playerController) == false) return;

            animator.SetBool(_isChoppingHash, false);
            knife.gameObject.SetActive(false);
        }

        private void HandleDash(InputAction.CallbackContext context)
        {
            if (!_isDashingPossible) return;
            StartCoroutine(Dash());
        }

        private bool typeElementHandled(string nameElement)
        {
            if (nameElement == "Tomato(Clone)")
            {
                return true;
            }

            if (nameElement == "Onion(Clone)")
            {
                return true;
            }

            if (nameElement == "Plate")
            {
                return true;
            }
            return false;
        }


         private void HandleChoices(InputAction.CallbackContext context)
        {
          //  Debug.Log($"[PlayerController] HandleCountertop: {transform.position}");
            // TODO Procurar aqui
            // StopAllCoroutines();
                    
            var interactable = _interactableController.CurrentInteractable;
            //Debug.Log("interactable com Space: "+interactable);
            //Debug.Log(" interactable Tomato: "+interactable.CurrentPickable);
            
            var getCountertop = interactable.GetComponent<Countertop>();
            var isMiddle = false;
            if (getCountertop && getCountertop.isMiddle)
                isMiddle = true;
    
            Debug.Log("Element is in the Middle Countertop: "+getCountertop?.isMiddle);

            if (!isMiddle)
            {
                this.HandlePickUp(context);
            }

            // TODO: dividir isto em Tutorial e jogo normal
            if (isMiddle)
            {
                var isInMyHand = false;
                if (interactable.CurrentPickable == null)
                {
                    isInMyHand = true;
                }
                // está no meio
                Debug.Log("isInMyHand: "+isInMyHand);
                
                if (isInMyHand)
                {
                    // lidar como handle V   
                    TutorialManager.isTomateSettedOnTable = true;
                    if (TutorialManager.isReady)
                       TutorialManager.isPlateOnTable = true;

                    this.HandlePickUp(context);
                }

                if (!isInMyHand)
                {
                 
                    TutorialManager.isTomateCutting = true;
                    if (TutorialManager.isReady)
                       TutorialManager.isDelivering = true;

                    // lidar com o Space
                    this.HandleCountertop(context);
                }
                // será que colocamos para mesa

                // sera que ja esta na mesa?
            }
        }

        /***

            ---------------------------------------
               Request Help for AssistantDialog
            ---------------------------------------
          
        **/
        private void HandleCountertop(InputAction.CallbackContext context)
        {
          //  Debug.Log($"[PlayerController] HandleCountertop: {transform.position}");
            StopAllCoroutines();
                    
            var interactable = _interactableController.CurrentInteractable;
           
 
            if (interactable.CurrentPickable == null) return;
            try { 
                    var nextCurrentPickable =  interactable.CurrentPickable;

                    Debug.Log("nextCurrentPickable?.gameObject.name: "+nextCurrentPickable?.gameObject.name);
                    // I will request help.
                    Request myRequest = new Request();  
                    myRequest.setCurrentPickable(nextCurrentPickable?.gameObject);

                    if (nextCurrentPickable?.gameObject.name == "Tomato(Clone)")
                    {
                        myRequest.setRequestType(RequestType.CutTomato);
                    }

                    if (nextCurrentPickable?.gameObject.name == "Onion(Clone)")
                    {
                        myRequest.setRequestType(RequestType.CutOnion);
                    }

                    if (nextCurrentPickable?.gameObject.name == "Plate")
                    {
                        myRequest.setRequestType(RequestType.DeliverOrder);
                    }


                    if (nextCurrentPickable?.gameObject.name == "DEBUG")
                    {
                        myRequest.setRequestType(RequestType.GetRandomElement);
                    }

                    requestHelpFor(myRequest);

            } catch {
                Debug.Log("Horrible things happened!");
            }
       
        }




        public void requestHelpFor(Request myRequest)
        { 
            // Dialog
            dialogueSystem.dialogueLines = myRequest._requestData.messageToAsk;
            FindObjectOfType<DialogueSystem>().StartTalking();

            // Audio
            this.PlaySoundTransition(dialogAudio);

            // Request
            // TODO: this could be a Invoke method
            _requestController.HandleAsking(myRequest);

            // Close Dialog
            StartCoroutine(DisableDialog());
        }


        private IEnumerator DisableDialog()
        {
            yield return new WaitForSeconds(7);
            //dialogChopping.SetActive(false);
            FindObjectOfType<DialogueSystem>().OutOfRange();
        }
        

        /*
     public void OnTriggerStay(Collider other)
        {
            this.gameObject.GetComponent<NPC>().enabled = true;
            FindObjectOfType<DialogueSystem>().EnterRangeOfNPC();
            if ((other.gameObject.tag == "Player") && Input.GetKeyDown(KeyCode.F))
            {
                this.gameObject.GetComponent<NPC>().enabled = true;
                dialogueSystem.Names = Name;
                dialogueSystem.dialogueLines = sentences;
                FindObjectOfType<DialogueSystem>().NPCName();
            }
        }*/
 

        private IEnumerator Dash()
        {
            _isDashingPossible = false;
            playerRigidbody.AddRelativeForce(dashForce * Vector3.forward);
            dashParticle.Play();
            dashParticle.PlaySoundTransition(dashAudio);

            yield return new WaitForFixedUpdate();
            _isDashing = true;
            yield return _dashDuration;
            _isDashing = false;
            yield return _dashCooldown;
            _isDashingPossible = true;
        }
        // Handle operations

        private void HandlePickUp(InputAction.CallbackContext context)
        {
            var interactable = _interactableController.CurrentInteractable;
        
            // empty hands, try to pick
            if (_currentPickable == null)
            {
                _currentPickable = interactable as IPickable;
                if (_currentPickable != null)
                {
                    animator.SetBool(_hasPickupHash, true);
                    this.PlaySoundTransition(pickupAudio);
                    _currentPickable.Pick();
                    _interactableController.Remove(_currentPickable as Interactable);
                    _currentPickable.gameObject.transform.SetPositionAndRotation(slot.transform.position,
                        Quaternion.identity);
                    _currentPickable.gameObject.transform.SetParent(slot);
                    return;
                }

                // Interactable only (not a IPickable)
                _currentPickable = interactable?.TryToPickUpFromSlot(_currentPickable);
                if (_currentPickable != null)
                {
                    animator.SetBool(_hasPickupHash, true);
                    this.PlaySoundTransition(pickupAudio);
                }

                _currentPickable?.gameObject.transform.SetPositionAndRotation(
                    slot.position, Quaternion.identity);
                _currentPickable?.gameObject.transform.SetParent(slot);
                return;
            }

            // we carry a pickable, let's try to drop it (we may fail)

            // no interactable in range or at most a Pickable in range (we ignore it)
            if (interactable == null || interactable is IPickable)
            {
                animator.SetBool(_hasPickupHash, false);
                this.PlaySoundTransition(dropAudio);
                _currentPickable.Drop();
                _currentPickable = null;
                return;
            }

            // we carry a pickable and we have an interactable in range
            // we may drop into the interactable

            // Try to drop on the interactable. It may refuse it, e.g. dropping a plate into the CuttingBoard,
            // or simply it already have something on it
            //Debug.Log($"[PlayerController] {_currentPickable.gameObject.name} trying to drop into {interactable.gameObject.name} ");

            bool dropSuccess = interactable.TryToDropIntoSlot(_currentPickable);
            if (!dropSuccess) return;

            animator.SetBool(_hasPickupHash, false);
            this.PlaySoundTransition(dropAudio);
            _currentPickable = null;
        }

        private void HandleMove(InputAction.CallbackContext context)
        {
           //  dialogChopping.SetActive(false);
            // TODO: Processors on input binding not working for analogical stick. Investigate it.
            Vector2 inputMovement = context.ReadValue<Vector2>();
            if (inputMovement.x > 0.3f)
            {
                inputMovement.x = 1f;
            }
            else if (inputMovement.x < -0.3)
            {
                inputMovement.x = -1f;
            }
            else
            {
                inputMovement.x = 0f;
            }

            if (inputMovement.y > 0.3f)
            {
                inputMovement.y = 1f;
            }
            else if (inputMovement.y < -0.3f)
            {
                inputMovement.y = -1f;
            }
            else
            {
                inputMovement.y = 0f;
            }

            _inputDirection = new Vector3(inputMovement.x, 0, inputMovement.y);
        }

        private void HandleInteract(InputAction.CallbackContext context)
        {
            _interactableController.CurrentInteractable?.Interact(this);
        }

        private void HandleStart(InputAction.CallbackContext context)
        {
            MenuPanelUI.PauseUnpause();
        }

        private void HandlePause(InputAction.CallbackContext context)
        {
            MenuPanelUI.OpenPause();
        }

        private void UpdateDialogueBox()
        {
              // Dialogue box update
            Vector3 Pos = Camera.main.WorldToScreenPoint(transform.position);
            Pos.y += 100;
            dialogChopping.transform.position = Pos;
         
        }

        private void Update()
        {
            if (!_isActive) return;

            UpdateDialogueBox();

            CalculateInputDirection();
        }

        private void FixedUpdate()
        {
            if (!_isActive) return;
            MoveThePlayer();
            AnimatePlayerMovement();
            TurnThePlayer();
        }

        // Moving the Chef
    
        private void MoveThePlayer()
        {
            if (_isDashing)
            {
                var currentVelocity = playerRigidbody.velocity.magnitude;

                var inputNormalized = _inputDirection.normalized;
                if (inputNormalized == Vector3.zero)
                {
                    inputNormalized = transform.forward;
                }

                playerRigidbody.velocity = inputNormalized * currentVelocity;
            }
            else
            {
                playerRigidbody.velocity = _inputDirection.normalized * movementSpeed;
            }
        }

        private void CalculateInputDirection()
        {
            var inputMovement = _moveAction.ReadValue<Vector2>();
            if (inputMovement.x > 0.3f)
            {
                inputMovement.x = 1f;
            }
            else if (inputMovement.x < -0.3)
            {
                inputMovement.x = -1f;
            }
            else
            {
                inputMovement.x = 0f;
            }

            if (inputMovement.y > 0.3f)
            {
                inputMovement.y = 1f;
            }
            else if (inputMovement.y < -0.3f)
            {
                inputMovement.y = -1f;
            }
            else
            {
                inputMovement.y = 0f;
            }

            _inputDirection = new Vector3(inputMovement.x, 0f, inputMovement.y);
        }

        private void TurnThePlayer()
        {
            if (!(playerRigidbody.velocity.magnitude > 0.1f) || _inputDirection == Vector3.zero) return;

            Quaternion newRotation = Quaternion.LookRotation(_inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 15f);
        }

        private void AnimatePlayerMovement()
        {
            animator.SetFloat(_velocityHash, _inputDirection.sqrMagnitude);
        }
    }
}
