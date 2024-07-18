using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    /// <summary>
    /// Collection of method used to animate the ui with core routine
    /// </summary>
    public class AnimationCoreRoutine 
    {
        public static IEnumerator Slide(RectTransform transform, SimpleDirection direction, float duration,bool slideIn = true)
        {
            Vector2 startPosition = Vector2.zero;
            Vector2 normalPosition = transform.anchoredPosition;
            switch (direction)
            {
                case SimpleDirection.Up:
                    startPosition = new Vector2(0, -Screen.height);
                    break;
                case SimpleDirection.Down:
                    startPosition = new Vector2(0, Screen.height);
                    break;
                case SimpleDirection.Left:
                    startPosition = new Vector2(Screen.width, 0);
                    break;
                case SimpleDirection.Right:
                    startPosition = new Vector2(-Screen.width, 0);
                    break;
            }
            if (slideIn)
            {
                for (float time = 0; time <= duration; time += Time.deltaTime)
                {
                    transform.anchoredPosition = Vector2.Lerp(startPosition, normalPosition, time / duration);
                    yield return null;
                }
                transform.anchoredPosition = normalPosition;
            }
            //slide out invert starting position
            else
            {
                for (float time = 0; time <= duration; time += Time.deltaTime)
                {
                    transform.anchoredPosition = Vector2.Lerp(normalPosition, startPosition, time / duration);
                    yield return null;
                }
                transform.anchoredPosition = normalPosition;
            }

        }
        public static IEnumerator Scale(RectTransform transform, float duration, bool ScaleIn = true)
        {
            Vector2 startScale = Vector2.one;
            
            if (!ScaleIn)
            {
                for (float time = 0; time <= duration; time += Time.deltaTime)
                {
                    
                    transform.localScale = Vector2.Lerp(startScale, Vector2.zero, time / duration);
                    yield return null;
                }
                transform.localScale = Vector2.zero;
            }
            //slide out invert starting position
            else
            {
                for (float time = 0; time <= duration; time += Time.deltaTime)
                {
                 
                    transform.localScale = Vector2.Lerp(Vector2.zero, startScale, time / duration);
                    yield return null;
                }
                transform.localScale = Vector2.one;
            }

        }

    }
}
