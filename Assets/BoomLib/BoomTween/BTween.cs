using System.Collections;
using BoomLib.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BoomLib.BoomTween
{
    public static class BTween
    {
        public static IEnumerator Squeeze(Transform target, float duration = 0.1f, bool deactivateOnEnd = false, bool unscaledTime = false)
        {
            yield return Squeeze(target, Vector3.one, new Vector2(1.3f, 0.7f), duration, deactivateOnEnd, unscaledTime);
        }

        public static IEnumerator Squeeze(Transform target, Vector3 originalSize, Vector2 squeeze, float duration = 0.1f, bool deactivateOnEnd = false, bool unscaledTime = false)
        {
            Vector3 newSize = new Vector3(squeeze.x * originalSize.x, squeeze.y * originalSize.y, originalSize.z);

            float t = 0f;
            while (t <= 1.0)
            {
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                t += delta / duration;
                target.localScale = Vector3.Lerp(newSize, originalSize, t);
                yield return null;
            }

            target.localScale = originalSize;
            if (deactivateOnEnd)
                target.gameObject.SetActive(false);
        }
        
        public static IEnumerator Shake(Transform target, float duration, float intensity, bool horizontal = false, bool vertical = false, bool unscaledTime = false)
        {
            Vector2 previousShake = Vector2.zero;
        
            float timer = 0.0f;
            while (timer <= duration)
            {
                Vector2 direction = Random.insideUnitCircle;

                if (horizontal)
                    direction.y = 0.0f;

                if (vertical)
                    direction.x = 0.0f;

                target.position += ((direction * intensity) - previousShake).ToVector3();
                previousShake = direction * intensity;
            
                yield return null;
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                timer += delta;
            }

            target.position -= previousShake.ToVector3();
        }

        public static IEnumerator TweenPosition(RectTransform target, Vector3 targetPosition, float duration, bool deactivateOnEnd = false, bool unscaledTime = false)
        {
            target.gameObject.SetActive(true);
        
            Vector3 velocity = Vector3.zero;
        
            while (Vector3.Distance(target.position, targetPosition) >= 0.15f)
            {
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                target.position = Vector3.SmoothDamp(target.position, targetPosition, ref velocity, duration, maxSpeed:Mathf.Infinity, deltaTime:delta);
                yield return null;
            }

            target.position = targetPosition;
        
            if (deactivateOnEnd)
                target.gameObject.SetActive(false);
        }
    
        public static IEnumerator TweenLocalPosition(Transform target, Vector3 targetPosition, float duration, bool deactivateOnEnd = false, bool unscaledTime = false)
        {
            target.gameObject.SetActive(true);

            Vector3 startingPosition = target.localPosition;
            float totalDistanceToTarget = Vector3.Distance(startingPosition, targetPosition);
            Vector3 direction = (targetPosition - startingPosition).normalized;
            Vector3 offsetTargetPosition = targetPosition + direction * 3.0f;
        
            Vector3 velocity = Vector3.zero;

            while (Vector3.Distance(startingPosition, target.localPosition) < totalDistanceToTarget)
            {
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                target.localPosition = Vector3.SmoothDamp(target.localPosition, offsetTargetPosition, ref velocity, duration, maxSpeed:Mathf.Infinity, deltaTime:delta);
                yield return null;
            }

            target.localPosition = targetPosition;
        
            if (deactivateOnEnd)
                target.gameObject.SetActive(false);
        }
    
        public static IEnumerator TweenLocalScale(RectTransform target, Vector3 targetScale, float duration, bool deactivateOnEnd = false, bool unscaledTime = false)
        {
            target.gameObject.SetActive(true);
        
            Vector3 velocity = Vector3.zero;
        
            while (Vector3.Distance(target.localScale, targetScale) >= 0.05f)
            {
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                target.localScale = Vector3.SmoothDamp(target.localScale, targetScale, ref velocity, duration, maxSpeed:Mathf.Infinity, deltaTime:delta);
                yield return null;
            }

            target.localScale = targetScale;
        
            if (deactivateOnEnd)
                target.gameObject.SetActive(false);
        }
    
        public static IEnumerator TweenLocalScale(Transform target, Vector3 targetScale, float duration, bool deactivateOnEnd = false, bool unscaledTime = false)
        {
            target.gameObject.SetActive(true);
        
            Vector3 velocity = Vector3.zero;
        
            while (Vector3.Distance(target.localScale, targetScale) >= 0.05f)
            {
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                target.localScale = Vector3.SmoothDamp(target.localScale, targetScale, ref velocity, duration, maxSpeed:Mathf.Infinity, deltaTime:delta);
                yield return null;
            }

            target.localScale = targetScale;
        
            if (deactivateOnEnd)
                target.gameObject.SetActive(false);
        }
        
        public static Vector3 LerpVector3(Vector3 start, Vector3 end, float t)
        {
            Vector3 current;
            current.x = Mathf.Lerp(start.x, end.x, t);
            current.y = Mathf.Lerp(start.y, end.y, t);
            current.z = Mathf.Lerp(start.z, end.z, t);

            return current;
        }
    
        public static Vector2 LerpVector2(Vector2 start, Vector2 end, float t)
        {
            Vector2 current;
            current.x = Mathf.Lerp(start.x, end.x, t);
            current.y = Mathf.Lerp(start.y, end.y, t);

            return current;
        }
        
        public static IEnumerator FillImage(Image image, float duration, bool fillIn, float maxFill = 1.0f, bool scaledTime = true)
        {
            float fill = fillIn ? 0.0f : maxFill;
            float timer = duration;
            float increment = maxFill / timer;

            image.gameObject.SetActive(true);
        
            while (timer > 0.0f)
            {
                image.fillAmount = fill;
            
                float delta = scaledTime ? Time.deltaTime : Time.unscaledDeltaTime;
                fill += fillIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            image.fillAmount = fillIn ? maxFill : 0.0f;;
        
            if (!fillIn)
                image.gameObject.SetActive(false);
        }

        public static Image MakeTransparent(this Image image)
        {
            Color transparent = Color.white;
            transparent.a = 0.0f;
            image.color = transparent;

            return image;
        }
    
        public static Image MakeVisible(this Image image)
        {
            Color visible = Color.white;
            visible.a = 1.0f;
            image.color = visible;

            return image;
        }

        public static Image SetImageColor(Image image, float alpha = 1.0f)
        {
            Color color = image.color;
            color.a = alpha;
            image.color = color;

            return image;
        }

        public static TextMeshProUGUI SetTextColor(TextMeshProUGUI text, float alpha = 1.0f)
        {
            if (alpha > 0.0f)
                text.gameObject.SetActive(true);
        
            Color color = text.color;
            color.a = alpha;
            text.color = color;

            return text;
        }
    }
}
