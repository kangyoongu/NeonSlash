using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Kang
{
    [RequireComponent(typeof(Image))]
    public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        [HideInInspector] public TabGroup tabGroup;

        [HideInInspector] public Image background;
        [HideInInspector] public Action OnClick;
        private void Awake()
        {
            tabGroup = GetComponentInParent<TabGroup>();
            background = GetComponent<Image>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SoundManager.Instance.PlayAudio(Clips.Button);
            tabGroup.OnTabSelected(this);
            OnClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tabGroup.OnTabEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tabGroup.OnTabExit(this);
        }
    }
}