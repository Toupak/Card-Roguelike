using ActionReaction;
using Cards.Scripts;
using Cursor.Script;
using Tooltip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image buttonIcon;
        [SerializeField] private SpellButton otherButton;
        [SerializeField] public int spellIndex;

        [Space] 
        [SerializeField] private SpellController defaultSpellControllerPrefab;
        
        [HideInInspector] public UnityEvent OnClickSpellButton = new UnityEvent();
        [HideInInspector] public UnityEvent OnCastSpell = new UnityEvent();

        private DisplayTooltipOnHover displayTooltipOnHover;
        
        public SpellController spellController { get; private set; }
        public SpellData spellData { get; private set; }
        public Image ButtonIcon => buttonIcon;
        
        public void Setup(CardController cardController, SpellData data)
        {
            if (data == null || CombatLoop.CombatLoop.instance == null)
                return;
            
            spellData = data;
            
            SetupButtonIcon(data);
            SetupSpellController(cardController, data);
        }

        private void SetupButtonIcon(SpellData data)
        {
            if (data.icon != null)
                buttonIcon.sprite = data.icon;
        }
        
        private void SetupSpellController(CardController cardController, SpellData data)
        {
            spellController = Instantiate(data.spellController != null ? data.spellController : defaultSpellControllerPrefab, transform);
            spellController.Setup(cardController, data, this, otherButton);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            bool isSpellValid = spellController != null && spellController.CanCastSpell() && spellData != null;
            
            if (!isSpellValid)
                return;
            
            bool isCursorFree = CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
            bool isPlayerTurn = CombatLoop.CombatLoop.instance != null && CombatLoop.CombatLoop.instance.currentTurn == CombatLoop.CombatLoop.TurnType.Player;

            if (isCursorFree && isPlayerTurn)
            {
                spellController.CastSpell(transform);
                OnCastSpell?.Invoke();
            }
            
            OnClickSpellButton?.Invoke();
        }
    }
}
