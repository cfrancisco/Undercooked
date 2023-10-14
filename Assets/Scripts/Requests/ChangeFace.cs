using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Undercooked.Data;
using Undercooked.Model;


namespace Undercooked.Requests
{
    public class ChangeFace : MonoBehaviour
    {

        [Header("Generic Faces")]
        [SerializeField] private GameObject _currentFace;
        [SerializeField] private GameObject _neutralFace;
        [SerializeField] private GameObject _previousFace;

        [Header("Mood Faces")]
        [SerializeField] private GameObject _goodFace;
        [SerializeField] private GameObject _confusedFace;
        [SerializeField] private GameObject _badFace;
        [SerializeField] private GameObject _salutingFace;
        [SerializeField] private GameObject _sleepyFace;
        [SerializeField] private GameObject _busyFace;
  
        [Header("Animation")]
        public Animator headAnimation;

        public void changeFace(ResponseType response)
        {
            _currentFace.SetActive(false);
            _previousFace = _currentFace;

            switch (response)
            {
                case ResponseType.Happy:
                    _currentFace = _goodFace;
                    break;
                case ResponseType.Confused:
                    _currentFace = _confusedFace;
                    break;
                case ResponseType.Angry:
                    _currentFace = _badFace;
                    break;
                case ResponseType.Normal:
                    _currentFace = _neutralFace;
                    break;
                case ResponseType.Saluting:
                    _currentFace = _salutingFace;
                    break;
                case ResponseType.Sleepy:
                    _currentFace = _sleepyFace;
                    break;
                case ResponseType.Busy:
                    _currentFace = _busyFace;
                    break;
                default:
                    _currentFace = _goodFace;
                    break;
            }

            _currentFace.SetActive(true);
 
            /// Animate Face
            headAnimation.SetBool("isPoping", true);
            
            // TODO som para quando expandir o rosto
            StartCoroutine(DisableAnimationFace());

        }

         private IEnumerator DisableAnimationFace()
        {
            yield return new WaitForSeconds(2);
              headAnimation.SetBool("isPoping", false);
        }
        

       
    }
}
