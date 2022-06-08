using UnityEngine;
using UnityEngine.UI;

namespace KittyFinder
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private static Kitty[] _kitties;
        [SerializeField]
        private Text _kittiesNumberText;
        private static int _kittiesLeft = 50;

        private void Awake()
        {
            FollowEvents();
            LoadKitties();
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
            SetKittyNumber(_kittiesLeft-1);
        }

        private void SetKittyNumber(int kittyNumber)
        {
            _kittiesLeft = kittyNumber;
            _kittiesNumberText.text = _kittiesLeft.ToString();
        }

        public void ExitGameButton()
        {
#if UNITY_EDITOR
            Debug.Log("EXIT");
            UnityEditor.EditorApplication.isPlaying = false;

#else
            Application.Quit();
#endif
            SaveKitties();
        }

        private static void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveKitties();
        }


        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                SaveKitties();
        }

        private static void SaveKitties()
        {
            string path = Application.persistentDataPath + "/savefile.json";
            Debug.Log(path);

            foreach (Kitty kitty in _kitties)
            {
                SaveLoad.SaveKitty(kitty.name, kitty.IsNotClicked, _kittiesLeft);
            }
        }


        private void LoadKitties()
        {
            KittyTable kittyTable = SaveLoad.LoadKitties();
            if (kittyTable == null) return;

            foreach (KittyData kittyData in kittyTable.Kitties)
            {
                foreach (Kitty kitty in _kitties)
                {
                    if (kittyData.Name == kitty.name)
                    {
                        Debug.Log(kittyData.Name);
                        Debug.Log(kittyData.IsNotClicked);
                        kitty.SetKittyActivity(kittyData.IsNotClicked);
                    }
                }
            }

            SetKittyNumber(kittyTable.KittiesLeft);


        }    
    }
}
