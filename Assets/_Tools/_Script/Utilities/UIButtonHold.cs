using UnityEngine;
using UnityEngine.EventSystems;

namespace Anuj.Utility.Ui
{
    public class UIButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool GetIsHolding => _isHolding;
        private bool _isHolding;
        public void OnPointerDown(PointerEventData eventData)
        {
            _isHolding = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            _isHolding = false;
        }
    }
}
