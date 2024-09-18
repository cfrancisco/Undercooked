using Undercooked.Model;
using UnityEngine;
using Undercooked.Player;
using Undercooked.Appliances;
using System.Collections;

namespace Undercooked.Requests
{

    public class PathForActions
    //: MonoBehaviour
    {
        public Transform placeToGetElement;

        private GameObject _placeToSlice1;
        private GameObject _placeToSlice2;
        private GameObject _placeToDelivery;
        private GameObject _placeToPutElement1;
        private GameObject _placeToPutElement2;
        private GameObject _countertops;


        private Transform _currentTarget;
        private InteractableController _interactableController;
        private IPickable _currentPickable;

        private Transform _slot;


        private bool onTheMove;
        private bool onTheRotate;

        [Header("Animation")]
        [SerializeField] private Animator animator;
        private readonly int _isCleaningHash = Animator.StringToHash("isCleaning");
        private readonly int _hasPickupHash = Animator.StringToHash("hasPickup");
        private readonly int _isChoppingHash = Animator.StringToHash("isChopping");
        private readonly int _velocityHash = Animator.StringToHash("velocity");

        [Header("Current Assistant")]
        private GameObject _currentAssistant;


        [Header("Moviment")]
        [SerializeField] private float movementSpeed = 1.1f;

        private float rotateStrength = 100.0f;
        private bool _shouldWalk = false;
        private Vector3 _inputDirection;

        [Header("CurrentStep")]
        public bool _isCompleted = false;
        public int _currentStep = 0;
        public RequestType _actionToMake;

        // Just to avoid using coroutine.
        float startTime = 0;
        float waitFor = 4;
        bool timerStart = false;
        public delegate void WhateverType(); // declare delegate type
        protected WhateverType callbackMethod; // to store the function

        public GameObject SlotForVegetable;


        public PathForActions(GameObject countertops, GameObject placeToSlice1, GameObject placeToSlice2, GameObject placeToDelivery, GameObject placeToPutElement1, GameObject placeToPutElement2, RequestType actionToMake)
        {
            _countertops = countertops;
            _placeToSlice1 = placeToSlice1;
            _placeToSlice2 = placeToSlice2;
            _placeToDelivery = placeToDelivery;
            _placeToPutElement1 = placeToPutElement1;
            _placeToPutElement2 = placeToPutElement2;
            _actionToMake = actionToMake;
        }

        public void SetCurrentAssistant(GameObject currentAssistant)
        {

            this._currentAssistant = currentAssistant;

            this.animator = _currentAssistant.GetComponentInChildren<Animator>();
            this._slot = _currentAssistant.GetComponent<AssistantController>().slot;
            this._interactableController = _currentAssistant.GetComponentInChildren<InteractableController>();
            this.movementSpeed = _currentAssistant.GetComponent<AssistantController>().model.movementSpeed;
        }

        public bool isFinished()
        {
            return _isCompleted;
        }

        private bool findCompletePlate()
        {

            var cmpts = _countertops.GetComponentInChildren<Plate>();
            SlotForVegetable = cmpts?.transform.gameObject;

            placeToGetElement = cmpts?.transform;

            if (placeToGetElement == null)
            {
                // cancel operation
                return false;
            }
            return true;
        }

        /* // Just for Troubleshooting
         public GameObject getSlotForVegetable(){
             return SlotForVegetable; 
         }*/

        private bool findAndSetVegetable(bool isTomato)
        {

            var cmpts = _countertops.GetComponentInChildren<Ingredient>();
            SlotForVegetable = cmpts?.transform.gameObject;

            //Debug.Log("cmpts: "+isTomato+ "  >>>"+cmpts);
            if (isTomato)
                placeToGetElement = cmpts?.transform;
            else
                placeToGetElement = cmpts?.transform;
            // onion

            if (placeToGetElement == null)
            {
                return false;
            }
            return true;
        }

        public bool isObjectAvailable()
        {
            var foundElement = false;
            if (_actionToMake == RequestType.DeliverOrder)
            {
                foundElement = this.findCompletePlate();
            }

            if (_actionToMake == RequestType.CutTomato)
            {
                foundElement = this.findAndSetVegetable(true);
            }

            if (_actionToMake == RequestType.CutOnion)
            {
                foundElement = this.findAndSetVegetable(false);
            }

            if (_actionToMake == RequestType.GetRandomElement)
            {
                foundElement = this.findAndSetVegetable(true);
            }

            return foundElement;
        }

        public void StartOperation(GameObject target)
        {
            _isCompleted = false;
            _currentStep = 0;
            //Debug.Log("1. StartOperation");
            //Debug.Log(target);

            var foundElement = isObjectAvailable();
            Debug.Log("[Path] Element found in Middle table. ");

            // TODO estou substintindo na gambiarra
            SlotForVegetable = target;
            placeToGetElement = target?.transform;

            if (foundElement)
            {
                this.NextStep();
            }
            else
            {
                Debug.Log("There's no vegetable/plate available.");
                this.setIsCompleted();
            }
        }

        private void NextStep()
        {

            _currentStep = _currentStep + 1;
            Debug.Log("[Path] Next step: " + _currentStep);
            // dashParticle.PlaySoundTransition(dashAudio);

            if (_actionToMake == RequestType.DeliverOrder)
            {
                this.NextStepForDelivery();
            }

            if (_actionToMake == RequestType.CutTomato)
            {
                this.NextStepForVegetable();
            }

            if (_actionToMake == RequestType.CutOnion)
            {
                this.NextStepForVegetable();
            }

            if (_actionToMake == RequestType.GetRandomElement)
            {
                this.NextStepForDebug();
            }

        }

        private void NextStepForDelivery()
        {
            if (_currentStep == 1)
                this.setTargetOnTable(placeToGetElement);

            if (_currentStep == 2)
                this.getElementFromSlot();

            if (_currentStep == 3)
                this.GoToDeliverCountertop();

            if (_currentStep == 4)
                this.PutElementOnTable();

            if (_currentStep == 5)
                this.setIsCompleted();
        }

        private void NextStepForDebug()
        {
            if (_currentStep == 1)
                this.setTargetOnTable(placeToGetElement);

            if (_currentStep == 2)
                this.getElementFromSlot();

            if (_currentStep == 3)
                this.GoToEmptyTable();

            if (_currentStep == 4)
                this.PutElementOnTable();

            if (_currentStep == 5)
                this.setIsCompleted();
        }

        private void NextStepForVegetable()
        {
            if (_currentStep == 1)
                this.setTargetOnTable(placeToGetElement);

            if (_currentStep == 2)
                this.getElementFromSlot();

            if (_currentStep == 3)
                this.GoToSlicingTable();

            if (_currentStep == 4)
                this.DoTheSlicing();

            if (_currentStep == 5)
                this.GetSlicedFood();

            if (_currentStep == 6)
                this.GoToEmptyTable();

            if (_currentStep == 7)
                this.PutElementOnTable();

            if (_currentStep == 8)
                this.setIsCompleted();
        }

        private void setIsCompleted()
        {
            _shouldWalk = false;
            _isCompleted = true;
            animator.SetFloat(_velocityHash, 0f);
            Debug.Log("[Path] Action was completed. (setIsCompleted called)");
        }

        private void setTargetOnTable(Transform placeToGo)
        {
            // go to there
            _currentTarget = placeToGo;
            _onMoviment();
        }

        private bool IsEmptyThisSlot(GameObject place)
        {
            //Debug.Log("Checking IsEmptyThisSlot: "+place);
            var interactable = place.GetComponentInChildren<Interactable>();
            //Debug.Log("IsEmptyThisSlot = interactable : "+interactable);
            interactable.CheckSlotOccupied();
            var _elementInteracable = interactable.CurrentPickable;
            //Debug.Log("IsEmptyThisSlot - : "+_elementInteracable);
            if (_elementInteracable == null)
            {
                return true;
            }
            return false;
        }


        private void PutElementOnTable()
        {
            Debug.Log("[Path] Put sliced food over the table.");
            var emptyTable = _currentTarget.gameObject;
            TutorialManager.wasCutted = true;
            this.HandlePickUp(emptyTable);
            this.NextStep();
        }

        private void GoToSlicingTable()
        {
            Debug.Log("[Path] Going to slicing table.");
            setTargetOnTable(_placeToSlice1.transform);
        }

        private void getElementFromSlot()
        {
            // >> slot >> countertop
            Debug.Log("[Path] Getting element from the Purple Countertop.");
            var counterTop = _currentTarget.parent.parent.gameObject;
            this.HandlePickUp(counterTop);
            this.NextStep();
        }

        private void DoTheSlicing()
        {
            Debug.Log("[Path] Slicing food.");
            _shouldWalk = false;
            var choppingBoard = _currentTarget.gameObject;
            this.HandlePickUp(choppingBoard);
            choppingBoard.GetComponent<Interactable>().Interact();
            this.WaitingFor(5, GoToNextStep);
        }

        private void GoToNextStep()
        {
            this.NextStep();
        }

        private void GetSlicedFood()
        {
            Debug.Log("[Path] Get sliced food.");
            var choppingBoard = _currentTarget.gameObject;
            this.HandlePickUp(choppingBoard);
            this.NextStep();
        }

        private void GoToDeliverCountertop()
        {
            Debug.Log("[Path] Going To deliver food on Countertop.");

            _currentTarget = _placeToDelivery.transform;
            _onMoviment();
        }

        public bool CheckIfBoardIsFull()
        {
            return !IsEmptyThisSlot(_placeToPutElement1) && !IsEmptyThisSlot(_placeToPutElement2);
        }

        private void GoToEmptyTable()
        {
            // walk to the place.
            _currentTarget = null;
            Debug.Log("[Path] Going to a empty output table.");

            if (IsEmptyThisSlot(_placeToPutElement1))
            {
                _currentTarget = _placeToPutElement1.transform;
                _onMoviment();
                return;
            }
            if (IsEmptyThisSlot(_placeToPutElement2))
            {
                _currentTarget = _placeToPutElement2.transform;
                _onMoviment();
                return;

            }

            if (_currentTarget == null)
            {
                Debug.Log("[Path] Mesas estão ocupadas. O que fazer? ");
                // TODO:  esperar uma mesa desocupar aqui. 
                //  StartCoroutine(RetryGoToEmptyTable());
                //   Invoke("RetryGoToEmptyTable", 2.0f);
                return;
            }
        }

        /*        IEnumerator RetryGoToEmptyTable()
                {
                    //Print the time of when the function is first called.
                    Debug.Log("Started GoToEmptyTable : " + Time.time);

                    //yield on a new YieldInstruction that waits for 2 seconds.
                    yield return new WaitForSeconds(2);

                    //After we have waited 5 seconds print the time again.
                    Debug.Log("Try again GoToEmptyTable : " + Time.time);
                }*/

        private void _onMoviment()
        {
            _shouldWalk = true;
            onTheRotate = true;
            onTheMove = true;
        }


        private void AnimatePlayerMovement()
        {
            _inputDirection = new Vector3(1f, 0f, 1f);
            animator.SetFloat(_velocityHash, _inputDirection.sqrMagnitude);
        }


        private void FixedUpdate()
        {
            AnimatePlayerMovement();
        }


        // This should be in AssistantController
        private void HandlePickUp(GameObject elementToHandle)
        {

            // Debug.Log("HandlePickUp: "+elementToHandle);

            // var interactable = _interactableController.CurrentInteractable;
            var interactable = elementToHandle.GetComponent<Interactable>();
            // empty hands, try to pick
            if (_currentPickable == null)
            {
                _currentPickable = interactable as IPickable;
                if (_currentPickable != null)
                {
                    animator.SetBool(_hasPickupHash, true);
                    //  _currentTarget.PlaySoundTransition(pickupAudio);
                    _currentPickable.Pick();
                    _interactableController.Remove(_currentPickable as Interactable);
                    _currentPickable.gameObject.transform.SetPositionAndRotation(_slot.transform.position,
                        Quaternion.identity);
                    _currentPickable.gameObject.transform.SetParent(_slot);
                    return;
                }

                // Interactable only (not a IPickable)
                _currentPickable = interactable?.TryToPickUpFromSlot(_currentPickable);
                if (_currentPickable != null)
                {
                    animator.SetBool(_hasPickupHash, true);
                    //   _currentTarget.PlaySoundTransition(pickupAudio);
                }

                _currentPickable?.gameObject.transform.SetPositionAndRotation(_slot.position, Quaternion.identity);
                _currentPickable?.gameObject.transform.SetParent(_slot);
                return;
            }

            // we carry a pickable, let's try to drop it (we may fail)

            // no interactable in range or at most a Pickable in range (we ignore it)
            if (interactable == null || interactable is IPickable)
            {
                animator.SetBool(_hasPickupHash, false);
                // _currentTarget.PlaySoundTransition(dropAudio);
                _currentPickable.Drop();
                _currentPickable = null;
                return;
            }

            // here we carry a pickable and we have an interactable in range
            // we may drop into the interactable

            // Try to drop on the interactable. It may refuse it, e.g. dropping a plate into the CuttingBoard,
            // or simply it already have something on it
            //    Debug.Log($"[PlayerController] {_currentPickable.gameObject.name} trying to drop into {interactable.gameObject.name} ");

            bool dropSuccess = interactable.TryToDropIntoSlot(_currentPickable);
            if (!dropSuccess) return;

            animator.SetBool(_hasPickupHash, false);
            //this.PlaySoundTransition(dropAudio);
            _currentPickable = null;
        }



        /**

        Just to avoid using coroutine.

        **/
        public void WaitingFor(int seconds, WhateverType cb)
        {
            timerStart = true;
            startTime = Time.time;
            waitFor = seconds;
            callbackMethod = cb;
        }

        public void UpdateForWaitMechanism()
        {
            if (timerStart && Time.time - startTime > waitFor)
            {
                //Do something
                callbackMethod();
                callbackMethod = null;
                timerStart = false;
            }
        }


        public void Update()
        {
            UpdateForWaitMechanism();

            if (_isCompleted) return;
            if (!_shouldWalk) return;

            this.FixedUpdate();

            if (onTheRotate)
                this.RotateTowardsTarget();

            if (onTheMove)
                this.MoveTowardsTarget();

            if (!onTheMove && !onTheRotate)
            {
                // CalculateInputDirection(transform.position);
                _shouldWalk = false;
                // Next Step only when not moving or not rotating and is not completed
                Debug.Log("[Path] Arrived at my target location.");
                this.NextStep();
            }
        }


        private void RotateTowardsTarget()
        {
            Quaternion targetRotation = Quaternion.LookRotation(_currentTarget.position - _currentAssistant.transform.position);
            _currentAssistant.transform.rotation = Quaternion.RotateTowards(_currentAssistant.transform.rotation, targetRotation, rotateStrength * Time.deltaTime);

            if (Quaternion.Angle(_currentAssistant.transform.rotation, targetRotation) < 0.001f)
            {
                //   Debug.Log("The Rotating Has Completed");
                _currentAssistant.transform.rotation = targetRotation;
                onTheRotate = false;
            }
        }



        private void MoveTowardsTarget()
        {
            _currentAssistant.transform.position = Vector3.MoveTowards(_currentAssistant.transform.position, _currentTarget.position, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(_currentAssistant.transform.position, _currentTarget.position) <= 1.0f)
            {
                Debug.Log("[PathForActions] The Moving Has Completed. Distance to target: " + Vector3.Distance(_currentAssistant.transform.position, _currentTarget.position).ToString() + "");
                //   _currentAssistant.transform.position = _currentTarget.position;
                onTheMove = false;
                animator.SetFloat(_velocityHash, 0f);
            }
        }



    }

}