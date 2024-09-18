using UnityEngine;

// NB: the asset is created only once because it contains a singleton,
// so we remove the capability of creating others commenting out "CreateAssetMenu" 
// [CreateAssetMenu(menuName = "MainDatabase")]

namespace Undercooked 
{
    public class MainDatabase : ScriptableObject
    {
        // NB: ScriptableObject Singleton needed for Serialization
        // even during play mode recompiles
        public class LifeManager : ScriptableObject
        {

            private static LifeManager _instance;
            public int[] dataFromAassistants = new int[5];

            public static LifeManager GetInstance()
            {
                if (!_instance)
                {
                    // NB: FindObjectOfType is used to retrieve the instance
                    // when play mode recompiles
                    _instance = FindObjectOfType<LifeManager>();
                }
                if (!_instance)
                {
                    // NB: create the Singleton, and initialise its values
                    _instance = CreateInstance<LifeManager>();
                    _instance.dataFromAassistants[0] = 2;
                    _instance.dataFromAassistants[1] = 2;
                    _instance.dataFromAassistants[2] = 2;
                    _instance.dataFromAassistants[3] = 2;
                    _instance.dataFromAassistants[4] = 2;
                }
                return _instance;
            }

            public  int GetLives(int index)
            {
                // Debug.Log("GetLives: "+ index.ToString());
                return this.dataFromAassistants[index];
            }

            public  void SetLives(int index, int number)
            {
                Debug.Log("SetLives: "+index.ToString());
                this.dataFromAassistants[index] = number;
            }

            public  void reduceOneGame(int index)
            {
                Debug.Log("[Select Assistant] Reduced one game in Assistant with index "+index.ToString());
                this.dataFromAassistants[index]--;
            }

        }
     
    }
}