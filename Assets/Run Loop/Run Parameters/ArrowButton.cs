using BoomLib.SFX_Player.Scripts;
using PrimeTween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Sequence = PrimeTween.Sequence;

namespace Run_Loop.Run_Parameters
{
    public class ArrowButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioClip clickSound;

        public UnityEvent OnClick = new UnityEvent(); 
        
        public void OnPointerDown(PointerEventData eventData)
        {
            SFXPlayer.instance.PlaySFX(clickSound);
            Sequence.Create()
                .Group(Tween.PunchScale(transform, Vector3.down, 0.1f))
                .ChainCallback(() => OnClick?.Invoke());
        }
    }
}
