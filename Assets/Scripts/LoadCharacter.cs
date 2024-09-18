using UnityEngine;
using Undercooked;
using TMPro;

public class LoadCharacter : MonoBehaviour
{
	public GameObject[] characterPrefabs;
	public Transform spawnPoint;
	public TMP_Text label;
	public GameObject myCurrentAssistant; 

	void Awake()
	{
		int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
		//GameObject prefab = characterPrefabs[selectedCharacter];
	    //GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        characterPrefabs[selectedCharacter].SetActive(true);
        myCurrentAssistant = characterPrefabs[selectedCharacter];
        label.text = myCurrentAssistant.GetComponent<AssistantController>().model.Nickname;
	}
}
