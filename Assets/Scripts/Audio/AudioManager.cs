using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public enum BGMType
{
    None,
    Main,
    Lobby,
    Battle
}

public enum SFXType
{
    None,
    DropExp,
    AddExp,
    Attack,
    Boom,
    Goblin_Attack,
    Goblin_Die,
    Hit,
    WalkGrass,
    SlowMotion,
    Dash,
    Ice_A,
    Ice_S,
    Ice_D,
    Ice_DP,
    Ice_Q,
    Ice_W,
    MartialHero_Attack,
    MartialHero_SPAttack,
    MartialHero_Sheath,
    MartialHero_Die,
}

public class AudioManager : MonoBehaviour
{
    [Header("Mixer Settings")]
    public AudioMixer AudioMixer;

    [Header("Background Music")]
    public AudioSource BgmSource;

    [Header("SFX Template Source")]
    public AudioSource SfxSource; // 풀링을 위한 템플릿으로만 사용

    [Header("SFX Pool Settings")]
    public int sfxPoolSize = 10;

    private Dictionary<BGMType, AudioClip> bgmDict = new Dictionary<BGMType, AudioClip>();
    private Dictionary<SFXType, AudioClip> sfxDict = new Dictionary<SFXType, AudioClip>();

    private List<AudioSource> sfxSourcePool = new List<AudioSource>();
    private AudioSource loopSource;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadAudioClips("Audio/BGM", bgmDict);
        LoadAudioClips("Audio/SFX", sfxDict);
        InitializeSFXPool();
    }

    void LoadAudioClips<TEnum>(string folderPath, Dictionary<TEnum, AudioClip> dict) where TEnum : System.Enum
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderPath);
        dict.Clear();

        foreach (AudioClip clip in clips)
        {
            foreach (TEnum enumValue in System.Enum.GetValues(typeof(TEnum)))
            {
                if (enumValue.Equals(default(TEnum))) continue;

                if (clip.name == enumValue.ToString())
                {
                    dict[enumValue] = clip;
                    Debug.Log($"{typeof(TEnum).Name} 로드 완료: {clip.name}");
                }
            }
        }
    }

    void InitializeSFXPool()
    {
        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.outputAudioMixerGroup = SfxSource.outputAudioMixerGroup;
            sfxSourcePool.Add(source);
        }
    }

    AudioSource GetAvailableSFXSource()
    {
        foreach (var source in sfxSourcePool)
        {
            if (!source.isPlaying)
                return source;
        }
        return null; // 부족하면 새로 생성하거나 무시
    }

    public void PlayBGM(BGMType bgmType)
    {
        if (bgmDict.TryGetValue(bgmType, out AudioClip clip))
        {
            BgmSource.clip = clip;
            BgmSource.loop = true;
            BgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{bgmType}' 을(를) 찾을 수 없습니다.");
        }
    }

    public void PlaySFX(SFXType sfxType)
    {
        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.pitch = 1f;
                source.clip = clip;
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxType}' 을(를) 찾을 수 없습니다.");
        }
    }

    public void PlaySFX(SFXType sfxType, float pitch)
    {
        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.pitch = pitch;
                source.clip = clip;
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxType}' 을(를) 찾을 수 없습니다.");
        }
    }

    public void PlaySFX(SFXType sfxType, float minPitch, float maxPitch)
    {
        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            AudioSource source = GetAvailableSFXSource();
            if (source != null)
            {
                source.pitch = Random.Range(minPitch, maxPitch);
                source.clip = clip;
                source.Play();
            }
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxType}' 을(를) 찾을 수 없습니다.");
        }
    }

    public void PlaySFXLoop(SFXType sfxType)
    {
        if (loopSource == null)
        {
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.loop = true;
            loopSource.playOnAwake = false;
            loopSource.outputAudioMixerGroup = SfxSource.outputAudioMixerGroup;
        }

        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            loopSource.clip = clip;
            loopSource.Play();
        }
        else
        {
            Debug.LogWarning($"루프 SFX '{sfxType}' 을(를) 찾을 수 없습니다.");
        }
    }

    public void StopSFXLoop()
    {
        if (loopSource != null && loopSource.isPlaying)
        {
            loopSource.Stop();
        }
    }

    public void SetBGMVolume(float value)
    {
        AudioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }

    public void SetSFXVolume(float value)
    {
        AudioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
}
