/* 
 * Game: GGJ2016
 * Author: Arhan Bakan
 * 
 * MusicManager.cs
 * Handles music in battle scene.
 */

using UnityEngine;

namespace Assets.Scripts
{
    public class MusicManager : MonoBehaviour
    {
        public AudioClip GameMusic;
        AudioSource _currentAudioSource;
        static bool AudioBegin = false;

        void Awake()
        {
            if (!AudioBegin)
            {
                _currentAudioSource = gameObject.GetComponent<AudioSource>();
                _currentAudioSource.Play();
                DontDestroyOnLoad(gameObject);
                AudioBegin = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
