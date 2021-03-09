﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        CanvasGroup canvasGroup;

        [SerializeField] 

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        public IEnumerator FadeOut (float time)
        {
            while (canvasGroup.alpha < 1)// alpha is not 1
            {
                // move alpha to 1
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0)// alpha is not 1
            {
                // move alpha to 1
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }

}