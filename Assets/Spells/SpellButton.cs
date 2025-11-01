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

            if (data.icon != null)
                buttonIcon.sprite = data.icon;
            
            GetComponent<DisplayTooltipOnHover>().SetTextToDisplay(data.spellName, data.description, TooltipDisplay.TooltipType.Spell);

            if (data.spellController != null)
            {
                spellController = Instantiate(data.spellController, transform);
                spellController.Setup(cardController, data, otherButton);
            }
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
