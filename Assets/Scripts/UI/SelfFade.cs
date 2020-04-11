using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SelfFade : MonoBehaviour
    {
        public float fadeTime;
        public bool destroySelf;
        private List<Graphic> toFade;

        void Start()
        {
            toFade = new List<Graphic>();
            toFade.AddRange(GetComponents<Graphic>());
            toFade.AddRange(GetComponentsInChildren<Graphic>());

            foreach (Graphic graphic in toFade)
            {
                graphic.CrossFadeAlpha(0, fadeTime, false);
            }
            if (destroySelf)
            {
                Destroy(gameObject, fadeTime);
            }
        }
    }
}
