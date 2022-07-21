using System;
using System.Collections;
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
        private const int c_originalKittiesLeft = 50;
        [SerializeField]
        private ParticleSystem _congrateParticle;
        [SerializeField]
        private float _particleSeconds = 8f;
        [SerializeField]
        private GameObject _victoryText;

        private bool _isGameRunning = true;
        private Coroutine coUpdateTimer;
        [SerializeField] private Text _timer_Txt;
        [SerializeField] private float _currentTime;
        private string _timePlayingStr;
        private TimeSpan _timePlaying;

        private AudioSource _gameAudio;
        [SerializeField] private AudioClip _victorySound;
        [SerializeField] private AudioClip _backSound;


        private void Awake()
        {
            FollowEvents();
            LoadKitties();
            BeginTimer();
            _gameAudio = GetComponent<AudioSource>();
        }

        private void BeginTimer()
        {

            if (coUpdateTimer != null)
            {
                StopCoroutine(coUpdateTimer);
            }

            coUpdateTimer = StartCoroutine(UpdateTimer());
        }

        private IEnumerator UpdateTimer()
        {
            while (_isGameRunning)
            {
                _currentTime += Time.deltaTime;

                _timePlaying = TimeSpan.FromSeconds(_currentTime);
                _timePlayingStr = _timePlaying.ToString("mm':'ss");
               // Debug.Log(_timePlaying);
                Debug.Log(_timePlayingStr);
                _timer_Txt.text = _timePlayingStr;

                yield return null;
            }
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
            Debug.Log(_kittiesLeft);

            //_kittiesLeft = 0;
            if (_kittiesLeft == 0)
            {
                GameWin();
            }
        }

        private void SetKittyNumber(int kittyNumber)
        {
            _kittiesLeft = kittyNumber;
            _kittiesNumberText.text = _kittiesLeft.ToString();
        }

        private void GameWin()
        {
            Debug.Log("GameWin");
            _isGameRunning = false;
            _gameAudio.Stop();
            _gameAudio.PlayOneShot(_victorySound);
            StartCoroutine(PlayParticles(_particleSeconds));
        }

        private IEnumerator PlayParticles(float secs)
        {
            Debug.Log("PlayParticles");
            _congrateParticle.Play();
            _victoryText.SetActive(true);
            yield return new WaitForSeconds(secs);
            _congrateParticle.Stop();
            _victoryText.SetActive(false);
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

        public void RestartGameButton()
        {
            Debug.Log("Restart");
            SetKittyNumber(c_originalKittiesLeft);

            foreach(Kitty kitty in _kitties)
            {
                kitty.SetKittyActivity(true);
            }
            StopAllCoroutines();
            _currentTime = 0;

            _isGameRunning = true;
            BeginTimer();
            _gameAudio.Play();//////////////////////////

        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                SaveKitties();
        }


        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                SaveKitties();
        }

        private void SaveKitties()
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
