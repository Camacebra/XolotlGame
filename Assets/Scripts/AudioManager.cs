using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Helpers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        public class Audio{
            public string tag; //Nombre del audio
            public AudioClip clip; //  Clip de audio
            public AudioSource audioSource;
            [Range(0, 5)] public float minTime; // Tiempo minimo entre clip
            public bool useCustomTime;
            public string customTagTime;
            [HideInInspector] public float timeElpse = 0;
        }
        public static AudioManager instance = null;
        [Header("BACKGROUNDMUSIC")]
        [SerializeField] AudioSource backgroundSound;
        [Header("AUDIOS")]
        public Audio[] audios;
        [HideInInspector] public List<string> clipsTags; // Lista de nombres de los clips
        [HideInInspector] public Dictionary<string, Audio> clipsDictionary; //Diccionario de audioclips
        AudioSource mainAudio;
        //Generamos la variable estatica antes de iniciar el juego
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);
        }
        protected void Start()
        {
            mainAudio = GetComponent<AudioSource>();
            //Crear diccionario de audios a utilizar al iniciar el juego
            clipsDictionary = new Dictionary<string, Audio>();
            foreach (Audio audio in audios)
            {
                clipsDictionary.Add(audio.tag, audio);
                clipsTags.Add(audio.tag);
            }
        }

        public void PlayClip(string tag, float vol = 1, float pitch = 1, AudioSource customAudioSource = null)
        {
            if (!clipsDictionary.ContainsKey(tag)) {
                Debug.LogWarning("Error clip no encontrado" + tag);
                return;
            }
            Audio audio = clipsDictionary[tag];
            AudioSource playAudio = audio.audioSource;
            if (!playAudio) playAudio = mainAudio;
            if (customAudioSource) playAudio = customAudioSource;
            // Comparemos el tiempo minimo entre clip
            if (audio.minTime > 0) {
                if (audio.useCustomTime)
                {
                    if (Time.time - clipsDictionary[audio.customTagTime].timeElpse > audio.minTime){
                        playAudio.pitch = pitch;
                        playAudio.PlayOneShot(audio.clip, vol);
                        clipsDictionary[audio.customTagTime].timeElpse = Time.time;
                    }
                }
                else if (Time.time - audio.timeElpse > audio.minTime) {
                    playAudio.pitch = pitch;
                    playAudio.PlayOneShot(audio.clip, vol);
                    audio.timeElpse = Time.time;
                }
            }
            else {
                playAudio.pitch = pitch;
                playAudio.PlayOneShot(audio.clip, vol);
            }
        }
        public void StopAudios(AudioSource stopAudio = null)
        {
            if (!stopAudio) stopAudio = mainAudio;
            stopAudio.Stop();
        }
        public void PlayFadeAudio(string tag, float vol = 0, float time = 0, bool loop = false)
        {
            if (!clipsDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Error clip no encontrado" + tag);
                return;
            }
            Audio audio = clipsDictionary[tag];
            AudioSource playAudio = audio.audioSource;
            if (!playAudio)
            {
                Debug.LogWarning("Error se necesita un AudioSource");
                return;
            }
            playAudio.clip = audio.clip;
            playAudio.volume = 0;
            playAudio.loop = loop;
            StartCoroutine(FadeAudio(playAudio, vol, time));
            playAudio.Play();
        }
        public void StopAudioFade(string tag, float time = 0)
        {
            if (!clipsDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Error clip no encontrado" + tag);
                return;
            }
            Audio audio = clipsDictionary[tag];
            AudioSource playAudio = audio.audioSource;
            if (!playAudio)
            {
                Debug.LogWarning("Error se necesita un AudioSource");
                return;
            }
            StartCoroutine(FadeAudio(playAudio, 0, time, true));
        }


        IEnumerator FadeAudio(AudioSource audio, float Vol, float time, bool stop = false)
        {
            float timeElpse = 0;
            float initialSound = audio.volume;
            while (timeElpse < time){
                audio.volume = Mathf.Lerp(initialSound, Vol, timeElpse / time);
                timeElpse += Time.deltaTime;
                yield return null;
            }
            audio.volume = Vol;
            if (stop)
                audio.Stop();
        }
    }
}

