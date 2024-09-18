using UnityEngine;
using System;

namespace Undercooked
{
    public class LogToFile : MonoBehaviour
    {
        [SerializeField] public string _timestamp = "";
        [SerializeField] public FileManager fileHandler;

        [SerializeField] public string _basename = "";
        private void Awake()
        {
            this._timestamp = this.GetTimestamp(DateTime.Now);

            this._basename = Application.dataPath + "/results/" + this._timestamp + "";

            fileHandler = new FileManager(this._basename + "_logFile.txt");

            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }
        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public String GetTimestamp(DateTime value)
        {
            return value.ToString("dd-MM-yyyy_HH-mm");
        }


        // Start is called before the first frame update
        void Start()
        {
            // fileHandler.cleanFile();
        }

        public void Log(string logString, string stackTrace, LogType logType)
        {
            fileHandler.writeLine(logString);
        }
    }
}
