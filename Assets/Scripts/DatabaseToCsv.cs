using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Undercooked.Requests;
using Undercooked;
using Undercooked.Managers;
using Undercooked.Model;

public class DatabaseToCsv : ScriptableObject
{
    private const int MaxNumberOfRequests = 10;
    [SerializeField] public int _level = -1;
    [SerializeField] public AssistentModel _assistant;

    [SerializeField] public int totalScore = 0;
    [SerializeField] public int numberOfDelivery = 0;

    [SerializeField] public int lastArousal = -1;
    [SerializeField] public int lastValence = -1;

    [SerializeField] private List<RequestMade> _requestsMade = new List<RequestMade>(MaxNumberOfRequests);
    [SerializeField] public float _posx = 0;
    [SerializeField] public float _posy = 0;

    [SerializeField] public string _timestamp = "";

    [SerializeField] public string _basepath = "";

    [SerializeField] public Vector3 lastPosition;

    [SerializeField] public FileManager fileRecord;
    [SerializeField] public FileManager filePanas;
    //[SerializeField] public FileManager fileSam;

    public static DatabaseToCsv _instance;

    private void Awake()
    {
        // string fil1 = @"C:\Users\mariu\
        this._timestamp = this.GetTimestamp(DateTime.Now);
        this._basepath = @Application.dataPath + "/results/" + this._timestamp + "";
        this.fileRecord = new FileManager(this._basepath + "_levels.csv");
        this.filePanas = new FileManager(this._basepath + "_panas.csv");
        // this.fileSam = new FileManager(this._basepath + "_sam.csv");

        this._assistant = CreateInstance<AssistentModel>();
    }


    public static DatabaseToCsv GetInstance()
    {
        if (!_instance)
        {
            // NB: FindObjectOfType is used to retrieve the instance
            // when play mode recompiles
            _instance = FindObjectOfType<DatabaseToCsv>();
        }
        if (!_instance)
        {
            // NB: create the Singleton, and initialise its values
            _instance = CreateInstance<DatabaseToCsv>();
            _instance.Start();
        }
        return _instance;
    }


    public void Start()
    {
        this.CleanLevel();
        OrderManager.OnOrderDelivered += HandleOrderDelivered;
        Debug.Log("[DatabaseToCsv] Starting storage.");
    }

    public void CleanLevel()
    {
        Debug.Log("[DatabaseToCsv] Cleaning Level.");
        this.totalScore = 0;
        this.numberOfDelivery = 0;
        this._requestsMade.Clear();
    }

    public String GetTimestamp(DateTime value)
    {
        return value.ToString("dd-MM-yyyy_HH-mm");
    }

    public string getLevel()
    {
        return this._level.ToString() + ";";
    }


    public string getHeaderLevel()
    {
        return "level;";
    }

    public string getHeaderDelivery()
    {
        return "totalScore;numberOfDelivery;";
    }

    public string getHeaderAssistantModel()
    {
        return "nickname;movement Speed;probality To Sleep;games Available;personality;";
    }

    public string getHeaderRequests()
    {
        return "timestamp Start;timestamp End;action Requested;faceShown;Was Realized;";
    }

    public string getHeaderPanas()
    {
        return "Question 0; Answer 0; Question 1; Answer 1; Question 2; Answer 2; Question 3; Answer 3; Question 4; Answer 4;";
    }

    public string getHeaderSam()
    {
        return "Arousal; Valence;";
    }

    public string getHeaderEmotion()
    {
        return "EmojiGrid After Round(X);EmojiGrid After Round(Y);";
    }

    /*
    *
    *
    */
    public string getEmotion()
    {
        return this._posx.ToString("0") + ";" + this._posy.ToString("0") + ";";
    }

    public string getSam()
    {
        return this.lastArousal.ToString() + ";" + this.lastValence.ToString() + ";";
    }

    public string getDelivery()
    {
        return this.totalScore.ToString() + ";" + this.numberOfDelivery.ToString() + ";";
    }


    public string getAssistantModel()
    {
        if (this._assistant != null)
        {

            Debug.Log("[getAssistantModel] @getAssistantModel " + this._assistant.getMyData());
            return this._assistant.Nickname + ";" +
             this._assistant.movementSpeed + ";" +
             (this._assistant.probalityToSleep * 100).ToString() + ";" +
             this._assistant.getGameAvailable() + ";" +
             this._assistant.personality.ToString() + ";";
        }
        return ";;;;;";
    }

    public void writeHeaderPanas(int baseIndex)
    {
        var textHeader = this.getHeaderPanas();
        this.filePanas.writeLine(textHeader);
    }

    public void writePanas(CurrentAnswer[] currentAnswers, int baseIndex)
    {
        Debug.Log("[DatabaseToCsv] Write 5 PANAS answers. ");
        var textQuestion = "";


        for (int i = 0; i < currentAnswers.Length; i++)
        {
            textQuestion += currentAnswers[i].question + ";" + currentAnswers[i].answerIndex + ";";
        }
        this.filePanas.writeLine(textQuestion);
    }

    public void writeFooterPanas()
    {
        this.filePanas.writeLine(";;;;;;;;;;");
    }

    // Writers

    public void writeLines()
    {
        this.totalScore = GameManager.Score;
        Debug.Log("[DatabaseToCsv] ---- Final Score: " + GameManager.Score);

        string prefix = this.getLevel() + this.getAssistantModel() + this.getDelivery() + this.getEmotion() + this.getSam();


        // There wasn't any request delivered.
        if (this._requestsMade.Count == 0)
        {
            this.fileRecord.writeLine(prefix);
        }
        else
        {
            TextWriter w = this.fileRecord.getWriter();
            // one line for each request realized
            for (int i = 0; i < this._requestsMade.Count; i++)
            {
                RequestMade requestmd = this._requestsMade[i];

                var wasRealized = "yes";
                if (requestmd._faceShown.ToString() == "Sleepy")
                    wasRealized = "no";

                string line = requestmd._timestampStart + ";" +
                requestmd._timestampEnd + ";" +
                requestmd._actionRealized.ToString() + ";" +
                requestmd._faceShown.ToString() + ";" +
                wasRealized + ";";

                w.WriteLine(prefix + line);
            }
            w.Close();
        }

    }


    public void writeHeader()
    {
        string header = this.getHeaderLevel() + this.getHeaderAssistantModel() +
        this.getHeaderDelivery() + this.getHeaderEmotion() + this.getHeaderSam() + this.getHeaderRequests();
        this.fileRecord.writeLine(header);
    }

    public void writeCSV()
    {
        this.writeHeader();
        this.writeLines();
    }

    // Handlers

    public void HandleOrderDelivered(Order order, int tipCalculated)
    {
        Debug.Log("[DatabaseToCsv] HandleOrderDelivered > Current Score: " + GameManager.Score);
        this.numberOfDelivery += 1;

        Debug.Log("[DatabaseToCsv] HandleOrderDelivered > " + this._assistant.Nickname + ";" +
            this._assistant.movementSpeed + ";" +
            (this._assistant.probalityToSleep * 100).ToString() + ";" +
            this._assistant.getGameAvailable() + ";" +
            this._assistant.personality.ToString() + ";");


    }

    // Setters

    public void setEmotionFeedback()
    {
        this._posx = this.calculateX_EmojiGrid(this.lastPosition.x) - 50;
        this._posy = this.calculateY_EmojiGrid(this.lastPosition.y) - 50;
        Debug.Log("[DatabaseToCsv] Storing Emotion Feedback: X(" + this._posx + "), Y(" + this._posy + ")");

    }

    public void setCurrentLevel(int level)
    {
        this._level = level;
    }

    public void setAssistant(AssistentModel assistant)
    {
        Debug.Log("setAssistant");
        this._assistant = assistant;
        this.getAssistantModel();
        Debug.Log("[setAssistant] @getAssistantModel " + this._assistant.getMyData());
    }

    public void setRequestMade(RequestMade lastOperation)
    {
        this._requestsMade.Add(lastOperation);
    }



    public int calculateX_EmojiGrid(float valX)
    {
        float minX = 500;
        float maxX = 1050;

        float correctX = ((valX - minX) / (maxX - minX)) * 100;
        Debug.Log("[calculateXemojiGrid] Calculating Fix_X(" + correctX + ") Original_X(" + valX + ")");
        return Convert.ToInt32(correctX);
    }

    public int calculateY_EmojiGrid(float valY)
    {
        float minY = 180;
        float maxY = 720;

        float correctY = ((valY - minY) / (maxY - minY)) * 100;
        Debug.Log("[calculateYemojiGrid] Calculating Fix_Y(" + correctY + ") Original_Y(" + valY + ")");

        return Convert.ToInt32(correctY);
    }


    public void setLastEmojiGrid(Vector3 lastEmojiGridPos)
    {
        //Debug.Log("[MenuPanel] Last Emotion Feedback: X("+lastEmojiGridPos.x+"), Y("+lastEmojiGridPos.y+")");


        this.lastPosition = lastEmojiGridPos;
    }


    public void setSam(int arousal, int valence)
    {
        Debug.Log("[DatabaseToCsv] Last SAM: arousal(" + arousal + "), valence(" + valence + ")");
        this.lastArousal = arousal;
        this.lastValence = valence;
    }

}