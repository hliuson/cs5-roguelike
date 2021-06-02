using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEscape : MonoBehaviour
{
    [SerializeField]
    private GameObject Canvas;

    public bool paused;
    // Start is called before the first frame update
    void Start()
    {
        Canvas = GameObject.Find("Pause Canvas");
        Time.timeScale = 1.0f;
        paused = false;
        Canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggle();
        }
    }

    public void toggle()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        Canvas.gameObject.SetActive(paused);
    }
}
