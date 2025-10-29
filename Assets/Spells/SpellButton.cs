using Cursor.Script;
using Spells.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SpellController spellController;

        private SpellData spellData;
        private bool isPlayerCard;
        
        public void Setup(SpellData data, bool isPlayer)
        {
            spellData = data;
            isPlayerCard = isPlayer;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            bool isCursorFree = CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free;
            bool isPlayerTurn = CombatLoop.CombatLoop.instance.CurrentTurn == CombatLoop.CombatLoop.TurnType.Player;
            
            if (isPlayerCard && spellData != null && isCursorFree && isPlayerTurn)
                spellController.CastSpell(transform, spellData);
        }
    }
}
