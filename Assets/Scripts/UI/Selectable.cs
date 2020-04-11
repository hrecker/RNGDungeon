using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Selectable : MonoBehaviour
    {
        public Image selectBackground;
        public Color hoverColor;
        public Color selectedColor;
        public Color hoverSelectedColor;
        private bool selected;

        private Action<Selectable> selectCallback;
        private Action<string> confirmCallback;
        private string confirmCallbackParam;

        private void Start()
        {
            selectBackground.color = Color.clear;
        }

        public void SetConfirmCallback(string param, Action<string> callback)
        {
            this.confirmCallback = callback;
            this.confirmCallbackParam = param;
        }

        public void SetSelectedCallback(Action<Selectable> callback)
        {
            this.selectCallback = callback;
        }

        public void PointerEnter()
        {
            if (selected)
            {
                selectBackground.color = hoverSelectedColor;
            }
            else
            {
                selectBackground.color = hoverColor;
            }
        }

        public void PointerExit()
        {
            if (selected)
            {
                selectBackground.color = selectedColor;
            }
            else
            {
                selectBackground.color = Color.clear;
            }
        }

        public void Deselect()
        {
            selected = false;
            selectBackground.color = Color.clear;
        }

        public void OnClick()
        {
            if (!selected)
            {
                selected = true;
                selectBackground.color = hoverSelectedColor;
                selectCallback?.Invoke(this);
            }
            else
            {
                confirmCallback?.Invoke(confirmCallbackParam);
                Deselect();
            }
        }
    }
}
