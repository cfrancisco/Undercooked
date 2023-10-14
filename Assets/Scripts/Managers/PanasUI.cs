using UnityEngine.UI;
using UnityEngine;
using TMPro;
using Undercooked.Managers; 

namespace Undercooked
{ 
        public class CurrentAnswer {
            public string question;
            public int answerIndex; 
        }


    public class PanasUI : MonoBehaviour
    {
        [SerializeField]   public GameObject[] questionGroupArr  = new GameObject[5];
        [SerializeField]  public GameObject nextPanel;
        [SerializeField]  public GameObject currentPanel;
        [SerializeField]  public CurrentAnswer[] currentAnswers  = new CurrentAnswer[5];
      
        // Start is called before the first frame update
        void Start()
        {
        }
        public void GetCurrentAnswers(){
            for (int i = 0;i<questionGroupArr.Length;i++){
               this.currentAnswers[i] = this.GetQuestion(this.questionGroupArr[i]);
            }
        }

        public CurrentAnswer GetQuestion(GameObject qGroup){

            CurrentAnswer resp = new CurrentAnswer();
            GameObject question = qGroup.transform.Find("Question").gameObject; 
            GameObject answers = qGroup.transform.Find("Answers").gameObject; 
            resp.question = question.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            for (int i = 0;i < answers.transform.childCount;i++){
                if (answers.transform.GetChild(i).GetComponent<Toggle>().isOn){
                    resp.answerIndex = i;
                    break;

                }
            }
           // DebugUndercooked.DumpToConsole("resp",resp);

            return resp; 
        }

        public void NextPage(){
            // painel obrigado por responder
            if (this.nextPanel == null)
            {
                LevelManager.GetInstance().LoadMenuScene();
            }

            this.GetCurrentAnswers(); 

            DatabaseToCsv.GetInstance().writePanas(this.currentAnswers);

            this.currentPanel.SetActive(false);
            this.nextPanel.SetActive(true);
        }
    }
}
