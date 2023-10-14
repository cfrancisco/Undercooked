using System.Threading.Tasks;
using DaltonLima.Core;
using Undercooked.Player;
using UnityEngine;
using UnityEngine.Assertions;

 
namespace Undercooked.Managers
{
    public class SoftGameManager : Singleton<SoftGameManager>
    {
        [SerializeField] private InputController inputController;
  
        private void Awake()
        {
            #if UNITY_EDITOR
                Assert.IsNotNull(inputController);
            #endif
        }

        private async void Start()
        {
        ///    await StartMainMenuAsync();  
         await Task.Delay(1000);
        }
        
    

        private async Task StartMainMenuAsync()
        {
      
            // activate MenuControls
         
         //   inputController.EnableGameplayControls();
        //    inputController.OnStartPressedAtMenu += HandleStartAtMenu;
            
           /* while (_userPressedStart == false)
            {
                await Task.Delay(1000);
            }*/

           // MenuPanelUI.InitialMenuSetActive(false);
            //inputController.OnStartPressedAtMenu -= HandleStartAtMenu;
            await Task.Delay(1000);

        }     
    }
}
