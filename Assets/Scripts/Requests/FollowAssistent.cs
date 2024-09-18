using System.Collections;
using Undercooked.Model;
using UnityEngine;


namespace Undercooked.Requests
{

    public class FollowAssistent : MonoBehaviour
    {

        [Header("Current Assistant")]
        public Request _currentRequest;

        public RequestController requestController;
        public GameObject _currentAssistant;
        public Transform slot;
        [SerializeField] public bool _isIdle = true;

        [Header("Busy")]
        public AudioClip songWhenBusy;
        public AudioSource audioSourceWhenBusy;

        [Header("Dialogue")]
        public GameObject dialogueGUI;
        public AudioClip dialogOpenAudio;
        public AudioSource audioSource;
        public bool dialogueActive = false;

        [Header("Targets")]
        public GameObject placeToSlice1;
        public GameObject placeToSlice2;
        public GameObject placeToDelivery;
        public GameObject placeToPutElement1;
        public GameObject placeToPutElement2;
        [SerializeField] private GameObject tables;

        [Header("Data")]
        public FaceData faceData;
        public GameObject SlotForVegetable;
        public PathForActions currentUsedPath;


        private PathForActions cutTomato;
        private PathForActions cutOnion;
        private PathForActions delivery;
        private PathForActions randomElement;


        private Coroutine _BackReactionAndGoIdleCoroutine;


        void Start()
        {
            _currentAssistant = GetComponent<LoadCharacter>().myCurrentAssistant;


            // Possible Actions for Assistant
            cutTomato = new PathForActions(tables, placeToSlice1, placeToSlice2, placeToDelivery, placeToPutElement1, placeToPutElement2, RequestType.CutTomato);
            cutOnion = new PathForActions(tables, placeToSlice1, placeToSlice2, placeToDelivery, placeToPutElement1, placeToPutElement2, RequestType.CutOnion);
            delivery = new PathForActions(tables, placeToSlice1, placeToSlice2, placeToDelivery, placeToPutElement1, placeToPutElement2, RequestType.DeliverOrder);

            randomElement = new PathForActions(tables, placeToSlice1, placeToSlice2, placeToDelivery, placeToPutElement1, placeToPutElement2, RequestType.GetRandomElement);


            cutTomato.SetCurrentAssistant(_currentAssistant);
            cutOnion.SetCurrentAssistant(_currentAssistant);
            delivery.SetCurrentAssistant(_currentAssistant);
            randomElement.SetCurrentAssistant(_currentAssistant);

            currentUsedPath = randomElement;
        }

        public void newIdleState(bool state)
        {
            this._isIdle = state;
            this.requestController.setIsIdle(state);
            Debug.Log("== [Assistant] New Idleness state: " + state);
        }

        public void IAmBusy()
        {
            Debug.Log("== [Assistant] I am busy!!");
            if (this.songWhenBusy)
                this.audioSourceWhenBusy.PlayOneShot(this.songWhenBusy, 0.5F);

            ResponseType response = ResponseType.Busy;
            this.StartReaction(response);
        }


        public void BoringOperation()
        {
            // StopAllCoroutines();
            Debug.Log("== [Assistant] I'm Bored.");
            ResponseType response = ResponseType.Sleepy;
            this.StartReaction(response);
            this.newIdleState(true);

            StartCoroutine(this.EndBoringOperation());
        }

        public IEnumerator EndBoringOperation()
        {
            yield return new WaitForSeconds(2);

            this.BackToNormalFaceOperation();
        }

        public void StartOperation(Request currentAction, ResponseType response)
        {
            this.newIdleState(false);

            Debug.Log("== [Assistant] Starting Operation: " + currentAction._requestData.type.ToString());
            this.StartReaction(response);

            _currentRequest = currentAction;

            switch (_currentRequest._requestData.type)
            {
                case RequestType.CutTomato:
                    currentUsedPath = cutTomato;
                    break;
                case RequestType.CutOnion:
                    currentUsedPath = cutOnion;
                    break;
                case RequestType.DeliverOrder:
                    currentUsedPath = delivery;
                    break;
                case RequestType.GetRandomElement:
                    currentUsedPath = randomElement;
                    break;
                default:
                    currentUsedPath = cutTomato;
                    break;
            }

            currentUsedPath?.StartOperation(currentAction._currentPickable);
        }

        public void BackToNormalFaceOperation()
        {
            this._BackReactionAndGoIdleCoroutine = StartCoroutine(this.BackReactionAndGoIdle());
        }

        /** 
            -----------------------------
                Reactions
            -----------------------------
        */
        public IEnumerator BackReactionAndGoIdle()
        {
            //Debug.Log("Started Coroutine at timestamp : " + Time.time);
            //yield on a new YieldInstruction that waits for 1 second.
            yield return new WaitForSeconds(1);

            //After we have waited 1 second print the time again.
            //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
            this.ChangeAssistantFace(ResponseType.Normal);
            this.StopCleanCoroutine();
        }

        private void StopCleanCoroutine()
        {
            if (this._BackReactionAndGoIdleCoroutine != null)
            {
                StopCoroutine(this._BackReactionAndGoIdleCoroutine);
                this._BackReactionAndGoIdleCoroutine = null;
            }
        }

        public void StartReaction(ResponseType response)
        {
            Debug.Log("== [Assistant] Starting Reaction.");
            /// show emoji + dialog + change Face
            GameObject emojiBox = faceData.GetFace(response);
            emojiBox.SetActive(true);
            this.ChangeAssistantFace(response);

            // 2. Open Dialog
            StartCoroutine(this.StartDialog());

            // 2. close Dialog
            StartCoroutine(this.DisableDialog(emojiBox));
        }

        public void ChangeAssistantFace(ResponseType response)
        {
            var faces = _currentAssistant.GetComponentsInChildren<ChangeFace>()[0];

            faces.changeFace(response);
        }



        /** 
            -----------------------------
                StartDialog
            -----------------------------
        */
        public IEnumerator StartDialog()
        {
            yield return new WaitForSeconds(1);

            if (dialogOpenAudio) audioSource.PlayOneShot(dialogOpenAudio, 0.5F);

            dialogueGUI.SetActive(true);
            if (!dialogueActive)
            {
                dialogueActive = true;
            }
        }


        private IEnumerator DisableDialog(GameObject emojiToDisable)
        {
            yield return new WaitForSeconds(4);
            this.DropDialogue();
            emojiToDisable.SetActive(false);
        }


        public void DropDialogue()
        {
            dialogueGUI.SetActive(false);
            dialogueActive = false;
            // StopAllCoroutines();
            Debug.Log("== [Assistant] Closed Dialogue Box.");
        }

        /** 
            -----------------------------
                Updates
            -----------------------------
        */

        void Update()
        {
            // this.UpdateIfImIdle();

            this.UpdateDialogueBox();

            this.UpdateOperation();

        }
        /*
                private void UpdateIfImIdle()
                {
                    if (this._isIdle)
                    {
                        // this allows getting new actions
                    }
                }*/

        private void UpdateOperation()
        {

            if (!this._isIdle)
            {
                currentUsedPath?.Update();

                if (currentUsedPath.isFinished())
                {
                    // Finished operation.
                    if (this._BackReactionAndGoIdleCoroutine == null)
                    {
                        this.newIdleState(true);

                        this.BackToNormalFaceOperation();
                    }
                }
            }
        }

        private void UpdateDialogueBox()
        {
            // Dialogue box update
            Vector3 Pos = Camera.main.WorldToScreenPoint(_currentAssistant.transform.position);
            Pos.y += 220;
            dialogueGUI.transform.position = Pos;
        }



    }


}