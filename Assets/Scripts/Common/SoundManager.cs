using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlchemyPlanet.Common
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        [SerializeField] private AudioClip[] bgms;
        private AudioSource AudioPlayer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            AudioPlayer = GetComponent<AudioSource>();
        }

        public void PlayBGM(int no)
        {
            StopBGM();

            AudioPlayer.clip = bgms[no];
            AudioPlayer.Play();
        }
        public void PauseCurrentBGM()
        {
            if (AudioPlayer.isPlaying)
                AudioPlayer.Pause();
        }
        public void ResumeCurrentBGM()
        {
            if (AudioPlayer.isPlaying)
                AudioPlayer.UnPause();
        }
        public void StopBGM()
        {
            if (AudioPlayer.isPlaying)
                AudioPlayer.Stop();
        }
    }
}
