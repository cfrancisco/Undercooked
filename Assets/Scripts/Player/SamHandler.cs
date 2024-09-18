using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System;
using System.Collections.Generic;
using Undercooked.UI;

namespace Undercooked
{
    public class SamHandler : MonoBehaviour
    {
        [Header("Mood")]
        [SerializeField] private GameObject _joystickValenceMood;
        [SerializeField] private GameObject _joystickArousalMood;

        [SerializeField] private Rect boundsValence;
        [SerializeField] private Rect boundsArousal;

        [SerializeField] private int valenceValue = -1;
        [SerializeField] private int arousalValue = -1;

        void Awake()
        {
            Assert.IsNotNull(_joystickValenceMood);
            Assert.IsNotNull(_joystickArousalMood);
        }

        private void Start()
        {
            Debug.Log("[SamHandler] Open SAM Painel.");
            Time.timeScale = 1;
            //  Rect bounds = new Rect(0, 0, Screen.width/2, Screen.height);

        }

        public void SendDataToDatabase()
        {
            DatabaseToCsv.GetInstance().setSam(this.arousalValue, this.valenceValue);
            MenuPanelUI.SendEmojigridToDatabase();
        }



        public int calculateX_Sam(float rawValue)
        {
            float minY = 500;
            float maxY = 1400;
            float fixedValue = ((rawValue - minY) / (maxY - minY)) * 100;
            fixedValue = fixedValue / 10; // 10 spcaes
            if (fixedValue < 1) fixedValue = 1;
            if (fixedValue > 9) fixedValue = 9;
            return Convert.ToInt32(fixedValue);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetMouseButton(0) && boundsArousal.Contains(Input.mousePosition))
            {
                Vector3 mousePos = Input.mousePosition;
                this._joystickArousalMood.transform.SetPositionAndRotation(mousePos, Quaternion.identity);

                this.arousalValue = calculateX_Sam(mousePos.x);
                Debug.Log("[DatabaseToCsv] Arousal ->>>> (" + mousePos.x + "): " + this.arousalValue);
                // Debug.Log("1 The left mouse button is being held down.");
            }



            if (Input.GetMouseButton(0) && boundsValence.Contains(Input.mousePosition))
            {
                Vector3 mousePos = Input.mousePosition;
                this._joystickValenceMood.transform.SetPositionAndRotation(mousePos, Quaternion.identity);
                Debug.Log("[DatabaseToCsv] Valence ->>>> (" + mousePos.x + ")");

                this.valenceValue = calculateX_Sam(mousePos.x);
            }

        }
    }
}
