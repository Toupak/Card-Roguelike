using BoomLib.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Spells.Targeting
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : Graphic
    {
        [SerializeField] private float thickness;

        private Vector2 start = Vector2.zero;
        private Vector2 end = Vector2.zero;
        
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            Vector2 direction = (end - start).normalized;
            float halfDistance = thickness / 2.0f;
            Vector2 perpendicular = direction.AddAngleToDirection(90.0f) * halfDistance;
            
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            vertex.position = start + perpendicular;
            vh.AddVert(vertex);
            
            vertex.position = start - perpendicular;
            vh.AddVert(vertex);
            
            vertex.position = end + perpendicular;
            vh.AddVert(vertex);
            
            vertex.position = end - perpendicular;
            vh.AddVert(vertex);
            
            vh.AddTriangle(0, 2, 1);
            vh.AddTriangle(1, 2, 3);
        }

        public void SetLinePositions(Vector2 _start, Vector2 _end)
        {
            start = _start;
            end = _end;
            SetVerticesDirty();
        }
    }
}
