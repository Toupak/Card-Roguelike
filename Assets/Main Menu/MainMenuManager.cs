using BoomLib.Tools;
using Run_Loop;
using Save_System;
using TMPro;
using Tutorial;
using UnityEditor;
using UnityEngine;

namespace Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI startGameButtonText;
        
        private bool isFirstTimePlaying;
        
        private void Start()
        {
            isFirstTimePlaying = SaveSystem.instance.IsNewSave;

            SetupStartGameButton();
        }

        private void SetupStartGameButton()
        {
            startGameButtonText.text = isFirstTimePlaying ? "New Game" : "Continue";
        }

        public void OnClickStartGame()
        {
            if (isFirstTimePlaying)
                TutorialManager.instance.StartTutorial();
            else
                RunLoop.instance.LoadHubFromMainMenu();
        }

        public void OnClickOptions()
        {
            
        }

        public void OnClickCredit()
        {
            
        }

        public void OnClickQuit()
        {
            if (Application.isEditor)
                EditorApplication.ExitPlaymode();
            Application.Quit();
        }
    }
}
