using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AudeLeLuel.RetroOSUIPack
{
    public class Popup : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransform rectTransform = transform as RectTransform;
            CanvasScaler canvasScaler = GetComponentInParent<CanvasScaler>();

            float xScaler = canvasScaler.referenceResolution.x / Screen.width;
            float yScaler = canvasScaler.referenceResolution.y / Screen.height;
            float scaler = ((1 - canvasScaler.matchWidthOrHeight) * xScaler) + (canvasScaler.matchWidthOrHeight * yScaler);

            rectTransform.anchoredPosition += eventData.delta * scaler;
        }
    }

}