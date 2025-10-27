using Cursor.Script;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SpellController spellController;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (CursorInfo.instance.currentMode == CursorInfo.CursorMode.Free)
                spellController.CastSpell(transform);
        }
    }
}
