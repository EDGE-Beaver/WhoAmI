using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour
{
    public AudioClip bgmClip; 
    private AudioSource audioSource;

    public Slider volumeSlider; // ���� ������ ���� �����̴� (�ν����Ϳ��� ����)

    void Awake()
    {
        // ����� �ҽ��� �ִ��� Ȯ���ϰ� ������ �߰�
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ����� �ҽ� �⺻ ����
        audioSource.clip = bgmClip;
        audioSource.loop = true; // �ݺ� ���
        audioSource.playOnAwake = true; // ���� ���� �� �ڵ� ���

        // �����̴� ������ �ʱ� ���� ���� (�����̴��� ������ �⺻�� 1)
        audioSource.volume = volumeSlider ? volumeSlider.value : 1f;

        // BGM ���
        PlayBGM();

        // �����̴� �� ���� �� ���� ����
        if (volumeSlider)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    // BGM ���
    public void PlayBGM()
    {
        if (!audioSource.isPlaying && bgmClip != null)
        {
            audioSource.Play();
        }
    }

    // BGM ����
    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // �����̴� ���� ���� ���� ����
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }

    private void OnDestroy()
    {
        // �� ���� �Ǵ� ������Ʈ ���� �� �̺�Ʈ ������ ���� (�޸� ���� ������)
        if (volumeSlider)
        {
            volumeSlider.onValueChanged.RemoveListener(SetVolume);
        }
    }
}
