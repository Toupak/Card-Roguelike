using UnityEngine;

namespace BoomLib.Tools
{
    public static class BBounds
    {
        public class BoundsData
        {
            public Vector2 position;
            public Vector2 size;

            public Vector2 bottomLeft => ComputeBottomLeft();
            public Vector2 topRight => ComputeTopRight();

            public float area => ComputeArea();

            private Vector2 ComputeTopRight()
            {
                return position + (Vector2.right * (size.x / 2.0f)) + (Vector2.up * (size.y / 2.0f));
            }

            private Vector2 ComputeBottomLeft()
            {
                return position + (Vector2.left * (size.x / 2.0f)) + (Vector2.down * (size.y / 2.0f));
            }

            private float ComputeArea()
            {
                return size.x * size.y;
            }

            public BoundsData(Vector2 Position, Vector2 Size)
            {
                position = Position;
                size = Size;
            }

            public static BoundsData Extrude(BoundsData targetBoundsData, float f)
            {
                Vector2 newSize = targetBoundsData.size + new Vector2(f * 2.0f, f * 2.0f);
                return new BoundsData(targetBoundsData.position, newSize);
            }
        
            public static Vector2 ConstraintBounds(BoundsData inBound, BoundsData outBound)
            {
                float rightConstraint = (outBound.position.x + (outBound.size.x / 2.0f)) - (inBound.position.x + (inBound.size.x / 2.0f));
                float leftConstraint =  (outBound.position.x - (outBound.size.x / 2.0f)) - (inBound.position.x - (inBound.size.x / 2.0f));
            
                float upConstraint = (outBound.position.y + (outBound.size.y / 2.0f)) - (inBound.position.y + (inBound.size.y / 2.0f));
                float downConstraint = (outBound.position.y - (outBound.size.y / 2.0f)) - (inBound.position.y - (inBound.size.y / 2.0f));

                Vector2 direction = Vector2.zero;
            
                if (rightConstraint < 0)
                    direction.x -= rightConstraint;

                if (leftConstraint > 0)
                    direction.x -= leftConstraint;
            
                if (upConstraint < 0)
                    direction.y -= upConstraint;

                if (downConstraint > 0)
                    direction.y -= downConstraint;
            
                return direction;
            }
        }
        
        public static void DrawSquare(BoundsData bound, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(bound.bottomLeft, bound.bottomLeft + (Vector2.up * bound.size.y));
            Gizmos.DrawLine(bound.bottomLeft, bound.bottomLeft + (Vector2.right * bound.size.x));
            Gizmos.DrawLine(bound.topRight, bound.topRight + (Vector2.down * bound.size.y));
            Gizmos.DrawLine(bound.topRight, bound.topRight + (Vector2.left * bound.size.x));
        }
    
        public static void DrawCone(Vector2 position, Vector2 direction, float angle, float distance, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawLine(position, position + (direction.AddAngleToDirection(angle) * distance));
            Gizmos.DrawLine(position, position + (direction.AddAngleToDirection(-angle) * distance));
        }
    }
}
