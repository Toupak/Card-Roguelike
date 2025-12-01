using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace BoomLib.Tools
{
    public static class Fader
    {
        public static IEnumerator Fade(SpriteRenderer sprite, float duration, bool fadeIn, float maxFade = 1.0f, float delay = -1.0f)
        {
            if (delay > 0.0f)
                yield return new WaitForSeconds(delay);
        
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = sprite.color;
        
            while (timer > 0.0f)
            {
                color.a = fade;
                sprite.color = color;
            
                float delta = Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            color.a = fadeIn ? maxFade : 0.0f;
            sprite.color = color;
        }
    
        public static IEnumerator Fade(LineRenderer line, float duration, bool fadeIn, float maxFade = 1.0f, float delay = -1.0f)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = line.startColor;
        
            if (delay > 0.0f)
                yield return new WaitForSeconds(delay);
            
            while (timer > 0.0f)
            {
                color.a = fade;
                line.startColor = color;
                line.endColor = color;
            
                float delta = Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            color.a = fadeIn ? maxFade : 0.0f;
            line.startColor = color;
            line.endColor = color;
        }
    
        public static IEnumerator Fade(Image sprite, float duration, bool fadeIn, float maxFade = 1.0f, bool unscaledTime = false, float delay = -1.0f)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = sprite.color;

            if (delay > 0.0f)
                yield return new WaitForSeconds(delay);
            
            sprite.gameObject.SetActive(true);
        
            while (timer > 0.0f)
            {
                color.a = fade;
                sprite.color = color;
            
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            color.a = fadeIn ? maxFade : 0.0f;
            sprite.color = color;
        
            if (!fadeIn)
                sprite.gameObject.SetActive(false);
        }
    
        public static IEnumerator Fade(List<Image> sprites, float duration, bool fadeIn, float maxFade = 1.0f, bool unscaledTime = false, float delay = -1.0f)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = sprites[0].color;

            if (delay > 0.0f)
                yield return new WaitForSeconds(delay);

            while (timer > 0.0f)
            {
                color.a = fade;

                foreach (Image sprite in sprites)
                    sprite.color = color;
            
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            color.a = fadeIn ? maxFade : 0.0f;
            foreach (Image sprite in sprites)
                sprite.color = color;
        }
    
        public static IEnumerator Fade(Light light, float duration, bool fadeIn, float maxFade = 1.0f, bool unscaledTime = false)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;

            light.gameObject.SetActive(true);
        
            while (timer > 0.0f)
            {
                light.intensity = fade;
            
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            light.intensity = fadeIn ? maxFade : 0.0f;;
        
            if (!fadeIn)
                light.gameObject.SetActive(false);
        }
    
        public static IEnumerator Fade(Light2D light, float duration, bool fadeIn, float maxFade = 1.0f, bool unscaledTime = false)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;

            light.gameObject.SetActive(true);
        
            while (timer > 0.0f)
            {
                light.intensity = fade;
            
                float delta = unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            light.intensity = fadeIn ? maxFade : 0.0f;;
        
            if (!fadeIn)
                light.gameObject.SetActive(false);
        }
    
        public static IEnumerator Fade(RawImage sprite, float duration, bool fadeIn, float maxFade = 1.0f)
        {
            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = sprite.color;
        
            while (timer > 0.0f)
            {
                color.a = fade;
                sprite.color = color;
            
                float delta = Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            color.a = fadeIn ? maxFade : 0.0f;
            sprite.color = color;
        
            if (!fadeIn)
                sprite.gameObject.SetActive(false);
        }
    
        public static IEnumerator Fade(TextMeshProUGUI text, float duration, bool fadeIn, float maxFade = 1.0f)
        {
            if (duration == 0.0f)
            {
                Color tmp = text.color;
                tmp.a = fadeIn ? maxFade : 0.0f;
                text.color = tmp;
                yield break;
            }
        
            text.gameObject.SetActive(true);

            float fade = fadeIn ? 0.0f : maxFade;
            float timer = duration;
            float increment = maxFade / timer;
            Color color = text.color;
        
            while (timer > 0.0f)
            {
                color.a = fade;
                text.color = color;
            
                float delta = Time.deltaTime;
                fade += fadeIn ? delta * increment : -delta * increment;
                timer -= delta;
            
                yield return null;
            }
        
            if (!fadeIn)
                text.gameObject.SetActive(false);
        
            color.a = fadeIn ? maxFade : 0.0f;
            text.color = color;
        }

        public static IEnumerator FadeVolume(AudioSource source, float duration, float from = 0.0f, float to = 0.0f)
        {
            float timer = 0.0f;

            while (timer <= duration)
            {
                source.volume = Tools.NormalizedValueToDecibel(Tools.NormalizeValueInRange(timer, 0.0f, duration, from, to));
                yield return null;
                timer += Time.deltaTime;
            }
        }
    }
}
