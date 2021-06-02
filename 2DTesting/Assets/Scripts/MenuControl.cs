using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public GameObject helpScreen;
    // Start is called before the first frame update
    private void Start()
    {
        helpScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.anyKey) {
            helpScreen.SetActive(false);
        }
    }
    public void exitGame()
    {
        Application.Quit();
    }
    // Update is called once per frame
    public void test()
    {
        Debug.Log("Pressed");
    }

    public void showHelp()
    {
        helpScreen.SetActive(true);
    }

    public void startGame()
    {
        SceneManager.LoadScene("FirstLevel");
    }
}
