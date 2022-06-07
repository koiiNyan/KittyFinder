using UnityEngine;
using UnityEngine.UI;

namespace KittyFinder
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Kitty[] _kitties;
        [SerializeField]
        private Text _kittiesNumberText;
        private int _kittiesLeft = 50;

        private void Awake()
        {
            FollowEvents();
        }

        private void FollowEvents()
        {
            foreach (Kitty kitty in _kitties)
            {
                kitty.OnClickEventHandler += KittyClicked;
            }
        }

        private void KittyClicked(Kitty kitty)
        {
            Debug.Log($"Kitty {kitty} Clicked");
            SetKittyNumber();
        }

        private void SetKittyNumber()
        {
            _kittiesLeft--;
            _kittiesNumberText.text = _kittiesLeft.ToString();
        }
    }
}
