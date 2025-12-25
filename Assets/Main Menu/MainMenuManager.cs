using Run_Loop;
using Save_System;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace Main_Menu
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI startGameButtonText;
        
        private bool isFirstTimePlaying;

        private bool hasClicked;
        
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
            if (hasClicked)
                return;
            hasClicked = true;

            if (isFirstTimePlaying)
                RunLoop.instance.GoToIntroFromMainMenu();
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
            if (hasClicked)
                return;
            hasClicked = true;
            
            if (Application.isEditor)
                EditorApplication.ExitPlaymode();
            Application.Quit();
        }
    }
}
