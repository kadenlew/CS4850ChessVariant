using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject settings;
    public TMPro.TextMeshProUGUI player1;
    public TMPro.TextMeshProUGUI player2;

    void Start()
    {
        player1.text = PlayerButtonText(Settings.player1AI);
        player2.text = PlayerButtonText(Settings.player2AI);
    }

    public void OnStart() {
        SceneManager.LoadScene(0);
    }
    public void OnSettings() {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void OnQuit() {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void OnBack()
    {
        menu.SetActive(true);
        settings.SetActive(false);
    }

    public void OnPlayer1()
    {
        Settings.player1AI = !Settings.player1AI;
        player1.text = PlayerButtonText(Settings.player1AI);
    }

    public void OnPlayer2()
    {
        Settings.player2AI = !Settings.player2AI;
        player2.text = PlayerButtonText(Settings.player2AI);
    }


    public string PlayerButtonText(bool aiControl)
    {
        if (aiControl)
            return "AI";
        else
            return "Human";
    }
}
