using System;
using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using UnityEngine;
using UnityEngine.Assertions;
using static BoomLib.Music_Player.Scripts.MusicTrack;
using Random = UnityEngine.Random;

namespace BoomLib.Music_Player.Scripts
{
    [Serializable]
    public class MusicTrack
    {
        public enum TrackType
        {
            MainMenu,
            InGame,
            Battle,
            None
        }

        public AudioClip musicTrack;
        public TrackType trackType;
    }
    
    /// <summary>
    /// 
    /// Hello ! I am the Music Player, my job is to play music clips in loops.
    /// I am a Singleton, you must put me in the scene.
    /// I need a reference to an AudioSource that exist in the scene. (usually a child of the GameObject I am attached to)
    /// I need a list of Tracks containing each an AudioClip and a TrackType, these are the music clips you want me to play.
    ///
    /// You can then call my method PlayMusic() and optionally give it a TrackType.
    /// I will look in the list of Tracks to play an AudioClip with the matching TrackType.
    /// 
    /// If you call PlayMusic() with TrackType == None, I will select a random AudioClip from all the available ones.
    ///
    /// You can also temporarily lower the volume of the music, during a cutscene or a dialog for example.
    /// 
    /// </summary>
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<MusicTrack> tracks;
        
        public static MusicPlayer instance;
        
        private MusicTrack currentTrack;
        public MusicTrack CurrentTrack => currentTrack;
        
        public bool isPlaying => audioSource.isPlaying;
        
        private bool isVolumeLow;
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;
        }

        private void Start()
        {
            Assert.IsNotNull(audioSource, $"[{nameof(MusicPlayer)}] error : audioSource not referenced in Inspector");
            Assert.IsTrue(tracks.Count > 0, $"[{nameof(MusicPlayer)}] error : no tracks referenced in Inspector");
            for (int i = 0; i < tracks.Count; i++)
                Assert.IsNotNull(tracks[i].musicTrack, $"[{nameof(MusicPlayer)}] error : audioClip not referenced in Music Track at index : {i}");
            SetupAudioSource(audioSource);
        }

        private void SetupAudioSource(AudioSource source)
        {
            source.volume = 0.0f; //important for the first fade in
            source.loop = true;
            source.bypassEffects = true;
            source.bypassListenerEffects = true;
            source.bypassReverbZones = true;
            source.priority = 0; //important to have high priority to not get cut if too many low priority sounds are being played
        }

        /// <summary>
        /// Call this method to start playing music in a loop.
        /// </summary>
        /// <param name="trackType">The type of music to be played. Defaults to None, meaning any clip can be selected.</param>
        /// <param name="fadeDuration">The volume will increase linearly during this duration. Defaults to 0.0, meaning the volume starts at maximum value.</param>
        public void PlayMusic(TrackType trackType = TrackType.None, float fadeDuration = 0.0f)
        {
            MusicTrack track = trackType == TrackType.None ? ChooseRandomClip() : ChooseClip(trackType);
            PlayTrack(track);

            isVolumeLow = false;
            FadeVolume(from:0.0f, to:1.0f, fadeDuration);
        }

        /// <summary>
        /// Call this method to temporarily lower the volume of the music.
        /// </summary>
        /// <param name="fadeDuration">The volume will decrease linearly during this duration. Defaults to 0.0, meaning the volume instantly switch to a lower value.</param>
        /// <param name="targetVolume">The volume will decrease until reaching this value. The range is [0.0, 1.0]</param>
        public void LowerVolume(float fadeDuration = 0.0f, float targetVolume = 0.3f)
        {
            if (isVolumeLow || !isPlaying)
                return;

            isVolumeLow = true;
            FadeVolume(from:audioSource.volume, targetVolume, fadeDuration);
        }
        
        /// <summary>
        /// Call this method to reset the volume of the music after lowering it.
        /// </summary>
        /// <param name="fadeDuration">The volume will increase linearly during this duration. Defaults to 0.0, meaning the volume instantly switch to a higher value.</param>
        /// <param name="targetVolume">The volume will increase until reaching this value. The range is [0.0, 1.0]</param>
        public void RaiseBackVolume(float fadeDuration = 0.0f, float targetVolume = 1.0f)
        {
            if (!isVolumeLow || !isPlaying)
                return;

            isVolumeLow = false;
            FadeVolume(from:audioSource.volume, targetVolume, fadeDuration);
        }

        private void PlayTrack(MusicTrack track)
        {
            audioSource.clip = track.musicTrack;
            audioSource.Play();
            currentTrack = track;
        }

        private MusicTrack ChooseClip(TrackType trackType)
        {
            List<MusicTrack> compatibleTracks = tracks.Where((t) => t.trackType == trackType).ToList();

            if (compatibleTracks.Count < 1)
                return ChooseRandomClip();

            int index = Random.Range(0, compatibleTracks.Count);
            return compatibleTracks[index];
        }

        private MusicTrack ChooseRandomClip()
        {
            int index = Random.Range(0, tracks.Count);
            return tracks[index];
        }
        
        private void FadeVolume(float from = 0.0f, float to = 1.0f, float fadeDuration = 0.0f)
        {
            StopAllCoroutines();

            if (fadeDuration > 0.0f)
                StartCoroutine(Fader.FadeVolume(audioSource, fadeDuration, from, to));
            else
                audioSource.volume = to;
        }
    }
}
