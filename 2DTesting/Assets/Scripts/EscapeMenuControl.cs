using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenuControl : MonoBehaviour
{

    public PlayerEscape playerEscape;
    // Start is called before the first frame update
    void Start()
    {
        playerEscape = FindObjectOfType<PlayerController>().GetComponent<PlayerEscape>();
    }

    public void resume()
    {
        playerEscape.toggle();
    }

    public void quitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
