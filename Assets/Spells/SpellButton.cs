using Cards.Scripts;
using Cursor.Script;
using Spells.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        private SpellController spellController;

        private SpellData spellData;
        private bool isPlayerCard;
        
        public void Setup(CardController cardController, SpellData data, bool isPlayer)
        {
            spellData = data;
            isPlayerCard = isPlayer;

            if (data.spellController != null)
            {
                spellController = Instantiate(data.spellController, transform);
                spellController.Setup(cardController);
            }
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            bool isCursorFree = CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
            bool isPlayerTurn = CombatLoop.CombatLoop.instance.CurrentTurn == CombatLoop.CombatLoop.TurnType.Player;
            
            if (spellController != null && isPlayerCard && spellData != null && isCursorFree && isPlayerTurn)
                spellController.CastSpell(transform, spellData);
        }
    }
}
