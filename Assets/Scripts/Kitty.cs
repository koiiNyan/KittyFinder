using UnityEngine;
using UnityEngine.EventSystems;

namespace KittyFinder
{
    public class Kitty : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GameObject _circle;
        private AudioSource _kittyAudio;
        [SerializeField] private AudioClip _kittySound;

        public bool IsNotClicked { get; private set; } = true;

        private void Start()
        {
            _kittyAudio = GetComponent<AudioSource>();
        }

        public event ClickEventHandler OnClickEventHandler;
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked Kitty");
            _circle.SetActive(true);
            _kittyAudio.PlayOneShot(_kittySound);

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
