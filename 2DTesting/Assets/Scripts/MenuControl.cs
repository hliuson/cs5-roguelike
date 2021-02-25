using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

    public void quitToMenu()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void Update()
    {
        
    }
}
