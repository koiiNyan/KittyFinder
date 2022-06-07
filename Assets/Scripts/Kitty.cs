using UnityEngine;
using UnityEngine.EventSystems;

namespace KittyFinder
{
    public class Kitty : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GameObject _circle;

        public bool IsNotClicked { get; private set; } = true;

        public event ClickEventHandler OnClickEventHandler;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked Kitty");
            _circle.SetActive(true);
            if (IsNotClicked) OnClickEventHandler?.Invoke(this);
            IsNotClicked = false;
        }

        public delegate void ClickEventHandler(Kitty kittyComponent);

        public void SetKittyActivity(bool isNotClicked)
        {
            IsNotClicked = isNotClicked;
            _circle.SetActive(!isNotClicked);

        }
    }
}
