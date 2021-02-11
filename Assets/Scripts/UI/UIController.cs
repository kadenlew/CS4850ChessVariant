using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject transferNotification;
    public GameObject confirmButton;
    public Selector selector;


    Chess.Definitions.Action gameAction_;


    public bool activeAction = false;

    private bool transferMode = false;


    // Start is called before the first frame update
    void Start()
    {
        transferNotification.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        confirmButton.SetActive(activeAction);
    }

    public void transferButton()
    {
        transferMode = !transferMode;
        transferNotification.SetActive(transferMode);
        selector.moveAllowed = !transferMode;
    }

    public void confirmAction()
    {
        selector._TempMovePiece();
    }
}
