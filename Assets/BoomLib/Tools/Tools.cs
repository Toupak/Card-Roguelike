using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BoomLib.Tools
{
    public static class Tools
    { 
        public static Vector3 ToVector3(this Vector2 vector)
        {
            return new Vector3(vector.x, vector.y, 0.0f);
        }
    
        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static float Distance(this Vector2 position, Vector2 other)
        {
            return (other - position).magnitude;
        }

        public static Quaternion ToRotation(this Vector2 direction)
        {
            return Quaternion.AngleAxis(DirectionToDegree(direction), Vector3.forward);
        }

        public static Vector2 AddAngleToDirection(this Vector2 direction, float angle)
        {
            float directionAngle = DirectionToDegree(direction);
            float newAngle = directionAngle + angle;
            return DegreeToVector2(newAngle).normalized;
        }
    
        public static Vector2 AddRandomAngleToDirection(this Vector2 direction, float minInclusive, float maxInclusive)
        {
            float directionAngle = DirectionToDegree(direction);
            float newAngle = directionAngle + Random.Range(minInclusive, maxInclusive);
            return DegreeToVector2(newAngle).normalized;
        }

        public static float DirectionToDegree(Vector2 direction)
        {
            return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
    
        public static float ToSignedDegree(this Vector2 direction)
        {
            return DirectionToDegree(direction);
        }
        
        public static float ToDegree(this Vector2 direction)
        {
            float signed = DirectionToDegree(direction);

            if (signed < 0.0f)
                signed = 360.0f + signed;
            
            return signed;
        }

        public static Vector2 RadianToVector2(float radian, float length = 1.0f)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)).normalized * length;
        }

        public static Vector2 DegreeToVector2(float degree, float length = 1.0f)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad).normalized * length;
        }
    
        public static float DegreeToRadian(float degrees)
        {
            return (Mathf.PI / 180.0f) * degrees;
        }

        public static float NormalizedValueToDecibel(float value)
        {
            if (value == 0.0f)
                return -80.0f;
            else
                return Mathf.Log10(value) * 20;
        }

        public static float RandomPositiveOrNegative(float number = 1.0f)
        {
            int random = (Random.Range(0, 2) * 2) - 1;
            return random * number;
        }

        public static float RandomAround(float number, float perOne)
        {
            return number + RandomPositiveOrNegative(number * perOne);
        }

        public static bool RandomBool()
        {
            return RandomPositiveOrNegative() > 0;
        }

        public static Vector2 RandomPositionInRange(Vector2 position, float range)
        {
            return position + (Random.insideUnitCircle * range);
        }
    
        public static Vector2 RandomPositionAtRange(Vector2 position, float range)
        {
            return position + (Random.insideUnitCircle * range);
        }
    
        public static List<T> GetRandomElements<T>(this IEnumerable<T> list, int elementsCount = 1)
        {
            return list.OrderBy(arg => Guid.NewGuid()).Take(elementsCount).ToList();
        }

        public static float NormalizeValue(float value, float min, float max)
        {
            return (value - min) / (max - min);
        }
    
        public static float NormalizeValueInRange(float value, float min, float max, float rangeMin, float rangeMax)
        {
            return ((rangeMax - rangeMin) * ((value - min) / (max - min))) + rangeMin;
        }

        public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
        {
            if (Quaternion.Dot(a, b) < 0)
            {
                return a * Quaternion.Inverse(Multiply(b, -1));
            }

            else return a * Quaternion.Inverse(b);
        }
    
        public static Vector2 ComputeClosestPointOnLine(Vector2 linePosition, Vector2 lineDirection, Vector2 position)
        {
            Vector2 delta = position - linePosition;
            float dot = Vector2.Dot(delta, lineDirection);
            return linePosition + (lineDirection * dot);
        }
    
        public static Quaternion Multiply(Quaternion input, float scalar)
        {
            return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
        }
    
        public enum MoveDirection
        {
            Right,
            RightBack,
            Back,
            LeftBack,
            Left,
            LeftFront,
            Front,
            RightFront,
        }
    
        public static int GetCardinalDirection(Vector2 currentDirection)
        {
            float angle = Mathf.Atan2(currentDirection.y, currentDirection.x);
            int direction = Mathf.RoundToInt(8 * angle / (2 * Mathf.PI) + 8) % 8;

            return direction;
        }

        public static Vector2 GetDirectionFromCardinal(int direction)
        {
            return Vector2.right.AddAngleToDirection(45 * direction);
        }

        public static void DeleteAllChildren(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(parent.GetChild(i).gameObject);
            }
        }
        
        public static CombatLoop.CombatLoop.TurnType Opposite(this CombatLoop.CombatLoop.TurnType type)
        {
            if (type == CombatLoop.CombatLoop.TurnType.Player)
                return CombatLoop.CombatLoop.TurnType.Enemy;

            if (type == CombatLoop.CombatLoop.TurnType.Enemy)
                return CombatLoop.CombatLoop.TurnType.Player;

            return type;
        }
    }
}