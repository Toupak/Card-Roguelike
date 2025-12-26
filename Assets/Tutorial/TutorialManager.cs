using System.Collections;
using BoomLib.Tools;
using Overworld.Character;
using Overworld.Lights;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        public static TutorialManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void PlayCharacterWakeUpAnimation()
        {
            StartCoroutine(PlayCharacterWakeUpAnimationRoutine());
        }

        private IEnumerator PlayCharacterWakeUpAnimationRoutine()
        {
            Light2D globalLight = GlobalLightHolder.instance.Light2D;
            globalLight.intensity = 0.0f;

            CharacterSingleton.instance.LockPlayer();
            CharacterSingleton.instance.animator.Play("Sleep");
            
            yield return new WaitForSeconds(2.0f);
            yield return Fader.Fade(globalLight, 3.0f, true, 0.3f);
            
            CharacterSingleton.instance.animator.Play("WakeUp");
            yield return new WaitForSeconds(3.0f);
            
            CharacterSingleton.instance.UnlockPlayer();
        }
    }
}
