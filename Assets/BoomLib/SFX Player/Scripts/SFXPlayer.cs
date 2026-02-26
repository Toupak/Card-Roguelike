using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BoomLib.SFX_Player.Scripts
{
    /// <summary>
    /// 
    /// Hello ! I am the SFX Player, my job is to play sfx clips in one shot when asked.
    /// I am a Singleton, you must put me in the scene.
    /// I need a reference to an AudioSource prefab in your project that I can Instantiate each time a clip is played.
    ///
    /// I always return the AudioSources I create.
    /// I usually set the AudioSources I create to auto-destroy after they are done playing their clip, unless they are looping.
    ///
    /// I usually set the pitch of the clips I play to a random value around 1.0.
    /// But you can give me a specific pitch when calling my PlaySFX() methods.
    /// 
    /// </summary>
    public class SFXPlayer : MonoBehaviour
    {
        const float RANDOM_PITCH = -3.5f; //Not the real random pitch, it is replaced during Instantiation to : 1.0f + Random.Range(-0.05f, 0.05f)
        
        [SerializeField] private AudioSource audioSourcePrefab;

        public static SFXPlayer instance;

        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;
        }
        
        private void Start()
        {
            Assert.IsNotNull(audioSourcePrefab, $"[{nameof(SFXPlayer)}] : audioSourcePrefab not referenced in Inspector");
        }

        /// <summary>
        /// Call this method to play an AudioClip in one shot. If you want to play a random clip from a list, use PlayRandomSFX
        /// </summary>
        /// <param name="clip">The clip that should be played.</param>
        /// <param name="volume">The volume the clip will be played at. The range is [0.0, 1.0]</param>
        /// <param name="delay">The delay before playing the clip. Defaults to 0.0</param>
        /// <param name="loop">Should the clip keep playing in a loop. If true, the AudioSource will not auto-destroy. Defaults to false.</param>
        /// <param name="pitch">The pitch the clip will be played at. The range is [-3.0, 3.0]. Defaults to random around 1.0. (1.0 => no pitch change)</param>
        /// <returns>
        /// The AudioSource that got Instantiated already setup and playing the clip.
        /// The AudioSource will auto-destroy after playing the clip UNLESS it is looping.
        /// </returns>
        public AudioSource PlaySFX(AudioClip clip, float volume = 0.1f, float delay = 0.0f, bool loop = false, float pitch = RANDOM_PITCH)
        {
            if (clip == null)
                return null;
            
            return CreateSFX(clip, volume, delay, loop, pitch);
        }
        
        /// <summary>
        /// Call this method to play a random AudioClip from a list in one shot. If you want to play a single clip, use PlaySFX
        /// </summary>
        /// <param name="clips">The list of clips that should be selected from.</param>
        /// <param name="volume">The volume the clip will be played at. The range is [0.0, 1.0]</param>
        /// <param name="delay">The delay before playing the clip. Defaults to 0.0</param>
        /// <param name="loop">Should the clip keep playing in a loop. If true, the AudioSource will not auto-destroy. Defaults to false.</param>
        /// <param name="pitch">The pitch the clip will be played at. The range is [-3.0, 3.0]. Defaults to random around 1.0. (1.0 => no pitch change)</param>
        /// <returns>
        /// The AudioSource that got Instantiated already setup and playing the clip.
        /// The AudioSource will auto-destroy after playing the clip UNLESS it is looping.
        /// </returns>
        public AudioSource PlayRandomSFX(List<AudioClip> clips, float volume = 0.1f, float delay = 0.0f, bool loop = false, float pitch = RANDOM_PITCH)
        {
            int index = Random.Range(0, clips.Count);
            return CreateSFX(clips[index], volume, delay, loop, pitch);
        }

        private AudioSource CreateSFX(AudioClip clip, float volume, float delay, bool loop, float pitch)
        {
            AudioSource source = Instantiate(audioSourcePrefab);

            source.clip = clip;
            source.volume = volume;
            source.loop = loop;
            source.pitch = pitch > RANDOM_PITCH ? pitch : 1.0f + Random.Range(-0.05f, 0.05f);
            source.priority = 1;

            if (delay <= 0.0f)
                source.Play();
            else
                source.PlayDelayed(delay);

            if (!loop)
                Destroy(source.gameObject, clip.length + delay + 0.05f);

            return source;
        }
    }
}
