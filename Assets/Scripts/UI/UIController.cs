using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Chess.Piece;
using UnityEngine.EventSystems;

enum UIState
{
    NoSelect,
    EnemySelect,
    PieceMainSelect,
    PieceLeadership,
    PieceMoveAttack,
    PieceSpecialAttack
}


public class UIController : MonoBehaviour
{
    private UIState uiStatus = UIState.NoSelect;
    public bool UIEnabled = true;

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

    // Information Panel Stuff
    public Text infoPName;
    public Text infoPTurns;
    public Text infoPKills;
    public Text infoPSurvival;
    private Dictionary<GamePieceBase, Veterancy> pieceVeterancy = new Dictionary<GamePieceBase, Veterancy>();


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

    // Other UI
    private CapturedPieceMenu capturedUI;
    private bool tooltip = false;

    // Hover Probabilities
    private List<HoverUI> floatingText = new List<HoverUI>();
    public GameObject floatingTextPrefab;

    // Settings gizmos
    private bool menuOpen = false;
    public GameObject menuPanel;

    public Toggle probabilityCheck;
    public Toggle animationPlay;
    public Toggle aiPause;
    public Slider aiMoveSpeed;
    public Text aiMoveSpeedText;

    // Animation controller
    public BezierMovement bezierMover;
    
    // Start is called before the first frame update
    void Start()
    {
        capturedUI = gameObject.GetComponentInChildren<CapturedPieceMenu>();
        originalPosition = cameraPivot.position;
        boardController = GameObject.FindGameObjectWithTag("GameController").GetComponent<Chess.BoardController>();
        probabilityCheck.isOn = Settings.dynamicProbabilities;
        aiMoveSpeed.value = Settings.aiDelaySlider;
        aiPause.isOn = !Settings.aiEnabled;
        Debug.Log(Settings.dynamicProbabilities.ToString());
        animationPlay.isOn = Settings.playAnimations;
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

        // Prevents clicking through UI
        if (!EventSystem.current.IsPointerOverGameObject() && UIEnabled)
            RayCastSelector();
        CheckCamera();

        // temperary fix
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

        // This needs to be a lot more robust
        UIEnabled = !bezierMover.animating;
    }

    // This moves around the UI and toggles on and off elements as the UI state changes
    private void UpdateUI()
    {
        switch (uiStatus)
        {
            case UIState.NoSelect:
                tableBase.SetActive(false);
                informationPanel.SetActive(false);
                moveButton.SetActive(false);
                attackButton.SetActive(false);
                leadershipButton.SetActive(false);
                confirmButton.SetActive(false);
                cancelButton.SetActive(false);
                if (IsPlayerControllable(boardController.is_white_turn))
                    endTurnButton.SetActive(true);
                else
                    endTurnButton.SetActive(false);
                endTurnButton.transform.position = ButtonPositionToVector(false, 0);
                break;

            case UIState.PieceMainSelect:
                if (selected)
                {
                    SetInfoPanel(selected);
                }
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
                leadershipButton.SetActive(true);
                leadershipButton.transform.position = ButtonPositionToVector(true, 1);
                cancelButton.SetActive(true);
                cancelButton.transform.position = ButtonPositionToVector(true, 2);
                endTurnButton.SetActive(false);
                break;

            case UIState.PieceLeadership:
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
                break;

            case UIState.PieceMoveAttack:
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
                break;

            case UIState.EnemySelect:
                if (selected)
                {
                    SetInfoPanel(selected);
                }
                if (tooltip)
                {
                    tableBase.SetActive(true);
                    PopulateAttackTable();
                }
                else
                    tableBase.SetActive(false);
                informationPanel.SetActive(true);
                confirmButton.SetActive(false);
                moveButton.SetActive(false);
                leadershipButton.SetActive(false);
                cancelButton.SetActive(true);
                cancelButton.transform.position = ButtonPositionToVector(true, 0);
                endTurnButton.SetActive(false);
                break;

            default:
                Debug.LogError("Unrecognized UI State");
                break;
        }

        if (menuOpen)
            menuPanel.SetActive(true);
        else
            menuPanel.SetActive(false);
    }

    // A tool for placing UI elements correctly
    private Vector3 ButtonPositionToVector(bool infoPanel, int index)
    {
        if (infoPanel)

            return new Vector3(infoPanelOffset + buttonOffset * (2 + index) + buttonSize * index, buttonOffset, 0);
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

    // Called every frame to see if player is clicking on things
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
                    if (uiStatus == UIState.NoSelect || uiStatus == UIState.PieceMainSelect || uiStatus == UIState.EnemySelect)
                    {
                        if (objectHit.gameObject.CompareTag("Player"))
                        {
                            if (objectHit.gameObject.GetComponent<GamePieceBase>().is_white == boardController.is_white_turn && IsPlayerControllable(boardController.is_white_turn))
                            {
                                if (selected)
                                {
                                    if (PieceHasTurn(selected) > 0)
                                        selected.Deselect();
                                    else
                                        selected.Select(HighlightColors[4]);
                                }
                                selected = objectHit.GetComponent<GamePieceBase>();
                                selected.Select(HighlightColors[0]);
                                uiStatus = UIState.PieceMainSelect;
                                UpdateUI();
                            }
                            else
                            {
                                if (selected)
                                {
                                    if (PieceHasTurn(selected) > 0)
                                        selected.Deselect();
                                    else
                                        selected.Select(HighlightColors[4]);
                                }
                                selected = objectHit.GetComponent<GamePieceBase>();
                                selected.Select(HighlightColors[3]);
                                uiStatus = UIState.EnemySelect;
                                UpdateUI();
                            }
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
            else
            {
                if(selected)
                {
                    Cancel();
                }
            }
        }
    }

    // Used by button only for now
    public void endTurn()
    {
        boardController.end_turn();
        UpdateUI();
        selected = null;
    }

    // Sets UI with Dynamic Probabilities
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

    // Tool to determine if a piece has a turn
    private uint PieceHasTurn(GamePieceBase piece)
    {
        if (piece is CommanderPiece)
        {
            CommanderPiece temp = (CommanderPiece)piece;
            return temp.energy;

        }
        if (piece is SoldierPiece)
        {
            SoldierPiece temp = (SoldierPiece)piece;
            return temp.commander.GetComponent<CommanderPiece>().energy;
        }

        Debug.LogError("seleteced piece is nothing?");
        return 0;


    }

    // Calculates probabilities
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
        actionList = boardController.get_piece_actions(selected);
        if (actionList != null)
        {
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
        }
        if (probabilityCheck.isOn)
            CreateFloatingProbability();
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
                {
                    boardController.setPositions = false;
                    boardController.execute_action(
                        new Chess.Definitions.MoveAction(
                            selected,
                            selectedBoard.GetComponent<Chess.Definitions.Tile>().position));
                }

                if (targetPiece)
                {
                    PieceType tempType = targetPiece.GetComponent<GamePieceBase>().type;
                    boardController.execute_action(
                        new Chess.Definitions.AttackAction(
                            selected,
                            targetPiece.GetComponent<GamePieceBase>()));
                }

                uiStatus = UIState.PieceMainSelect;
                DeselectAll();
                if (selected is CommanderPiece)
                {
                    CommanderPiece temp = (CommanderPiece)(selected);
                    if (temp.energy == 0)
                    {
                        foreach (Chess.Piece.SoldierPiece soldier in temp.soldiers_)
                        {
                            soldier.Select(HighlightColors[4]);
                        }
                        temp.Select(HighlightColors[4]);
                    }
                }
                else if (selected is SoldierPiece)
                {
                    SoldierPiece temp = (SoldierPiece)selected;
                    if (temp.commander.energy == 0)
                    {
                        foreach (Chess.Piece.SoldierPiece soldier in temp.commander.soldiers_)
                        {
                            soldier.Select(HighlightColors[4]);
                        }
                        temp.commander.Select(HighlightColors[4]);
                    }
                }
                selected.Select(HighlightColors[0]);
                UpdateUI();
            }
        }
    }

    // Used by UI button and hotkey
    public void Cancel()
    {
        if (uiStatus == UIState.PieceMainSelect || uiStatus == UIState.EnemySelect)
        {
            if (selected)
            {
                if (PieceHasTurn(selected) > 0)
                    selected.Deselect();
                else
                    selected.Select(HighlightColors[4]);
            }
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

    public void MenuButton()
    {
        menuOpen = !menuOpen;
        UpdateUI();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(0);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitProgramButton()
    {
        Application.Quit();
        Debug.Log("Game Ended through ingame menu");
    }


    public void HandleCombatResult(Chess.Definitions.Result result, GamePieceBase victor)
    {
        if (result is Chess.Definitions.AttackResult)
        {
            Chess.Definitions.AttackResult attackResult = (Chess.Definitions.AttackResult)result;
            if (attackResult.was_successful)
            {
                ChangeVeterancy(victor, 1, 0);
                capturedUI.CapturedCounterIncrement(attackResult.targetType, !boardController.is_white_turn);
            }
            else
                ChangeVeterancy(victor, 0, 1);
        }
    }

    // Tool for quickly deselecting all objects. Note it doesn't deselect the selected piece
    private void DeselectAll()
    {
        foreach (GameObject piece in relevantPieces)
        {
            if (PieceHasTurn(piece.GetComponent<GamePieceBase>()) > 0)
                piece.GetComponent<GamePieceBase>().Deselect();
            else
                piece.GetComponent<GamePieceBase>().Select(HighlightColors[4]);
        }
        foreach (GameObject tile in relevantTiles)
        {
            tile.GetComponent<Chess.Definitions.Tile>().Deselect();
        }
        DestroyFloatingText();
        relevantPieces.Clear();
        relevantTiles.Clear();
        selectedBoard = null;
        targetPiece = null;
    }

    // Produces floating text for each piece in the list of relevant pieces compated to selected
    private void CreateFloatingProbability()
    {
        if (selected)
        {
            foreach (GameObject targetPiece in relevantPieces)
            {
                GameObject I = Instantiate(floatingTextPrefab, transform);
                HoverUI IScript = I.GetComponent<HoverUI>();
                floatingText.Add(IScript);
                IScript.SetText(WinProbability(selected.type, targetPiece.GetComponent<GamePieceBase>().type));
                IScript.target = new Vector3(targetPiece.transform.position.x, 1f, targetPiece.transform.position.z);
                IScript.adapt = true;
            }
        }
    }

    // Destroys all floating text elements
    private void DestroyFloatingText()
    {
        foreach (HoverUI element in floatingText)
        {
            Destroy(element.gameObject);
        }
        floatingText.Clear();
    }

    // For updating when toggle is switched
    public void ProbabilityToggleChanged()
    {
        Settings.dynamicProbabilities = probabilityCheck.isOn;
        if (probabilityCheck.isOn)
        {
            if (uiStatus == UIState.PieceMoveAttack || uiStatus == UIState.PieceSpecialAttack)
                CreateFloatingProbability();
        }
        else
            DestroyFloatingText();
    }

    public void AinmationToggleChanged()
    {
        Settings.playAnimations = animationPlay.isOn;
    }

    public void AIEnabledToggleChanged()
    {
        Settings.aiEnabled = !aiPause.isOn;
    }

    public void SpeedSliderChanged()
    {
        Settings.aiDelaySlider = aiMoveSpeed.value;
        aiMoveSpeedText.text = (Settings.aiDelaySlider * 0.25f).ToString();
        boardController.UpdateAIDelay(Settings.aiDelaySlider * 0.25f);
        if (Settings.aiDelaySlider == 4)
            aiMoveSpeedText.text += (" Second");
        else
            aiMoveSpeedText.text += (" Seconds");
    }

    // Creates string of the probability
    private string WinProbability(PieceType attacker, PieceType defender)
    {
        int tableRoll = 0;
        tableRoll = Chess.Definitions.AttackAction.captureTable[(attacker, defender)];

        // invert
        tableRoll = (7 - tableRoll);
        return ((int)((float)tableRoll / 6f * 100)).ToString() + "%";

    }

    // Gets targets for attack/move
    private List<GameObject> GetRelevantMoveAttack(HashSet<Chess.Definitions.Action> localActions)
    {
        if (selected && localActions != null)
        {
            List<GameObject> targets = new List<GameObject>();    
            foreach (Chess.Definitions.Action action in localActions)
            {
                if(action is Chess.Definitions.AttackAction)
                {
                    Chess.Definitions.AttackAction attack = (Chess.Definitions.AttackAction)action;
                    targets.Add(attack.target.gameObject);
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
                    tiles.Add(boardController.board_tiles[move.target].gameObject);
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
                relatedPieces.Add(selected.GetComponent<SoldierPiece>().commander.gameObject);
            }
            else if(selected.GetComponent<CommanderPiece>())
            {
                CommanderPiece p = selected.GetComponent<CommanderPiece>();
                foreach (Chess.Piece.SoldierPiece soldier in p.soldiers_)
                {
                    relatedPieces.Add(soldier.gameObject);
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

    // Sets information in the info panel
    private void SetInfoPanel(GamePieceBase piece)
    {
        Veterancy tempVet = GetVeterancy(piece);
        infoPName.text = piece.type.ToString();
        infoPTurns.text = PieceHasTurn(piece).ToString();
        infoPKills.text = tempVet.kills.ToString();
        infoPSurvival.text = tempVet.survival.ToString();
    }

    // Gets or creates a veterancy for the proper piece
    private Veterancy GetVeterancy(GamePieceBase piece)
    {
        try
        {
            return pieceVeterancy[piece];
        }
        catch (KeyNotFoundException)
        {
            pieceVeterancy.Add(piece, new Veterancy(piece));
            return pieceVeterancy[piece];
        }

    }

    // Changes a pieces veterancy kills or survival
    private void ChangeVeterancy(GamePieceBase piece, uint deltaKill, uint deltaSurvive)
    {
        Veterancy tempVeterancy = GetVeterancy(piece);
        tempVeterancy.kills += deltaKill;
        tempVeterancy.survival += deltaSurvive;
    }

    // Tool for selection
    private bool IsPlayerControllable(bool whitePlayer)
    {
        if (whitePlayer)
            return !Settings.player1AI;
        else
            return !Settings.player2AI;
    }

    public void ForceUIUpdate()
    {
        UpdateUI();
    }
}
