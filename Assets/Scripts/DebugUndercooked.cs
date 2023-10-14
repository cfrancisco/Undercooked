using System.Collections;
using System.Threading.Tasks;
using DaltonLima.Core;
using Undercooked.Appliances;
using Undercooked.Data;
using Undercooked.Model;
using Undercooked.Player;
using Undercooked.UI;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

 
namespace Undercooked 
{

    public class DebugUndercooked
    {

        public static void DumpToConsole(string name, object obj)
        {
            var output = JsonUtility.ToJson(obj, true);
            Debug.Log(name+": "+output);
        }
    }

}
