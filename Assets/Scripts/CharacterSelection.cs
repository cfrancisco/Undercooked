using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class CharacterSelection : MonoBehaviour
{
	public GameObject[] characters;
	public int selectedCharacter = 0;
    public TextMeshProUGUI textAssistentName;
    public AudioSource songWhenClick;


    private void Awake()
    {
      //  textAssistentName = textAssistentName.GetComponent<TextMeshProUGUI>();
    characters[selectedCharacter].SetActive(true);
     var assistentData = characters[selectedCharacter].GetComponent<AssistentModel>();
    textAssistentName.text = assistentData.Nickname;
    }

	public void NextCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter = (selectedCharacter + 1) % characters.Length;
		characters[selectedCharacter].SetActive(true);
        var assistentData = characters[selectedCharacter].GetComponent<AssistentModel>();
        textAssistentName.text = assistentData.Nickname;
         songWhenClick.Play();

	}

	public void PreviousCharacter()
	{
		characters[selectedCharacter].SetActive(false);
		selectedCharacter--;
		if (selectedCharacter < 0)
		{
			selectedCharacter += characters.Length;
		}
		characters[selectedCharacter].SetActive(true);
        var assistentData = characters[selectedCharacter].GetComponent<AssistentModel>();
        textAssistentName.text = assistentData.Nickname;
          songWhenClick.Play();
	}

	public void StartGame()
	{
		PlayerPrefs.SetInt("selectedCharacter", selectedCharacter);
		SceneManager.LoadScene("Level1", LoadSceneMode.Single);
	}
}
