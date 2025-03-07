/*
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraWalker : MonoBehaviour
{
    //�ν����� ���� �⺻ ����
    //ī�޶� �̵� �ӵ� : 5, 5
    //���̵� �ƿ� �ð� : 1

    private void Start()
    {
        //===== ī�޶� �̵� ȿ�� =====
        //ī�޶� �̵� ������ �޾ƿ���
        titlePos = new Vector3(camPos_Title.transform.localPosition.x, camPos_Title.transform.localPosition.y, cam.transform.localPosition.z);
        lobbyPos = new Vector3(camPos_MainLobby.transform.localPosition.x, camPos_MainLobby.transform.localPosition.y, cam.transform.localPosition.z);
        bookPos = new Vector3(camPos_Book.transform.localPosition.x, camPos_Book.transform.localPosition.y, cam.transform.localPosition.z);

        //(�����)��ǥ Ȯ�ο�
        //Debug.Log(titlePos + ", " + lobbyPos + ", " + bookPos);

        //ī�޶� �̵� ������ �⺻��
        targetPos = cam.transform.localPosition;
        targetSize = cam.orthographicSize;

        //ó���� Ÿ��Ʋ���� ����
        inTitle = true;
        inLobby = false;
        inBook = false;

        //===== �����Ҷ� ���̵� ���� & ī�޶� ȿ�� =====
        can = fadeoutBlack.GetComponent<CanvasGroup>();
        canq = fadeoutBlack_QUIT.GetComponent<CanvasGroup>();
        fadeoutBlack_QUIT.SetActive(false);
        TitleStart();
    }

    private void Update()
    {
        //===== ī�޶� �̵� ȿ�� =====
        //ī�޶� �̵�
        cam.transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, camSpeed * Time.deltaTime);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);

        //Lerp �������� ������(���� �ǹ� ������)
        speedCheck1 = cam.velocity.sqrMagnitude;
        if (cam.velocity.sqrMagnitude <= 0.00001 && speedCheck1 < speedCheck2)
        {
            cam.transform.localPosition = targetPos;
        }
        speedCheck2 = speedCheck1;

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
            ToTitle();
        }
    }

    [Header("===== 1. ī�޶� �̵� ȿ�� =====")]

    //�ν����� ����
    [Header("ī�޶�")]
    public Camera cam;

    [Header("ī�޶� �̵� ������")]
    public GameObject camPos_Title;
    public GameObject camPos_MainLobby;
    public GameObject camPos_Book;

    [Header("ī�޶� �̵� �ӵ�(��� �̵�, Ȯ��/���)")]
    public float camSpeed;
    public float zoomSpeed;

    //ī�޶� �̵� ������ ��ǥ
    private Vector3 titlePos;
    private Vector3 lobbyPos;
    private Vector3 bookPos;

    //ī�޶� �̵� ������
    private Vector3 targetPos;
    private float targetSize;

    //���� � �޴��� �ִ��� Ȯ��
    private bool inTitle;
    private bool inLobby;
    private bool inBook;

    //ī�޶� �̵� ������ ����
    public void ToTitleCam()
    {
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
        targetSize = 5f;
        inTitle = true;
        inLobby = false;
        inBook = false;
    }

    public void ToLobbyCam()
    {
        camSpeed = 5f;
        targetPos = lobbyPos;
        targetSize = 16f;
        inTitle = false;
        inLobby = true;
        inBook = false;
    }

    public void ToBookCam()
    {
        camSpeed = 5f;
        targetPos = bookPos;
        targetSize = 16f;
        inTitle = false;
        inLobby = false;
        inBook = true;
    }

    //���� ������ Ÿ��Ʋ ī�޶� �̵�
    private float speedCheck1 = 0f, speedCheck2 = 0f;
    private float alphaCheck1 = 0f, alphaCheck2 = 0f;
    private bool titleIntro = false;
    public void TitleStart()
    {
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y - 150f, cam.transform.localPosition.z);
        titleIntro = true;
        ToTitleCam();
    }

    [Header("===== 2. ���̵� �ƿ� ���� =====")]

    //�ν����� ����
    [Header("���̵� �ƿ��� ������ �Ƕ���")]
    public GameObject fadeoutBlack;
    public GameObject fadeoutBlack_QUIT;

    [Header("���̵� �ƿ� �ð�")]
    public float fadeTime;

    //ȿ�� ������ ���� CanvasGroup ����
    private CanvasGroup can;
    private CanvasGroup canq;

    //���̵� ��
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
    IEnumerator FadeInEffect() //���� ���ľߵ�@@@@@@@@@@@@@@@@@@@@@
    {
        fadeoutBlack.SetActive(true);
        can.interactable = false;
        can.blocksRaycasts = false;
        can.alpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime * 500f)
        {
            can.alpha = Mathf.Lerp(can.alpha, 0f, elapsedTime / (fadeTime * 500f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeoutBlack.SetActive(false);
    }
    IEnumerator FadeInEffect_Title() //���� ó�� �����Ҷ��� ���̵���
    {
        fadeoutBlack.SetActive(true);
        can.interactable = false;
        can.blocksRaycasts = false;
        can.alpha = 1f;
        float elapsedTime = 0f;
        while (elapsedTime < (fadeTime * 1000f))
        {
            can.alpha = Mathf.Lerp(can.alpha, 0f, elapsedTime / (fadeTime * 1000f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        fadeoutBlack.SetActive(false);
    }

    //���̵� �ƿ�
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
    IEnumerator FadeOutEffect() //���� ���ľߵ�@@@@@@@@@@@@@@@@@@@@@
    {
        fadeoutBlack.SetActive(true);
        can.interactable = true;
        can.blocksRaycasts = true;
        can.alpha = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            can.alpha = Mathf.Lerp(can.alpha, 1f, elapsedTime / fadeTime * 100);
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
        while (elapsedTime < fadeTime / 4f)
        {
            canq.alpha = Mathf.MoveTowards(canq.alpha, 1f, elapsedTime / (fadeTime / 4f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    //���� ����
    private bool onGameQuit = false;
    public async void QuitGame()
    {
        StopAllCoroutines();
        onGameQuit = true;
        FadeOut();
        await Task.Delay(500);
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [Header("===== 3. Ÿ��Ʋ �޴��� ȭ�� ��ȯ =====")]

    //�ν����� ����
    [Header("å ������(ǥ�� ����)")]
    public GameObject book_whole;
    public GameObject book_title;
    public GameObject book_load;
    public GameObject book_credit;
    public GameObject book_setting;

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
    public void ToSetting()
    {
        book_whole.SetActive(true);
        ToBookCam();
        book_title.SetActive(false);
        book_load.SetActive(false);
        book_credit.SetActive(false);
        book_setting.SetActive(true);
    }
    public void ToTitle()
    {
        ToTitleCam();
        book_title.SetActive(true);
        book_load.SetActive(false);
        book_credit.SetActive(false);
        book_setting.SetActive(false);
        book_whole.SetActive(false);
    }
}
*/