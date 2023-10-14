using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Undercooked.Managers;

public class TutorialHandler : MonoBehaviour
{
    [Header("Tutorial")]
	public GameObject[] pages;
	public int selectedPage = 0;
    public AudioSource songWhenClick;

    [Header("Input")]
	[SerializeField] private PlayerInput playerInput;
	private InputAction _moveAction;
	private InputAction _startAtPlayerAction;

    private void Awake()
    {
		_moveAction = playerInput.currentActionMap["Move"];
		_startAtPlayerAction = playerInput.currentActionMap["Move"];

		_moveAction.performed += HandleMoveTutorial;
		_startAtPlayerAction.performed += HandleEnterTutorial;

    }

	private void HandleEnterTutorial(InputAction.CallbackContext context)
	{
		var pressedButton = ((KeyControl)context.control).keyCode.ToString();

		if  (pressedButton == "Enter")
		{
			this.NextPage();
		}

		if  (pressedButton == "Space")
		{
			this.NextPage();
		}

	}

	private void HandleMoveTutorial(InputAction.CallbackContext context)
	{
	
		var pressedButton = ((KeyControl)context.control).keyCode.ToString();

 		//if (Input.GetKeyDown(KeyCode.A))
		if  (pressedButton == "A")
		{
			this.PreviousPage();
		}
		if  (pressedButton == "D")
		{
			this.NextPage();
		}
	}


	public void NextPage()
	{

		songWhenClick.Play();

        if  (selectedPage + 1 == pages.Length)
        {
            this.GoToCharacterSelection();
        }
		else
		{

			pages[selectedPage].SetActive(false);
			selectedPage = (selectedPage + 1) % pages.Length;
			pages[selectedPage].SetActive(true);

		}
	}

	public void PreviousPage()
	{
		pages[selectedPage].SetActive(false);
		selectedPage--;
		if (selectedPage < 0)
		{
			selectedPage = 0;
		}
		pages[selectedPage].SetActive(true);

		songWhenClick.Play();
	}

	public void GoToCharacterSelection()
	{
		_moveAction.performed -= HandleMoveTutorial;
		_startAtPlayerAction.performed -= HandleMoveTutorial;
		LevelManager.GetInstance().LoadAssistantScene();
	}
}
