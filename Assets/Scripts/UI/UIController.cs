using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chess.Piece;

enum UIState
{
    NoSelect,
    PieceMainSelect,
    PieceLeadership,
    PieceMove,
    PieceAttack
}


public class UIController : MonoBehaviour
{
    //As long as the numbers of colors don't change this shouldn't cause huge issues
    public Color[] HighlightColors;

    public GameObject transferNotification;
    
    public GameObject tooltipImage;
    public Image whiteTurn;
    public Image blackTurn;

    public int buttonOffset;
    public int buttonSize;
    public int infoPanelOffset;

    public GameObject informationPanel;
    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject leadershipButton;
    public GameObject confirmButton;
    public GameObject cancelButton;
    public GameObject endTurnButton;


    Chess.Definitions.Action gameAction_;

    private UIState uiStatus = UIState.NoSelect;
    private bool activeAction = false;
    private GamePieceBase selected = null;
    public List<GameObject> relevantPieces = new List<GameObject>();
    private GameObject selectedBoard = null;


    private bool transferMode = false;

    //false is black, true is white
    private bool turn = true;

    private bool tooltip = false;


    // Start is called before the first frame update
    void Start()
    {
        //transferNotification.SetActive(false);
        tooltipImage.SetActive(false);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && cancelButton.activeSelf)
        {
            Cancel();
        }

        if (Input.GetButtonDown("Jump") && confirmButton.activeSelf)
        {

        }

        RayCastSelector();
        confirmButton.SetActive(activeAction);
    }

    private void UpdateUI()
    {
        if(uiStatus == UIState.NoSelect)
        {
            informationPanel.SetActive(false);
            moveButton.SetActive(false);
            attackButton.SetActive(false);
            leadershipButton.SetActive(false);
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
            endTurnButton.SetActive(true);
            endTurnButton.transform.position = ButtonPositionToVector(false, 0);
        }
        if(uiStatus == UIState.PieceMainSelect)
        {
            informationPanel.SetActive(true);
            moveButton.SetActive(true);
            moveButton.transform.position = ButtonPositionToVector(true, 0);
            attackButton.SetActive(true);
            attackButton.transform.position = ButtonPositionToVector(true, 1);
            leadershipButton.SetActive(true);
            leadershipButton.transform.position = ButtonPositionToVector(true, 2);
            cancelButton.SetActive(true);
            cancelButton.transform.position = ButtonPositionToVector(true, 3);

            endTurnButton.SetActive(false);
        }
        if (uiStatus == UIState.PieceLeadership)
        {
            informationPanel.SetActive(true);
            moveButton.SetActive(false);
            attackButton.SetActive(false);
            leadershipButton.SetActive(false);
            confirmButton.SetActive(false);
            endTurnButton.SetActive(false);
            cancelButton.SetActive(true);
            cancelButton.transform.position = ButtonPositionToVector(true, 0);
        }



        if (turn)
        {
            whiteTurn.color = new Color(whiteTurn.color.r, whiteTurn.color.g, whiteTurn.color.b, 1f);
            blackTurn.color = new Color(blackTurn.color.r, blackTurn.color.g, blackTurn.color.b, 0f);
        }
        else
        {
            whiteTurn.color = new Color(whiteTurn.color.r, whiteTurn.color.g, whiteTurn.color.b, 0f);
            blackTurn.color = new Color(blackTurn.color.r, blackTurn.color.g, blackTurn.color.b, 1f);
        }
    }

    private Vector3 ButtonPositionToVector (bool infoPanel, int index)
    {
        if(infoPanel)
        
            return new Vector3(infoPanelOffset + buttonOffset * (2+index) + buttonSize * index, buttonOffset, 0);
        else
            return new Vector3(buttonOffset * (1 + index) + buttonSize * index, buttonOffset, 0);
    }

    private void RayCastSelector()
    {
        if (Input.GetMouseButtonDown(0) && ((uiStatus == UIState.NoSelect) || (uiStatus == UIState.PieceMainSelect)))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectHit = hit.transform.gameObject;
                {
                    if (objectHit.gameObject.CompareTag("Player") && objectHit.gameObject.GetComponent<GamePieceBase>().is_white == turn)
                    {
                        if (selected)
                            selected.Deselect();

                        selected = objectHit.GetComponent<GamePieceBase>();
                        selected.Select(HighlightColors[0]);
                        uiStatus = UIState.PieceMainSelect;
                        UpdateUI();
                    }
                    //if (moveAllowed && objectHit.gameObject.CompareTag("Board") && selected)
                    //{
                    //    selectedBoard = objectHit.gameObject;
                    //    uiController.activeAction = true;
                    //}
                }
            }
            else
            {
                //if (selected)
                //    selected.Deselect();
                //selected = null;
                //selectedBoard = null;
                //uiController.activeAction = false;
            }

        }
    }

    public void endTurn()
    {
        turn = !turn;
        UpdateUI();
    }

    public void Leadership()
    {
        uiStatus = UIState.PieceLeadership;
        relevantPieces = GetRelevantLeadership();
        foreach (GameObject piece in relevantPieces)
        {
            piece.GetComponent<GamePieceBase>().Select(HighlightColors[2]);
        }
        UpdateUI();
    }

    public void Cancel()
    {
        if (uiStatus == UIState.PieceMainSelect)
        {
            if (selected)
                selected.Deselect();
            uiStatus = UIState.NoSelect;
            UpdateUI();
        }
        if (uiStatus == UIState.PieceLeadership)
        {
            foreach (GameObject piece in relevantPieces)
            {
                piece.GetComponent<GamePieceBase>().Deselect();
            }
            relevantPieces.Clear();
            uiStatus = UIState.PieceMainSelect;
            UpdateUI();
        }

    }

    public void Tooltip()
    {
        tooltip = !tooltip;
        tooltipImage.SetActive(tooltip);
    }

    private List<GameObject> GetRelevantLeadership()
    {
        if(selected)
        {
            List<GameObject> relatedPieces = new List<GameObject>();
            if (selected.GetComponent<SoldierPiece>())
            {
                relatedPieces.Add(selected.GetComponent<SoldierPiece>().commander);
            }
            else if(selected.GetComponent<CommanderPiece>())
            {
                // Cannot implement until I can access the list commanders have
                CommanderPiece p = selected.GetComponent<CommanderPiece>();
                Debug.Log("Feature not implemented yet");
            }
            else
            {
                Debug.LogError("A Piece is currently neither a commander nor a soldier!");
            }

            return relatedPieces;
        }
        return null;
    }
}
