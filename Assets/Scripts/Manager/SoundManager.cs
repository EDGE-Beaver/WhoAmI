using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사운드 파일을 관리하는 매니저
/// 효과음, BGM, 보이스 오디오 파일을 저장 및 재생
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Header("🔊 효과음 파일 (Inspector에서 추가 가능)")]
    public List<AudioClip> SoundEffectClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> SoundEffect = new Dictionary<string, AudioClip>();

    [Header("🎵 BGM 파일 (Inspector에서 추가 가능)")]
    public List<AudioClip> BgmAudioClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> BgmAudio = new Dictionary<string, AudioClip>();

    [Header("🎤 보이스 파일 (Inspector에서 추가 가능)")]
    public List<AudioClip> VoiceAudioClips = new List<AudioClip>();
    private Dictionary<string, AudioClip> VoiceAudio = new Dictionary<string, AudioClip>();

    [Header("🔊 오디오 소스 (필요한 오브젝트를 드래그)")]
    public AudioSource SeAudioSource;
    public AudioSource BgmAudioSource;
    public AudioSource VoiceAudioSource;

    private string CurrentSeFile;
    private string CurrentBgmFile;
    private string CurrentVoiceFile;
    private AudioClip CurrentVoiceClip; // 🔥 현재 재생할 보이스 클립

    void Awake()
    {
        // 🔥 Inspector에서 추가한 오디오 클립을 Dictionary에 저장
        foreach (var clip in SoundEffectClips)
        {
            SoundEffect[clip.name] = clip;
        }
        foreach (var clip in BgmAudioClips)
        {
            BgmAudio[clip.name] = clip;
        }
        foreach (var clip in VoiceAudioClips)
        {
            VoiceAudio[clip.name] = clip;
        }

        Debug.Log($"✅ 등록된 효과음 목록: {string.Join(", ", SoundEffect.Keys)}");
        Debug.Log($"✅ 등록된 BGM 목록: {string.Join(", ", BgmAudio.Keys)}");
        Debug.Log($"✅ 등록된 보이스 목록: {string.Join(", ", VoiceAudio.Keys)}");
    }

    /* ========== 🎵 오디오 설정 함수 ========== */

    /// <summary>
    /// 현재 효과음을 설정합니다.
    /// </summary>
    public void SetCurrentSe(string SeName)
    {
        if (!SoundEffect.ContainsKey(SeName))
        {
            Debug.LogError($"❌ SetCurrentSe 오류: {SeName}이라는 효과음 파일이 없습니다!");
            return;
        }
        CurrentSeFile = SeName;
        Debug.Log($"✅ 현재 효과음 설정됨: {CurrentSeFile}");
    }

    /// <summary>
    /// 현재 BGM을 설정합니다.
    /// </summary>
    public void SetCurrentBgm(string BgmName)
    {
        if (!BgmAudio.ContainsKey(BgmName))
        {
            Debug.LogError($"❌ SetCurrentBgm 오류: {BgmName}이라는 BGM 파일이 없습니다!");
            return;
        }
        CurrentBgmFile = BgmName;
        Debug.Log($"✅ 현재 BGM 설정됨: {CurrentBgmFile}");
    }

    /// <summary>
    /// 현재 보이스를 설정합니다.
    /// </summary>
    public void SetCurrentVoice(string VoiceName)
    {
        if (!VoiceAudio.ContainsKey(VoiceName))
        {
            Debug.LogError($"❌ SetCurrentVoice 오류: {VoiceName}이라는 보이스 파일이 없습니다!");
            return;
        }
        CurrentVoiceFile = VoiceName;
        CurrentVoiceClip = VoiceAudio[VoiceName];
        Debug.Log($"✅ 현재 보이스 설정됨: {CurrentVoiceFile}");
    }

    /* ========== 🔊 오디오 재생 함수 ========== */

    /// <summary>
    /// 현재 효과음을 재생합니다.
    /// </summary>
    public void PlayCurrentSe()
    {
        if (!SoundEffect.ContainsKey(CurrentSeFile))
        {
            Debug.LogError($"❌ PlayCurrentSe 오류: {CurrentSeFile} 효과음이 존재하지 않습니다!");
            return;
        }
        SeAudioSource.PlayOneShot(SoundEffect[CurrentSeFile]);
        Debug.Log($"▶️ 효과음 재생: {CurrentSeFile}");
    }
    /// <summary>
    /// 현재 설정된 보이스 파일명을 반환합니다.
    /// </summary>
    public string GetCurrentVoiceFile()
    {
        return CurrentVoiceFile;
    }

    /// <summary>
    /// 현재 BGM을 재생합니다.
    /// </summary>
    public void PlayCurrentBgm()
    {
        if (!BgmAudio.ContainsKey(CurrentBgmFile))
        {
            Debug.LogError($"❌ PlayCurrentBgm 오류: {CurrentBgmFile} BGM이 존재하지 않습니다!");
            return;
        }
        BgmAudioSource.clip = BgmAudio[CurrentBgmFile];
        BgmAudioSource.Play();
        Debug.Log($"▶️ BGM 재생: {CurrentBgmFile}");
    }

    /// <summary>
    /// 현재 보이스를 재생합니다.
    /// </summary>
    public void PlayCurrentVoice()
    {
        if (CurrentVoiceClip == null)
        {
            Debug.LogError("❌ PlayCurrentVoice 오류: 현재 설정된 보이스 파일이 없습니다!");
            return;
        }
        VoiceAudioSource.PlayOneShot(CurrentVoiceClip);
        VoiceAudioSource.Play();
        Debug.Log($"🎤 보이스 재생: {CurrentVoiceFile}");
    }

    /* ========== 🎚 보이스 설정 함수 ========== */

    /// <summary>
    /// 보이스의 피치를 조절합니다. (-3 ~ 3 범위)
    /// </summary>
    public void SetVoicePitch(float pitch)
    {
        VoiceAudioSource.pitch = Mathf.Clamp(pitch, -3f, 3f);
        Debug.Log($"🎛 보이스 피치 변경: {VoiceAudioSource.pitch}");
    }

    /// <summary>
    /// 보이스의 볼륨을 조절합니다. (0 ~ 1 범위)
    /// </summary>
    public void SetVoiceVolume(float volume)
    {
        VoiceAudioSource.volume = Mathf.Clamp01(volume);
        Debug.Log($"🔊 보이스 볼륨 변경: {VoiceAudioSource.volume}");
    }
}
