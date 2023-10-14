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
    [SerializeField] public string _filename = "";
    [SerializeField] public AssistentModel _assistant; 
    [SerializeField] public int totalScore = 0;
    [SerializeField] public int numberOfDelivery = 0;
    [SerializeField] private List<RequestMade> _requestsMade = new List<RequestMade>(MaxNumberOfRequests);
    [SerializeField] public float _posx = 0;
    [SerializeField] public float _posy = 0;

   public static DatabaseToCsv _instance;

    private void Awake()
    {
        Debug.Log("[DatabaseToCsv] Awake.");
        string timeStamp = this.GetTimestamp(DateTime.Now);
        this._filename = Application.persistentDataPath + "/res" + timeStamp + ".csv";
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
        this.totalScore = 0;
        this.numberOfDelivery = 0; 
        this._requestsMade.Clear();
        OrderManager.OnOrderDelivered += HandleOrderDelivered;
    }

    public String GetTimestamp(DateTime value)
    {
        return value.ToString("dd-MM-yyyy_HH-mm");
    }

    public string getLevel()
    {
        return this._level.ToString()+";";
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
        return "nickname;movementSpeed;probalityToSleep;gamesAvailable;personality;";
    }

    public string getHeaderRequests()
    {
        return "timestampStart;timestampEnd;actionRealized;faceShown;";
    }

    public string getHeaderEmotion()
    {
        return "positionX;positionY;";
    }

    public string getEmotion()
    {
        return this._posx.ToString("0")+";"+this._posy.ToString("0")+";";
    }


    public string getDelivery()
    {
        return this.totalScore.ToString()+";"+this.numberOfDelivery.ToString()+";";
    }


    public string getAssistantModel()
    {
        if (this._assistant)
        {
            return this._assistant.Nickname+";"+
            this._assistant.movementSpeed+";"+
            this._assistant.probalityToSleep+";"+
            this._assistant.getGameAvailable()+";"+
            this._assistant.personality.ToString()+";";
        }
        return ";;;;;";
    }


    public void writePanas(CurrentAnswer[] currentAnswers)
    {
        var textHeader = "";
        var textQuestion = "";
        string timeStamp = this.GetTimestamp(DateTime.Now);
       
        var filenamePANAS = Application.persistentDataPath + "/res" + timeStamp + ".csv";

        TextWriter w = new StreamWriter(filenamePANAS, true);

        for (int i = 0; i < currentAnswers.Length; i++)
        {
            textHeader +=" Question "+i+"; Answer "+i+";";
            textQuestion += currentAnswers[i].question+";"+ currentAnswers[i].answerIndex+";";
        }
    
        w.WriteLine(textHeader);
        w.WriteLine(textQuestion);
        w.Close();
    }

     // Writers
    
    public void writeLines()
    {
        string prefix = this.getLevel()+this.getAssistantModel()+this.getDelivery()+this.getEmotion(); 

        TextWriter w = new StreamWriter(this._filename, true);
        Debug.Log("_filename: "+this._filename);

        // There wasn't any request delivered.
        if (this._requestsMade.Count == 0 )
        {
            w.WriteLine(prefix);
        }
        else
        {
            // one line for each request realized
            for (int i = 0; i < this._requestsMade.Count; i++)
            {
                RequestMade requestmd = this._requestsMade[i];
                string line = requestmd._timestampStart+";"+
                requestmd._timestampEnd+";"+
                requestmd._actionRealized.ToString()+";"+
                requestmd._faceShown.ToString()+";";
                w.WriteLine(prefix+line);
            }
        }

        w.Close();
    }

    
    public void writeHeader(){
        TextWriter w = new StreamWriter(_filename,false);
        string header = this.getHeaderLevel()+this.getHeaderAssistantModel()+
        this.getHeaderDelivery()+this.getHeaderEmotion()+this.getHeaderRequests(); 
        w.WriteLine(header);
        w.Close();
    }

    public void writeCSV(){

        this.setCurrentLevel(LevelManager.currentLevelIndex);

        this.writeHeader();

        this.writeLines();

        // TODO: open the just created file
    }

 

    // Handlers

    public void HandleOrderDelivered(Order order, int tipCalculated){
        this.totalScore = GameManager.Score;
        this.numberOfDelivery += 1;  
    }

    // Setters

    public void setEmotionFeedback(float x, float y)
    {
       this._posx = x;
       this._posy = y;
    }

    public void setCurrentLevel(int level)
    {
        this._level = level;
    }

    public void setAssistant(AssistentModel assistant)
    {
        this._assistant = assistant;
    }

    public void setRequestMade(RequestMade lastOperation)
    {
        this._requestsMade.Add(lastOperation);
    }

}