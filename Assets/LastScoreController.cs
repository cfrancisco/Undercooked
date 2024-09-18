using TMPro;
using Undercooked.Data;
using Undercooked.Managers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Undercooked
{
    public class LastScoreController : MonoBehaviour
    {
        [SerializeField] public string[] scores = new string[11];
        [SerializeField] public GameObject endingPanel;

        public TextMeshProUGUI lastScoreN1text;
        public TextMeshProUGUI lastScoreN2text;
        public TextMeshProUGUI allScore1Text;
        public TextMeshProUGUI allScore2Text;

        private void Awake()
        {
#if UNITY_EDITOR
            Assert.IsNotNull(this.endingPanel);
#endif
        }
        // Start is called before the first frame update
        void Start()
        {
            this.endingPanel.SetActive(false);
            this.ShowLastScores();
        }

        void ShowLastScores()
        {
            Debug.Log("[LastScoreController] ShowLastScores.");
            if (LevelManager.currentLevelIndex > 10)
            {
                this.GetTop10Scores();
            }

            if (LevelManager.currentLevelIndex > 1)
            {
                LevelData levelN1 = LevelManager.GetInstance().getLevelN(LevelManager.currentLevelIndex - 1);
                LevelData levelN2 = LevelManager.GetInstance().getLevelN(LevelManager.currentLevelIndex - 2);
                lastScoreN1text.text = levelN1.lastScore.ToString();
                lastScoreN2text.text = levelN2.lastScore.ToString();
            }
            if (LevelManager.currentLevelIndex == 1)
            {

                LevelData levelN1 = LevelManager.GetInstance().getLevelN(LevelManager.currentLevelIndex - 1);
                lastScoreN1text.text = levelN1.lastScore.ToString();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        void GetTop10Scores()
        {
            Debug.Log("[LastScoreController] Get Top10 Scores.");
            allScore1Text.text = "Fase       Score<br>";
            allScore2Text.text = "Fase       Score<br>";
            this.endingPanel.SetActive(true);
            for (var i = 1; i < 6; i++)
            {
                this.scores[i] = LevelManager.GetInstance().getLevelN(i).lastScore.ToString();
                allScore1Text.text = allScore1Text.text + " 0" + i + "         " + this.scores[i] + "<br>";
            }
            for (var i = 6; i < 10; i++)
            {
                this.scores[i] = LevelManager.GetInstance().getLevelN(i).lastScore.ToString();
                allScore2Text.text = allScore2Text.text + " 0" + i + "         " + this.scores[i] + "<br>";
            }
            // 10 is a particular case 
            this.scores[10] = LevelManager.GetInstance().getLevelN(10).lastScore.ToString();
            allScore2Text.text = allScore2Text.text + " " + 10 + "         " + this.scores[10] + "<br>";

        }
    }
}
