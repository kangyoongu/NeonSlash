using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ImageEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool outlineAction;
    public bool scaleAction;
    public bool pointingAction;
    public float scale;
    public Color color;

    private Outline _outline;
    private Color _originColor;
    private Image _image;
    private void Awake()
    {
        if (outlineAction)
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
        }
        _image = GetComponent<Image>();
        _originColor = _image.color;
    }
    private void OnEnable()
    {
        if (outlineAction) _outline.enabled = false;
        if (scaleAction) transform.localScale = Vector3.one;
        if (pointingAction) _image.color = _originColor;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (outlineAction) _outline.enabled = true;
        if (scaleAction) transform.DOScale(new Vector3(scale, scale, 1), 0.2f).SetUpdate(true);
        if (pointingAction)
        {
            _image.DOColor(color, 0.2f).SetUpdate(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (outlineAction) _outline.enabled = false;
        if (scaleAction) transform.DOScale(Vector3.one, 0.2f).SetUpdate(true);
        if (pointingAction) _image.DOColor(_originColor, 0.2f).SetUpdate(true);
    }
}
