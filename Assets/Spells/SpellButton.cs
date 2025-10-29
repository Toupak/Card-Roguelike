using Cards.Scripts;
using Cursor.Script;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        private SpellController spellController;

        public SpellData spellData { get; private set; }
        public bool isPlayerCard { get; private set; }
        
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
            
            if (spellController != null && spellController.CanCastSpell() && isPlayerCard && spellData != null && isCursorFree && isPlayerTurn)
                spellController.CastSpell(transform, spellData);
        }
    }
}
