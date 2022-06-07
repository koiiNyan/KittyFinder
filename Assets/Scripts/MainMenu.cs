using UnityEngine;
using UnityEngine.SceneManagement;

namespace KittyFinder
{

    public class MainMenu : MonoBehaviour
    {
        public void OnMenuClicked()
        {
            SceneManager.LoadScene(1);
        }
    }
}
