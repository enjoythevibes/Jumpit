using System.Collections;
using UnityEngine;

namespace enjoythevibes.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField]
        private AudioSource backgroundSource = default;
        [SerializeField]
        private AudioClip background = default;
        [SerializeField]
        private AudioClip crystal = default;
        [SerializeField]
        private AudioClip[] notes = default;

        private bool active = true;
        private Coroutine playBackgroundCoroutine;
        private Coroutine stopBackgroundCoroutine;
        [SerializeField]
        private AnimationCurve volumeChangeCurve = default;

        public struct NoteArgument : IEventArgument
        {
            public int note;
            public NoteArgument(int note)
            {
                this.note = note;
            }
        }

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();    
            EventsManager.AddListener(Events.GameInitialization, OnGameInitialization);
            EventsManager.AddListener(Events.PlayGame, OnStopPlayBackground);
            EventsManager.AddListener(Events.RestartGame, OnStopPlayBackground);
            EventsManager.AddListener(Events.GameOver, OnStartPlayBackgound);
            EventsManager.AddListener(Events.AddCrystal, OnPlayCrystalSound);
            EventsManager.AddListener(Events.PlayNote, OnPlayNote);
            EventsManager.AddListener(Events.EnableAudio, OnEnableAudio);
            EventsManager.AddListener(Events.DisableAudio, OnDisableAudio);
        }

        private void OnGameInitialization()
        {
            var playerData = Data.DataSaver.playerData;
            if (playerData.audioEnabled == true)
                OnEnableAudio();
            else
                OnDisableAudio();
        }

        private void OnDisableAudio()
        {
            OnStopPlayBackground();
            active = false;
        }

        private void OnEnableAudio()
        {
            active = true;
            OnStartPlayBackgound();
        }

        private void PlaySound(AudioClip audioClip, float volume = 1f)
        {
            if (active)
            {
                audioSource.PlayOneShot(audioClip, volume);    
            }
        }

        private void OnPlayCrystalSound()
        {
            PlaySound(crystal, 0.1f);
        }

        private void OnPlayNote(IEventArgument argument)
        {
            var note = ((NoteArgument)argument).note;
            PlaySound(notes[note]);
        }

        private void OnStartPlayBackgound()
        {
            if (!active) return;
            if (stopBackgroundCoroutine != null)
            {
                StopCoroutine(stopBackgroundCoroutine);
                stopBackgroundCoroutine = null;
            }
            playBackgroundCoroutine = StartCoroutine(PlayLoopBackground());
        }

        private void OnStopPlayBackground()
        {
            if (!active) return;
            if (playBackgroundCoroutine != null)
            {
                StopCoroutine(playBackgroundCoroutine);
                playBackgroundCoroutine = null;
            }
            stopBackgroundCoroutine = StartCoroutine(StopPlayBackground());
        }

        private IEnumerator PlayLoopBackground()
        {
            backgroundSource.clip = background;
            backgroundSource.Play();
            var timer = 0f;
            while (timer < 1f)
            {
                var volume = volumeChangeCurve.Evaluate(timer);
                timer += Time.deltaTime;
                backgroundSource.volume = volume;
                yield return null;
            }
            backgroundSource.volume = 1f;
            while (true)
            {
                yield return new WaitForSeconds(background.length);
                backgroundSource.Play();
            }
        }

        private IEnumerator StopPlayBackground()
        {
            var timer = 1f;
            while (timer >= 0f)
            {
                var volume = volumeChangeCurve.Evaluate(timer);
                timer -= Time.deltaTime;
                backgroundSource.volume = volume;
                yield return null;
            }
            backgroundSource.volume = 0f;
            backgroundSource.Stop();
        }
    }
}