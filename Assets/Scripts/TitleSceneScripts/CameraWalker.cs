using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//ī�޶� �̵� -> UI �̵����� ����
public class CameraWalker : MonoBehaviour
{
    /*�ν����� ���� �⺻ ����
     * ī�޶� �̵� �ӵ�(Cam/Zoom Speed) : 5, 5
     * ���̵� �ƿ� �ð�(Fade Time) : 1
     * */
    private void Start()
    {


        //===== ī�޶� �̵� ȿ�� =====
        //ī�޶� �̵� ������ �޾ƿ���@@@
        titlePos_Scale = camPos_Title.transform.localScale;
        lobbyPos_Scale = camPos_MainLobby.transform.localScale;
        bookPos_Scale = camPos_Book.transform.localScale;

        titlePos = new Vector3(camPos_Title.transform.localPosition.x / -titlePos_Scale.x, camPos_Title.transform.localPosition.y / -titlePos_Scale.y, UI.transform.localPosition.z);
        lobbyPos = new Vector3(camPos_MainLobby.transform.localPosition.x / -lobbyPos_Scale.x, camPos_MainLobby.transform.localPosition.y / -lobbyPos_Scale.y, UI.transform.localPosition.z);
        bookPos = new Vector3(camPos_Book.transform.localPosition.x / -bookPos_Scale.x, camPos_Book.transform.localPosition.y / -bookPos_Scale.y, UI.transform.localPosition.z);

        targetPos = UI.transform.localPosition;
        targetScale = UI.transform.localScale;

        //===== ���̵� ȿ�� =====
        can = fadeoutBlack.GetComponent<CanvasGroup>();
        canq = fadeoutBlack_QUIT.GetComponent<CanvasGroup>();
        fadeoutBlack_QUIT.SetActive(false);
        TitleStart();

        //===== ���� ���� & ���� �޴� ���â =====
        WarnBox_Q.SetActive(false);
        WarnBox_QtextM.SetActive(false);
        WarnBox_QtextQ.SetActive(false);
    }

    private void Update()
    {
        //===== ī�޶� �̵� ȿ�� =====
        UI.transform.localPosition = Vector3.Lerp(UI.transform.localPosition, targetPos, camSpeed * Time.deltaTime);
        UI.transform.localScale = Vector3.Lerp(UI.transform.localScale, targetScale, zoomSpeed * Time.deltaTime);

        //Lerp �������� ������(Ȥ�� �� ������ ����� �־��)
        alphaCheck1 = can.alpha;
        if (can.alpha <= 0.01f && alphaCheck1 < alphaCheck2)
        {
            can.alpha = 0f;
        }
        if (can.alpha >= 0.95f && alphaCheck1 > alphaCheck2)
        {
            can.alpha = 1f;
        }
        alphaCheck2 = alphaCheck1;

        //Esc ������ Ÿ��Ʋ�� Ż��
        if (!inTitle && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isEscWarning)
            {
                EscWarning_NO();
            }
            else
            {
                if (inLobby)
                {
                    ToBookCam();
                }
                else
                {
                    ToTitle();
                }
            }
        }
    }

    [Header("===== 1. ī�޶� �̵� ȿ�� =====")]

    [Header("ī�޶�(ī�޶� ��� �̵��� UI)")]
    public GameObject UI;

    [Header("ī�޶� �̵� ������")]
    public GameObject camPos_Title;
    public GameObject camPos_MainLobby;
    public GameObject camPos_Book;

    [Header("ī�޶� �̵� �ӵ�(��� �̵�, Ȯ��/���)")]
    public float camSpeed;
    public float zoomSpeed;

    private Vector3 titlePos_Scale;
    private Vector3 lobbyPos_Scale;
    private Vector3 bookPos_Scale;

    private Vector3 titlePos;
    private Vector3 lobbyPos;
    private Vector3 bookPos;

    private Vector3 targetPos;
    private Vector3 targetScale;

    private bool inTitle;
    private bool inLobby;
    private bool inBook;

    public void ToTitleCam()
    {
        PlaySFX(); //@@@ ȿ���� ����
        if (titleIntro)
        {
            camSpeed = 1f;
            FadeIn();
        }
        else
        {
            camSpeed = 5f;
        }
        targetPos = titlePos;
        targetScale = new Vector3(3.2f / titlePos_Scale.x, 3.2f / titlePos_Scale.y, 3.2f / titlePos_Scale.z);
        inTitle = true;
        inLobby = false;
        inBook = false;
    }

    public void ToLobbyCam()
    {
        PlaySFX(); //@@@ ȿ���� ����
        camSpeed = 5f;
        targetPos = lobbyPos;
        targetScale = new Vector3(3.2f / lobbyPos_Scale.x, 3.2f / lobbyPos_Scale.y, 3.2f / lobbyPos_Scale.z);
        inTitle = false;
        inLobby = true;
        inBook = false;
    }

    public void ToBookCam()
    {
        PlaySFX(); //@@@ ȿ���� ����
        camSpeed = 5f;
        targetPos = bookPos;
        targetScale = new Vector3(3.2f / bookPos_Scale.x, 3.2f / bookPos_Scale.y, 3.2f / bookPos_Scale.z);
        inTitle = false;
        inLobby = false;
        inBook = true;
    }

    private void PlaySFX()
    {
        if (sfxPlayer && menuChangeSFX)
        {
            sfxPlayer.volume = sfxVolumeSlider ? sfxVolumeSlider.value : 1.0f;
            sfxPlayer.PlayOneShot(menuChangeSFX);
        }
    }

    private float alphaCheck1 = 0f, alphaCheck2 = 0f;
    private bool titleIntro = false;

    public void TitleStart()
    {
        inTitle = true;
        inLobby = false;
        inBook = false;

        UI.transform.localPosition = new Vector3(titlePos.x, titlePos.y + 150f, titlePos.z);
        UI.transform.localScale = new Vector3(3.2f / titlePos_Scale.x, 3.2f / titlePos_Scale.y, 3.2f / titlePos_Scale.z);
        titleIntro = true;
        ToTitle();
        ToTitleCam();
    }

    [Header("===== 2. ���̵� ȿ�� =====")]
    public GameObject fadeoutBlack;
    public GameObject fadeoutBlack_QUIT;
    public float fadeTime;

    private CanvasGroup can;
    private CanvasGroup canq;

    [Header("===== 3. ȿ���� ���� =====")]
    public AudioSource sfxPlayer;
    public AudioClip menuChangeSFX;
    public Slider sfxVolumeSlider;

    [Header("===== 4. ���� ���� ���� =====")]
    public string startSceneName = "MainMenu"; //@@@ �ν����Ϳ��� �� ����

    public void ToTitle()
    {
        ToTitleCam();
        book_title.SetActive(true);
        book_load.SetActive(false);
        book_credit.SetActive(false);
        book_setting.SetActive(false);
        book_whole.SetActive(false);
    }

    [Header("===== 5. Ÿ��Ʋ �޴��� ȭ�� ��ȯ =====")]

    public GameObject book_whole;
    public GameObject book_title;
    public GameObject book_load;
    public GameObject book_credit;
    public GameObject book_setting;

    // ���� ���� ��ư�� ������ �� ����
    public void ToGameStart()
    {
        PlaySFX(); // ȿ���� ���
        StartCoroutine(GameStartSequence());
    }
    public void FadeOut()
    {
        if (onGameQuit)
        {
            StopCoroutine(FadeOutEffect_Quit());
            StartCoroutine(FadeOutEffect_Quit());
        }
        else
        {
            onGameQuit = false;
            StopCoroutine(FadeOutEffect());
            StartCoroutine(FadeOutEffect());
        }
    }
    IEnumerator FadeOutEffect()
    {
        fadeoutBlack.SetActive(true);
        can.interactable = true;
        can.blocksRaycasts = true;
        can.alpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            if (elapsedTime >= fadeTime)
            {
                can.alpha = 1f;
                break;
            }
            can.alpha = Mathf.SmoothStep(0f, 1f, elapsedTime / (fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator FadeOutEffect_Quit() //���� ����� ���̵�ƿ�
    {
        fadeoutBlack_QUIT.SetActive(true);
        canq.interactable = true;
        canq.blocksRaycasts = true;
        canq.alpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            if (elapsedTime >= fadeTime)
            {
                canq.alpha = 1f;
                break;
            }
            canq.alpha = Mathf.SmoothStep(0f, 1f, elapsedTime / (fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
    // �� ���� �� ���� ���� �� �̵�
    private IEnumerator GameStartSequence()
    {
        // ���̵� �ƿ� ȿ��
        FadeOut();

        // ���̵� �ƿ��� ���� ������ ���
        yield return new WaitForSeconds(fadeTime);

        // �� �̵�
        if (!string.IsNullOrEmpty(startSceneName))
        {
            SceneManager.LoadScene(startSceneName);
        }
    }

    //���� ����
    private bool onGameQuit = false;
    public async void QuitGame()
    {
        StopAllCoroutines();
        onGameQuit = true;
        FadeOut();
        await Task.Delay((int)fadeTime * 1000);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void ToLoad()
    {
        book_whole.SetActive(true);
        ToBookCam();
        book_title.SetActive(false);
        book_load.SetActive(true);
        book_credit.SetActive(false);
        book_setting.SetActive(false);
    }

    public void ToCredit()
    {
        book_whole.SetActive(true);
        ToBookCam();
        book_title.SetActive(false);
        book_load.SetActive(false);
        book_credit.SetActive(true);
        book_setting.SetActive(false);
    }

    public void FadeIn()
    {
        if (titleIntro)
        {
            titleIntro = false;
            StopCoroutine(FadeInEffect_Title());
            StartCoroutine(FadeInEffect_Title());
        }
        else
        {
            StopCoroutine(FadeInEffect());
            StartCoroutine(FadeInEffect());
        }
    }

    IEnumerator FadeInEffect()
    {
        fadeoutBlack.SetActive(true);
        can.interactable = false;
        can.blocksRaycasts = false;
        can.alpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            if (elapsedTime >= fadeTime)
            {
                can.alpha = 0f;
                break;
            }
            can.alpha = Mathf.SmoothStep(1f, 0f, elapsedTime / (fadeTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeoutBlack.SetActive(false);
    }

    IEnumerator FadeInEffect_Title()
    {
        fadeoutBlack.SetActive(true);
        can.interactable = false;
        can.blocksRaycasts = false;
        can.alpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime * 4)
        {
            if (elapsedTime >= fadeTime * 4)
            {
                can.alpha = 0f;
                break;
            }
            can.alpha = Mathf.SmoothStep(1f, 0f, elapsedTime / (fadeTime * 4));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeoutBlack.SetActive(false);
    }

    public void ToSetting()
    {
        book_whole.SetActive(true);
        ToBookCam();
        book_title.SetActive(false);
        book_load.SetActive(false);
        book_credit.SetActive(false);
        book_setting.SetActive(true);
    }

    [Header("===== 6. ���� ���� & ���� �޴� ���â =====")]

    //���â
    public GameObject WarnBox_Q;
    //���� �޴� �ؽ�Ʈ
    public GameObject WarnBox_QtextM;
    //���� ���� �ؽ�Ʈ
    public GameObject WarnBox_QtextQ;

    //���â�� ������ִ���, � ���â���� Ȯ��
    private bool isEscWarning = false;
    private bool isEscWarning_Q = false;

    public void Debug_EscWarning_setQtrue()
    {
        isEscWarning_Q = true;
    }

    public void EscWarning()
    {
        WarnBox_Q.SetActive(true);
        isEscWarning = true;
        if (isEscWarning_Q)
        {
            WarnBox_QtextQ.SetActive(true);
        }
        else
        {
            WarnBox_QtextM.SetActive(true);
        }
    }

    public void EscWarning_YES()
    {
        if (isEscWarning_Q)
        {
            EscWarning_NO();
            QuitGame();
        }
        else
        {
            EscWarning_NO();
            TitleStart();
        }
    }

    public void EscWarning_NO()
    {
        isEscWarning = false;
        isEscWarning_Q = false;
        WarnBox_Q.SetActive(false);
        WarnBox_QtextM.SetActive(false);
        WarnBox_QtextQ.SetActive(false);
    }
}
