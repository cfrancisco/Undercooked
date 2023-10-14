using UnityEngine;
using UnityEngine.Assertions;


namespace Undercooked
{
    public class EmojiGridHandler : MonoBehaviour
    {
        [Header("Mood")]
        [SerializeField] private  GameObject _joystickMood;
        [SerializeField] private  GameObject _messageMood1;
        [SerializeField] private  GameObject _messageMood2;
        [SerializeField] private  Rect bounds; 
        void Awake()
        {
            Assert.IsNotNull(_joystickMood);
            Assert.IsNotNull(_messageMood1);
            Assert.IsNotNull(_messageMood2);

        }
        // Start is called before the first frame update
     
       private void Start()
        {
            Debug.Log("[EmojiGridHandler] Start");
            Time.timeScale = 1;
          //  Rect bounds = new Rect(0, 0, Screen.width/2, Screen.height);

        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButton(0) && bounds.Contains(Input.mousePosition))
            {
                Vector3 mousePos = Input.mousePosition;
                this._joystickMood.transform.SetPositionAndRotation(mousePos, Quaternion.identity);
                // Debug.Log("1 The left mouse button is being held down.");
            }
        }
    }
}
