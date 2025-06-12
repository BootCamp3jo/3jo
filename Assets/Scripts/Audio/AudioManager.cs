using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

public enum BGMType
{
    None,
    Main,
}
public enum SFXType
{
    None,
    DropExp,
    AddExp,
}

public class AudioManager : MonoBehaviour
{
    [Header("Mixer Settings")]
    public AudioMixer AudioMixer;

    [Header("Background Music")]
    public AudioSource BgmSource;

    [Header("Sound Effects")]
    public AudioSource SfxSource;

    private Dictionary<BGMType, AudioClip> bgmDict = new Dictionary<BGMType, AudioClip>();
    private Dictionary<SFXType, AudioClip> sfxDict = new Dictionary<SFXType, AudioClip>();

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
    }


    // 오디오 클립 로드 (BGM/SFX)
    void LoadAudioClips<TEnum>(string folderPath, Dictionary<TEnum, AudioClip> dict) where TEnum : System.Enum
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(folderPath);
        dict.Clear();

        foreach (AudioClip clip in clips)
        {
            foreach (TEnum enumValue in System.Enum.GetValues(typeof(TEnum)))
            {
                // None 이나 기본값은 건너뜀
                if (enumValue.Equals(default(TEnum))) continue;

                if (clip.name == enumValue.ToString())
                {
                    dict[enumValue] = clip;
                    Debug.Log($"{typeof(TEnum).Name} 로드 완료: {clip.name}");
                }
            }
        }
    }

    // 배경음 출력
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

    // 효과음 출력
    public void PlaySFX(SFXType sfxType)
    {
        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            SfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxType}' 을(를) 찾을 수 없습니다.");
        }
    }

    // 효과음 출력 - 피치 볼륨 조절
    public void PlaySFX(SFXType sfxType,float pitch)
    {
        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            SfxSource.pitch = pitch;
            SfxSource.PlayOneShot(clip);
            SfxSource.pitch = 1f;
        }
        else
        {
            Debug.LogWarning($"SFX '{sfxType}' 을(를) 찾을 수 없습니다.");
        }
    }
    // 랜덤 피치 효과음 출력
    public void PlaySFX(SFXType sfxType, float minPitch, float maxPitch)
    {
        if (sfxDict.TryGetValue(sfxType, out AudioClip clip))
        {
            float randomPitch = Random.Range(minPitch, maxPitch);
            SfxSource.pitch = randomPitch;
            SfxSource.clip = clip;
            SfxSource.Play();
            // 끝나면 pitch 원래대로 복구 필요하면 추가
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

    // 배경음 볼륨 조절
    public void SetBGMVolume(float value)
    {
        AudioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
    // 효과음 볼륨 조절
    public void SetSFXVolume(float value)
    {
        AudioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20);
    }
}
