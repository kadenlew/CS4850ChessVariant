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
    private UIState uiStatus = UIState.NoSelect;

    // This is defined at runtime
    private Chess.BoardController boardController;

    //As long as the numbers of colors don't change this shouldn't cause huge issues
    public Color[] HighlightColors;

    // Camera stuff
    public Transform cameraPivot;
    private Vector3 originalPosition;
    public GameObject CameraResetButton;

    // General Stuff
    public Image whiteTurn;
    public Image blackTurn;

    // Variables for adjusting UI
    public int buttonOffset;
    public int buttonSize;
    public int infoPanelOffset;

    // Bottom UI Buttons
    public GameObject informationPanel;
    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject leadershipButton;
    public GameObject confirmButton;
    public GameObject cancelButton;
    public GameObject endTurnButton;

    // Information Panel Buttons
    public Text infoPName;

    // DynamicPopUpTable
    public GameObject tableBase;
    public Text tableHeader;
    public Text tableRolls;

    // Internal vairables for selections and highlights
    private Chess.Definitions.Action gameAction_; 
    private GamePieceBase selected = null;
    private List<GameObject> relevantPieces = new List<GameObject>();
    private List<GameObject> relevantTiles = new List<GameObject>();
    private HashSet<Chess.Definitions.Action> actionList = new HashSet<Chess.Definitions.Action>();
    private GameObject selectedBoard = null;
    private GameObject targetPiece = null;

    

    //false is black, true is white
    private bool tooltip = false;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = cameraPivot.position;
        boardController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Chess.BoardController>();
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
        CheckCamera();
    }

    // This moves around the UI and toggles on and off elements as the UI state changes
    private void UpdateUI()
    {
        if(uiStatus == UIState.NoSelect)
        {
            tableBase.SetActive(false);
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
            if (selected)
                infoPName.text = selected.type.ToString();
            if (tooltip)
            {
                tableBase.SetActive(true);
                PopulateAttackTable();
            }
            else
                tableBase.SetActive(false);
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
            if (tooltip)
            {
                tableBase.SetActive(true);
                PopulateAttackTable();
            }
            else
                tableBase.SetActive(false);
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
            if (tooltip)
            {
                tableBase.SetActive(true);
                PopulateAttackTable();
            }
            else
                tableBase.SetActive(false);
            if (selectedBoard || targetPiece)
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


        if (boardController.is_white_turn)
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

    // Checks if the camera has left the center of the board
    private void CheckCamera()
    {
        if (Vector3.Distance(cameraPivot.position, originalPosition) > 0.001f)
        {
            CameraResetButton.SetActive(true);
        }
        else
        {
            CameraResetButton.SetActive(false);
        }
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
                        if (objectHit.gameObject.CompareTag("Player") && objectHit.gameObject.GetComponent<GamePieceBase>().is_white == boardController.is_white_turn)
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
                            if (targetPiece)
                            {
                                targetPiece.GetComponent<GamePieceBase>().Select(HighlightColors[1]);
                                targetPiece = null;
                            }
                            if (selectedBoard)
                                selectedBoard.GetComponent<Chess.Definitions.Tile>().Select();
                            selectedBoard = objectHit.gameObject;
                            selectedBoard.GetComponent<Chess.Definitions.Tile>().SelectMove();
                            UpdateUI();
                        }
                    }
                    if (uiStatus == UIState.PieceMoveAttack && objectHit.gameObject.CompareTag("Player") && selected)
                    {
                        if (relevantPieces.Contains(objectHit.gameObject))
                        {
                            if (selectedBoard)
                            {
                                selectedBoard.GetComponent<Chess.Definitions.Tile>().Select();
                                selectedBoard = null;
                            }
                            if (targetPiece)
                                targetPiece.GetComponent<GamePieceBase>().Select(HighlightColors[1]);
                            targetPiece = objectHit.gameObject;
                            targetPiece.GetComponent<GamePieceBase>().Select(HighlightColors[3]);
                            UpdateUI();
                        }
                    }
                }
            }
        }
    }

    public void endTurn()
    {
        boardController.end_turn();
        UpdateUI();
    }

    private void PopulateAttackTable()
    {
        tableHeader.text = selected.type.ToString() + "'s Attack Rolls";
        string display = "";
        display += RollsForPiece(selected.type, Chess.Piece.PieceType.King) + "\n";
        display += RollsForPiece(selected.type, Chess.Piece.PieceType.Queen) + "\n";
        display += RollsForPiece(selected.type, Chess.Piece.PieceType.Knight) + "\n";
        display += RollsForPiece(selected.type, Chess.Piece.PieceType.Bishop) + "\n";
        display += RollsForPiece(selected.type, Chess.Piece.PieceType.Rook) + "\n";
        display += RollsForPiece(selected.type, Chess.Piece.PieceType.Pawn) + "\n";
        tableRolls.text = display;

    }

    private string RollsForPiece(Chess.Piece.PieceType attacker, Chess.Piece.PieceType defender)
    {
        int tableRoll = 0;
        tableRoll = Chess.Definitions.AttackAction.captureTable[(attacker, defender)];
        if (tableRoll == 1)
            return "Automatic";
        else
        {
            string temp = "6";
            for (int i = 5; i >= tableRoll; i--)
            {
                temp += "," + i.ToString();
            }
            return temp;
        }

        return null;
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
        actionList = boardController.get_piece_actions(selected.gameObject);
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
            if (selectedBoard || targetPiece)
            {
                if (selectedBoard)
                    boardController.execute_action(
                        new Chess.Definitions.MoveAction(
                            selected.gameObject,
                            selectedBoard.GetComponent<Chess.Definitions.Tile>().position));

                if(targetPiece)
                    boardController.execute_action(
                        new Chess.Definitions.AttackAction(
                            selected.gameObject,
                            targetPiece));
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
        UpdateUI();
    }

    public void ResetCameraButton()
    {
        cameraPivot.position = originalPosition;
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
        targetPiece = null;
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
