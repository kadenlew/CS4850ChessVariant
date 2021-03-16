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
    PieceMoveAttack,
    PieceSpecialAttack
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
    public List<GameObject> relevantTiles = new List<GameObject>();
    public HashSet<Chess.Definitions.Action> actionList = new HashSet<Chess.Definitions.Action>();

    private GameObject selectedBoard = null;

    private Chess.BoardController boardController;

    private bool transferMode = false;

    //false is black, true is white
    private bool turn = true;

    private bool tooltip = false;


    // Start is called before the first frame update
    void Start()
    {
        boardController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Chess.BoardController>();
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
            Confirm();
        }

        RayCastSelector();
    }

    // This moves around the UI and toggles on and off elements as the UI state changes
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
            confirmButton.SetActive(false);
            moveButton.SetActive(true);
            moveButton.transform.position = ButtonPositionToVector(true, 0);
            //attackButton.SetActive(true);
            //attackButton.transform.position = ButtonPositionToVector(true, 1);
            leadershipButton.SetActive(true);
            leadershipButton.transform.position = ButtonPositionToVector(true, 1);
            cancelButton.SetActive(true);
            cancelButton.transform.position = ButtonPositionToVector(true, 2);

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
        if (uiStatus == UIState.PieceMoveAttack)
        {
            if(selectedBoard)
            {
                informationPanel.SetActive(true);
                moveButton.SetActive(false);
                attackButton.SetActive(false);
                leadershipButton.SetActive(false);
                endTurnButton.SetActive(false);
                confirmButton.SetActive(true);
                confirmButton.transform.position = ButtonPositionToVector(true, 0);
                cancelButton.SetActive(true);
                cancelButton.transform.position = ButtonPositionToVector(true, 1);
            } 
            else
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

    // A tool for placing UI elements correctly
    private Vector3 ButtonPositionToVector (bool infoPanel, int index)
    {
        if(infoPanel)
        
            return new Vector3(infoPanelOffset + buttonOffset * (2+index) + buttonSize * index, buttonOffset, 0);
        else
            return new Vector3(buttonOffset * (1 + index) + buttonSize * index, buttonOffset, 0);
    }


    private void RayCastSelector()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectHit = hit.transform.gameObject;
                {
                    if (uiStatus == UIState.NoSelect || uiStatus == UIState.PieceMainSelect)
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
                    }
                    if (uiStatus == UIState.PieceMoveAttack && objectHit.gameObject.CompareTag("Board") && selected)
                    {
                        if (relevantTiles.Contains(objectHit.gameObject))
                        {
                            if (selectedBoard)
                                selectedBoard.GetComponent<Chess.Definitions.Tile>().Select();
                            selectedBoard = objectHit.gameObject;
                            selectedBoard.GetComponent<Chess.Definitions.Tile>().SelectMove();
                            UpdateUI();
                        }
                    }
                }
            }
        }
    }

    public void endTurn()
    {
        turn = !turn;
        UpdateUI();
    }

    // Used by UI Button
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

    // Used by UI button
    public void AttackMove()
    {
        uiStatus = UIState.PieceMoveAttack;
        selected.Explore(ref actionList);
        relevantPieces = GetRelevantMoveAttack(actionList);
        foreach (GameObject piece in relevantPieces)
        {
            piece.GetComponent<GamePieceBase>().Select(HighlightColors[1]);
        }
        relevantTiles = GetRelevantMoveTiles(actionList);
        foreach (GameObject tile in relevantTiles)
        {
            tile.GetComponent<Chess.Definitions.Tile>().Select();
        }
        UpdateUI();
    }

    // Used by UI button and hotkey
    public void Confirm()
    {
        if (uiStatus == UIState.PieceMoveAttack)
        {
            if (selectedBoard)
            {
                boardController.execute_action(
                    new Chess.Definitions.MoveAction(
                        selected.gameObject,
                        selectedBoard.GetComponent<Chess.Definitions.Tile>().position));

                uiStatus = UIState.NoSelect;
                DeselectAll();
                selected.Deselect();
                selected = null;
                UpdateUI();
            }
        }
    }


    // Used by UI button and hotkey
    public void Cancel()
    {
        if (uiStatus == UIState.PieceMainSelect)
        {
            if (selected)
                selected.Deselect();
            uiStatus = UIState.NoSelect;
            UpdateUI();
        }
        if (uiStatus == UIState.PieceLeadership || uiStatus == UIState.PieceMoveAttack)
        {
            DeselectAll();
            uiStatus = UIState.PieceMainSelect;
            UpdateUI();
        }

    }

    public void Tooltip()
    {
        tooltip = !tooltip;
        tooltipImage.SetActive(tooltip);
    }

    // Tool for quickly deselecting all objects. Note it doesn't deselect the selected piece
    private void DeselectAll()
    {
        foreach (GameObject piece in relevantPieces)
        {
            piece.GetComponent<GamePieceBase>().Deselect();
        }
        foreach (GameObject tile in relevantTiles)
        {
            tile.GetComponent<Chess.Definitions.Tile>().Deselect();
        }
        relevantPieces.Clear();
        relevantTiles.Clear();
        actionList.Clear();
        selectedBoard = null;
    }

    // Gets targets for attack/move
    private List<GameObject> GetRelevantMoveAttack(HashSet<Chess.Definitions.Action> localActions)
    {
        if (selected)
        {
            List<GameObject> targets = new List<GameObject>();    
            foreach (Chess.Definitions.Action action in localActions)
            {
                if(action is Chess.Definitions.AttackAction)
                {
                    Chess.Definitions.AttackAction attack = (Chess.Definitions.AttackAction)action;
                    targets.Add(attack.target);
                }
            }
            return targets;
        }
        return null;
    }

    // Gets move tiles for attack/move
    private List<GameObject> GetRelevantMoveTiles(HashSet<Chess.Definitions.Action> localActions)
    {
        if (selected)
        {
            List<GameObject> tiles = new List<GameObject>();
            foreach (Chess.Definitions.Action action in localActions)
            {
                if (action is Chess.Definitions.MoveAction)
                {
                    Chess.Definitions.MoveAction move = (Chess.Definitions.MoveAction)action;
                    tiles.Add(boardController.board_tiles[move.target]);
                }
                
            }
            return tiles;
        }


        return null;
    }

    // Gets leadership relevant to the piece
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
                CommanderPiece p = selected.GetComponent<CommanderPiece>();
                foreach (GameObject soldier in p.soldiers_)
                {
                    relatedPieces.Add(soldier);
                }
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
