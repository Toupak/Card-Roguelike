using System.Collections;
using BoomLib.Scene_Management;
using BoomLib.SFX_Player.Scripts;
using BoomLib.Tools;
using UnityEngine;
using UnityEngine.UI;
using Button = BoomLib.UI.Scripts.Button;

namespace BoomLib.Main_Menu.Scripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        [SerializeField] private Button newGameButton;
        [SerializeField] private AudioClip startSound;

        [Space] 
        [SerializeField] private SceneField targetScene;
        
        private bool isTransitionToIntroStarted;
        
        private IEnumerator Start()
        {
            blackScreen.gameObject.SetActive(true);
            newGameButton.HideInstantly();
            
            yield return new WaitForSeconds(2.0f);
            SFXPlayer.instance.PlaySFX(startSound);
        
            StartCoroutine(Fader.Fade(blackScreen, 4.0f, false));
            yield return new WaitForSeconds(2.0f);
            
            newGameButton.OnClick.AddListener(GoToIntroScene);
            newGameButton.Display();
        }

        private void GoToIntroScene()
        {
            if (isTransitionToIntroStarted)
                return;

            isTransitionToIntroStarted = true;
            
            SceneSwapper.instance.SwapScene(targetScene);
        }
    }
}
