using UnityEngine;
using UnityEngine.EventSystems;

namespace KittyFinder
{
    public class Kitty : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GameObject _circle;

        public event ClickEventHandler OnClickEventHandler;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked Kitty");
            _circle.SetActive(true);
            OnClickEventHandler?.Invoke(this);
        }

        public delegate void ClickEventHandler(Kitty kittyComponent);
    }
}
