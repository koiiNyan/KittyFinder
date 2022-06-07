using UnityEngine;
using UnityEngine.EventSystems;

namespace KittyFinder
{
    public class Kitty : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GameObject _circle;
        [SerializeField]
        private bool _isNotClicked = true;

        public event ClickEventHandler OnClickEventHandler;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked Kitty");
            _circle.SetActive(true);
            if (_isNotClicked) OnClickEventHandler?.Invoke(this);
            _isNotClicked = false;
        }

        public delegate void ClickEventHandler(Kitty kittyComponent);
    }
}
