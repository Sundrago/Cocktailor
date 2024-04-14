using UnityEngine;

namespace Cocktailor
{
    /// <summary>
    /// Manages the playing of sound effects.
    /// </summary>
    public class SfxManager : MonoBehaviour
    {
        [SerializeField] private AudioSource[] audios = new AudioSource[10];
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