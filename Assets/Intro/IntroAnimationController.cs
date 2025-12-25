using System.Collections;
using PrimeTween;
using Run_Loop;
using UnityEngine;

namespace Intro
{
    public class IntroAnimationController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup line_1;
        [SerializeField] private CanvasGroup line_2;
        [SerializeField] private CanvasGroup line_3;
        [SerializeField] private CanvasGroup line_4;

        private IEnumerator Start()
        {
            line_1.alpha = 0.0f;
            line_2.alpha = 0.0f;
            line_3.alpha = 0.0f;
            line_4.alpha = 0.0f;

            yield return new WaitForSeconds(1.0f);

            bool isAnimationCompleted = false;

            Sequence.Create()
                .Chain(Tween.Alpha(line_1, 1.0f, 4.0f))
                .Chain(Tween.Alpha(line_2, 1.0f, 1.5f))
                .Chain(Tween.Alpha(line_3, 1.0f, 1.0f))
                .Chain(Tween.Alpha(line_4, 1.0f, 2.0f))
                .Chain(Tween.Alpha(line_1, 0.0f, 3.0f))
                .Group(Tween.Alpha(line_2, 0.0f, 3.0f))
                .Group(Tween.Alpha(line_3, 0.0f, 3.0f))
                .Group(Tween.Alpha(line_4, 0.0f, 3.0f))
                .ChainCallback(() => isAnimationCompleted = true);

            yield return new WaitUntil(() => isAnimationCompleted);
            yield return new WaitForSeconds(1.0f);
            
            RunLoop.instance.StartRunFromIntro();
        }
    }
}
