using System.Collections;
using Lean.Transition;
using Undercooked.Appliances;
using Undercooked.Model;
using Undercooked;
using UnityEngine;
using UnityEngine.InputSystem;
using Undercooked.Player;

public class AssistantController : MonoBehaviour
{
    [Header("Assistant Info")]
    [SerializeField] public AssistentModel model;

    [Header("Physics")]
    [SerializeField] public Rigidbody playerRigidbody;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    private readonly int _isCleaningHash = Animator.StringToHash("isCleaning");
    private readonly int _hasPickupHash = Animator.StringToHash("hasPickup");
    private readonly int _isChoppingHash = Animator.StringToHash("isChopping");
    private readonly int _velocityHash = Animator.StringToHash("velocity");

    private bool _isActive = true;
    private IPickable _currentPickable;
    
    [SerializeField] public Transform slot;
    [SerializeField] private ParticleSystem dashParticle;
    [SerializeField] private Transform knife;

    private DialogueSystem dialogueSystem;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupAudio;
    [SerializeField] private AudioClip dropAudio;

    
    private void Awake()
    {
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        //    selector.GetComponent<MeshRenderer>().material.color = color;
    }

    private void SubscribeInteractableEvents()
    {
        ChoppingBoard.OnChoppingStart += HandleChoppingStart;
        ChoppingBoard.OnChoppingStop += HandleChoppingStop;
        // _pickUpAction.performed += HandlePickUp;
        Sink.OnCleanStart += HandleCleanStart;
        Sink.OnCleanStop += HandleCleanStop;
    }

    private void UnsubscribeInteractableEvents()
    {
        ChoppingBoard.OnChoppingStart -= HandleChoppingStart;
        ChoppingBoard.OnChoppingStop -= HandleChoppingStop;
        Sink.OnCleanStart -= HandleCleanStart;
        Sink.OnCleanStop -= HandleCleanStop;
        //_pickUpAction.performed -= HandlePickUp;
    }

    private void HandleCleanStart(PlayerController playerController)
    {
        animator.SetBool(_isCleaningHash, true);
    }

    private void HandleCleanStop(PlayerController playerController)
    {
        animator.SetBool(_isCleaningHash, false);
    }

    private void HandleChoppingStart(PlayerController playerController)
    {
        animator.SetBool(_isChoppingHash, true);
        knife.gameObject.SetActive(true);
    }

    private void HandleChoppingStop(PlayerController playerController)
    {
        animator.SetBool(_isChoppingHash, false);
        knife.gameObject.SetActive(false);
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        // _interactableController.CurrentInteractable?.Interact(this);
    }

    
}
