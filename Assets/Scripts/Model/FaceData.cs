using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Undercooked.Model 
{
    public class FaceData : MonoBehaviour
    {

        [Header("Moods")]
        public GameObject[] moodFaceList;
        //[SerializeField]  public static Dictionary<ResponseType, Image> moodFaceTag;
        /*
        = new Dictionary<ResponseType, string>()
        {
            {ResponseType.Happy, "TagHappy"},
            {ResponseType.Confused, "TagConfused"},
            {ResponseType.Angry, "TagAngry"}
        };*/


        // Start is called before the first frame update
        
        public GameObject GetFace(ResponseType type)
        {
            switch (type)
            {
                case ResponseType.Happy:
                    return moodFaceList[0];
                case ResponseType.Confused:
                    return moodFaceList[1];
                case ResponseType.Angry:
                    return moodFaceList[2];
                case ResponseType.Saluting:
                    return moodFaceList[3];
                case ResponseType.Sleepy:
                    return moodFaceList[4];
                case ResponseType.Busy:
                    return moodFaceList[5];
                default:
                    return moodFaceList[0];
            }
   
        }
    }
}