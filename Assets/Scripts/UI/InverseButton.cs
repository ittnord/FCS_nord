using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FCS
{
    public class InverseButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField]
        private Image _backgroundImage;

        public void OnPointerUp(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform,
                eventData.position, eventData.pressEventCamera, out pos))
            {
                InputController.Instance.InverseDirection = false;
            }
                
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform,
                eventData.position, eventData.pressEventCamera, out pos))
            {
                InputController.Instance.InverseDirection = true;
            }
        }
    }
}