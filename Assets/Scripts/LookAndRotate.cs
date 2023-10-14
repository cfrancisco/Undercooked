using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Undercooked
{
    public class LookAndRotate : MonoBehaviour
    {
        public Transform target;
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = 1;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            transform.LookAt(target);
            transform.Translate(Vector3.right * Time.deltaTime);
        }
    }
}
