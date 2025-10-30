using Cards.Scripts;
using Cursor.Script;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image buttonIcon;
         
        private SpellController spellController;

        public SpellData spellData { get; private set; }
        public bool isPlayerCard { get; private set; }
        
        public void Setup(CardController cardController, SpellData data, bool isPlayer)
        {
            spellData = data;
            isPlayerCard = isPlayer;

            if (data.icon != null)
                buttonIcon.sprite = data.icon;

            if (data.spellController != null)
            {
                spellController = Instantiate(data.spellController, transform);
                spellController.Setup(cardController, data);
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            bool isSpellValid = spellController != null && spellController.CanCastSpell(spellData) && spellData != null;
            bool isCursorFree = CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
            bool isPlayerTurn = CombatLoop.CombatLoop.instance.CurrentTurn == CombatLoop.CombatLoop.TurnType.Player;
            
            if (isPlayerCard && isSpellValid && isCursorFree && isPlayerTurn)
                spellController.CastSpell(transform, spellData);
        }
    }
}
