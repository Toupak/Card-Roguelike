using BoomLib.SFX_Player.Scripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Map.Encounters.Fusion.Spell_Button_Toggle
{
    public class FusionToggleButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private AudioClip clickSound;

        public UnityEvent OnClick = new UnityEvent(); 
        
        public bool isOn { get; private set; }
        
        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SFXPlayer.instance.PlaySFX(clickSound);
            
            isOn = !isOn;
            OnClick.Invoke();

            animator.Play(isOn ? "Click_On" : "Click_Off");
        }
    }
}