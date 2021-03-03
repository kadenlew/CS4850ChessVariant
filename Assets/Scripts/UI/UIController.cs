using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject transferNotification;
    public GameObject confirmButton;
    public GameObject tooltipImage;
    public Selector selector;
    public TMP_Text turnText;


    Chess.Definitions.Action gameAction_;


    public bool activeAction = false;

    private bool transferMode = false;

    //false is black, true is white
    private bool turn = true;

    private bool tooltip = false;


    // Start is called before the first frame update
    void Start()
    {
        transferNotification.SetActive(false);
        tooltipImage.SetActive(false);
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

    public void endTurn()
    {
        turn = !turn;
        if(turn)
        {
            turnText.text = "White's Turn";
        }
        else
        {
            turnText.text = "Black's Turn";
        }
    }

    public void Tooltip()
    {
        tooltip = !tooltip;
        tooltipImage.SetActive(tooltip);
    }
}
