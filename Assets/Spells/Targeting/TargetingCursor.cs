using BoomLib.Tools;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spells.Targeting
{
    public class TargetingCursor : MonoBehaviour
    {
        [Header("ArrowPrefabs")]
        [SerializeField] private GameObject arrowHeadPrefab;
        [SerializeField] private GameObject arrowNodePrefab;
        [SerializeField] private int arrowNodeQuantity;
        [SerializeField] private float arrowScaleFactor;

        private RectTransform origin;
        private List<RectTransform> arrowNodes = new List<RectTransform>();
        private List<Vector2> controlPoints = new List<Vector2>();
        private readonly List<Vector2> controlPointsFactors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };

        private bool isSetup;

        public void Setup(Transform startingPosition)
        {
            isSetup = true;

            origin = GetComponent<RectTransform>();

            for (int i = 0; i < arrowNodeQuantity; i++)
            {
                arrowNodes.Add(Instantiate(arrowNodePrefab, transform).GetComponent<RectTransform>());
            }

            arrowNodes.Add(Instantiate(arrowHeadPrefab, transform).GetComponent<RectTransform>());

            for (int i = 0; i < 4; i++)
            {
                controlPoints.Add(Vector2.zero);
            }
        }

        public void UpdatePosition()
        {
            if (!isSetup)
                return;

            //P0 : ArrowHead Point
            controlPoints[0] = new Vector2(origin.position.x, origin.position.y);

            //P3 : Mouse position
            controlPoints[3] = Mouse.current.position.value;

            //P1, P2, determined by P0 & P3
            controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointsFactors[0];
            controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointsFactors[1];

            //Need (arrowNodes.Count - 1) =/= 0
            for (int i = 0; i < arrowNodes.Count; i++)
            {
                //Cubic Bezier Curve
                float t = Mathf.Log(1f * i / (arrowNodes.Count - 1f) + 1f, 2f);

                arrowNodes[i].position =
                    Mathf.Pow(1 - t, 3) * controlPoints[0] +
                    3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1] +
                    3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                    Mathf.Pow(t, 3) * controlPoints[3];

                //Calculates rotation for ArrowNodes
                if (i > 0)
                {
                    Vector3 euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.right, arrowNodes[i].position - arrowNodes[i - 1].position));
                    arrowNodes[i].rotation = Quaternion.Euler(euler);
                }

                //Calculates scale for ArrowNodes
                float scale = arrowScaleFactor * (1 - 0.03f * (arrowNodes.Count - 1 - i));
                arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
            }

            //Calculates first arrow node rotation
            arrowNodes[0].transform.rotation = arrowNodes[1].transform.rotation;
        }
        
        public void DestroyCursor()
        {
            if (gameObject != null)
                Destroy(gameObject);
        }
    }
}
