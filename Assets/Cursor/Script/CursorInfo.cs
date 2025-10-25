using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Board.Script;

namespace Cursor.Script
{
    public class CursorInfo : MonoBehaviour
    {
        public static CursorInfo instance;

        public Container lastContainer { get; private set; } = null;
        
        private void Awake()
        {
            instance = this;
        }

        private void FixedUpdate()
        {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            EventSystem.current.RaycastAll(pointerEventData, results);
            
            if (results.Count < 1)
                return;

            List<RaycastResult> container = results.Where((c) => c.gameObject.CompareTag("Container")).ToList();
            
            if (container.Count < 1)
                return;
            
            SetLastContainer(container[0].gameObject.GetComponent<Container>());
        }

        public void SetLastContainer(Container newContainer)
        {
            lastContainer = newContainer;
        }
    }
}
