using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Undercooked; 
using Undercooked.Managers;
using TMPro;


public class CharacterSelection : MonoBehaviour
{

	[Header("Characters")]
	public GameObject[] characters;
	public int selectedCharacter = 0;
    public TextMeshProUGUI textAssistentName;
    public TextMeshProUGUI textGameAvailables;
    public AudioSource songWhenClick;


    [Header("Input")]
	[SerializeField] private PlayerInput playerInput;
	private InputAction _moveAction;
	private InputAction _startAtPlayerAction;

    private void Awake()
    {
		_moveAction = playerInput.currentActionMap["Move"];
		_startAtPlayerAction = playerInput.currentActionMap["Start@Player"];

		this.showAssistantData();
		
		_moveAction.performed += HandleMoveChar;
		_startAtPlayerAction.performed += HandleEnterChar;

    }

	private void showAssistantData(){
		characters[selectedCharacter].SetActive(true);

		var assistentData = this.characters[selectedCharacter].GetComponent<AssistantController>().model;
		textAssistentName.text = assistentData.Nickname;
 		Debug.Log("[Select Assistant] Showing Nickname: "+assistentData.Nickname+ "  with Games available: "+assistentData.getGameAvailable());


		if (assistentData.getGameAvailable() == 0)
		{
			textGameAvailables.text = "Sem jogos disponiveis";
		}
		else
		if (assistentData.getGameAvailable() == 1)
		{
			textGameAvailables.text = assistentData.getGameAvailable().ToString()+" jogo disponivel";
		}
		else{
			textGameAvailables.text = assistentData.getGameAvailable().ToString()+" jogos disponiveis";
		}
	}

	private void HandleEnterChar(InputAction.CallbackContext context)
	{
		var pressedButton = ((KeyControl)context.control).keyCode.ToString();

		if  (pressedButton == "Space")
		{
			StartGame();
		}

		if  (pressedButton == "Enter")
		{
			StartGame();
		}

	}

	private void HandleMoveChar(InputAction.CallbackContext context)
	{
		var pressedButton = ((KeyControl)context.control).keyCode.ToString();
		if  (pressedButton == "A" || pressedButton == "LeftArrow")
		{
			PreviousCharacter();
		}
		if  (pressedButton == "D" || pressedButton == "RightArrow")
		{
			NextCharacter();
		}
	}


	public void NextCharacter()
	{
		Debug.Log("[Select Assistant] Next Character Button was clicked.");
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
	   
	    this.showAssistantData();

		songWhenClick.Play();

	}

	public void PreviousCharacter()
	{
		Debug.Log("[Select Assistant] Previous Character Button was clicked.");
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{ 
			selectedCharacter += characters.Length;
		}
		
        this.showAssistantData();

		songWhenClick.Play();
	}

	public void StartGame()
	{
		Debug.Log("[Select Assistant] Start Game Button was clicked.");
	
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);	
		int gamesAvailable = characters[selectedCharacter].GetComponent<AssistantController>().model.getGameAvailable(); 


		if (gamesAvailable == 0)
		{
			Debug.Log("[Select Assistant] Isn't possible to select this character.");
		} 
		else
		{
			// if is not in tutorial phase
          if (LevelManager.currentLevelIndex == 0)
			{
			Debug.Log("[Select Assistant] Tutorial phase doesn't reduce one live.");

			}
			else
			{
				this.characters[selectedCharacter].GetComponent<AssistantController>().model.reduceOneGame();
			}

			_moveAction.performed -= HandleMoveChar;
			_startAtPlayerAction.performed -= HandleEnterChar;

			DatabaseToCsv.GetInstance().setAssistant(this.characters[selectedCharacter].GetComponent<AssistantController>().model);
	
			LevelManager.GetInstance().LoadGameScene();
		}
	}
	public void HandleGoToMenu()
	{
		Debug.Log("[Clicked] Go to Menu Button was clicked.");
		LevelManager.GetInstance().LoadMenuScene();
	}

}
