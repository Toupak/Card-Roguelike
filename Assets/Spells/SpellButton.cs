using UnityEngine;
using UnityEngine.EventSystems;

namespace Spells
{
    public class SpellButton : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private SpellController spellController;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            spellController.CastSpell(transform);
        }
    }
}
