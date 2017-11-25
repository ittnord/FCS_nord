using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FCS
{
    public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [SerializeField]
        private Image _backgroundImage;
        [SerializeField]
        private Image _controllerImage;
        [SerializeField]
        private float _radius = 200;
        

        private void UpdatePosition(PointerEventData eventData)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundImage.rectTransform,
                eventData.position, eventData.pressEventCamera, out pos))
            {
                var offset = pos / _radius;
                offset = offset.sqrMagnitude > 1 ? offset.normalized : offset;
                InputController.Instance.InputDirection = new Vector3(offset.x, 0, offset.y);
                _controllerImage.rectTransform.anchoredPosition = offset * _radius;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdatePosition(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            InputController.Instance.InputDirection = Vector3.zero;
            _controllerImage.rectTransform.anchoredPosition = Vector3.zero;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdatePosition(eventData);
        }
    }
}