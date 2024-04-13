using UnityEngine;

namespace Cocktailor
{
    public class SfxManager : MonoBehaviour
    {
        public AudioSource[] audios = new AudioSource[10];
        public static SfxManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void PlaySfx(int idx)
        {
            if (PlayerPrefs.GetInt("sfxSet") == 0) return;
            audios[idx].Play();
        }
    }
}