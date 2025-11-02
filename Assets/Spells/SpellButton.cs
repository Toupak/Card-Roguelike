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
        [SerializeField] private GameObject otherButton;

        [Space] 
        [SerializeField] private SpellController defaultSpellControllerPrefab;
        
        [HideInInspector] public UnityEvent OnClickSpellButton = new UnityEvent();

        public SpellController spellController { get; private set; }
        public SpellData spellData { get; private set; }
        public bool isPlayerCard { get; private set; }
        public Image ButtonIcon => buttonIcon;
        
        public void Setup(CardController cardController, SpellData data, bool isPlayer)
        {
            if (data == null)
                return;
            
            spellData = data;
            isPlayerCard = isPlayer;
            
            SetupTooltip(data);
            SetupButtonIcon(data);
            SetupSpellController(cardController, data);
        }

        private void SetupTooltip(SpellData data)
        {
            GetComponent<DisplayTooltipOnHover>().SetTextToDisplay(data.spellName, data.description, TooltipDisplay.TooltipType.Spell);
        }

        private void SetupButtonIcon(SpellData data)
        {
            if (data.icon != null)
                buttonIcon.sprite = data.icon;
        }
        
        private void SetupSpellController(CardController cardController, SpellData data)
        {
            spellController = Instantiate(data.spellController != null ? data.spellController : defaultSpellControllerPrefab, transform);
            spellController.Setup(cardController, data, otherButton);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            bool isSpellValid = spellController != null && spellController.CanCastSpell(spellData) && spellData != null;
            bool isCursorFree = CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
            bool isPlayerTurn = CombatLoop.CombatLoop.instance.CurrentTurn == CombatLoop.CombatLoop.TurnType.Player;
            
            if (isPlayerCard && isSpellValid && isCursorFree && isPlayerTurn)
                spellController.CastSpell(transform, spellData);
            
            OnClickSpellButton?.Invoke();
        }
    }
}
