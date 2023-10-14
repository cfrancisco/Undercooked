using Undercooked.Model;
using UnityEngine;
using Undercooked; 

namespace Undercooked 
{
public class AssistentModel : MonoBehaviour
{
    
    public string Nickname; 

    public PersonalityType personality;
    
    public float movementSpeed = 1f;

    public float probalityToSleep = 0.3f;

    public int AssistentIndex; 

    [SerializeField] private int _gamesAvailable;
  
    public int getGameAvailable()
    {
       // Debug.Log("getGameAvailable: "+ MainDatabase.LifeManager.GetInstance().GetLives(this.AssistentIndex).ToString());
        return MainDatabase.LifeManager.GetInstance().GetLives(this.AssistentIndex); 
    }

    public void reduceOneGame(){
       if (this.getGameAvailable() > 0)
       {
            MainDatabase.LifeManager.GetInstance().reduceOneGame(this.AssistentIndex);
       }
    } 

    public bool nextShouldHelp(){
        int randomIndex = Random.Range(0, 100);
            Debug.Log("% to Sleep: "+this.probalityToSleep);


        if (randomIndex > this.probalityToSleep*100) return true;
        return false; 
    }
        
    public ResponseType getFaceBasedPersona()
    { 
        ResponseType response;

        switch (personality)
        {
            case PersonalityType.Good:
                response = ResponseType.Happy;
                break;
            case PersonalityType.Neutral:
                response = ResponseType.Confused;
                break;
            case PersonalityType.Bad:
                response = ResponseType.Angry;
                break;
            default:
                response = ResponseType.Angry;
                break;
        }
        return response; 
    }
        

}
}