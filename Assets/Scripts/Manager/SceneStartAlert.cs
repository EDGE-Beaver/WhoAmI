using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SceneStartAlert : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject AlertPanel;
    public GameObject Panel;
    public List<GameObject> Text = new List<GameObject>();
    public GameObject button;
    // Start is called before the first frame update
    void Awake()
    {
       AlertPanel.SetActive(true);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void buttonClick(){
        Time.timeScale = 1.0f;
        AlertPanel.SetActive(false);
        isGamePaused = false;
    }

}
