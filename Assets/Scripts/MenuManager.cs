using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnStart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    public void OnSettings(){
        Debug.Log("Do Settings");
    }

    public void OnQuit(){
        Debug.Log("Quit");
        Application.Quit();
    }
}
