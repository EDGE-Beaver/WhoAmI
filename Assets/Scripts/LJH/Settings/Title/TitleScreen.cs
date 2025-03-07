using UnityEngine;
using UnityEngine.SceneManagement; // �� ��ȯ�� ���� �ʿ�

public class TitleScreen : MonoBehaviour
{
    public string nextSceneName = "Main"; // �Ѿ �� �̸�

    private bool isKeyPressed = false; // Ű �Է� Ȯ�� �÷���

    void Update()
    {
        // �ƹ� Ű�� ������ ��
        if (Input.anyKeyDown && !isKeyPressed)
        {
            isKeyPressed = true; // �ߺ� �Է� ����
            StartGame(); // ���� ����
        }
    }

    void StartGame()
    {
        Debug.Log("��");

        // ���� ������ �̵�
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("�� �̸� �����߳�?");
        }
    }
}
