using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chess.Piece;



public class CapturedPieceMenu : MonoBehaviour
{
    // Collapsea and Expand button
    public float collapsedPosition;
    public float moveSpeed;
    public Text buttonText;

    // Control elements
    private bool collaped = false;
    private bool moveCompleted = true;
    private RectTransform elementTransform;

    // New piece classes have to be defined here. Dictonaries cannot be initialized in the inspector sadly
    [System.Serializable]
    public struct MaterielValues
    {
        public int QueenValue;
        public int KnightValue;
        public int BishopValue;
        public int RookValue;
        public int PawnValue;
    }

    public MaterielValues pieceValues;

    // Stores numbe rof captured pieces
    private Dictionary<(PieceType, bool), uint> capturedValues = new Dictionary<(PieceType, bool), uint>
    {
        {(PieceType.Queen, true), 0 }, {(PieceType.Queen, false), 0},
        {(PieceType.Knight, true), 0 }, {(PieceType.Knight, false), 0},
        {(PieceType.Bishop, true), 0 }, {(PieceType.Bishop, false), 0},
        {(PieceType.Rook, true), 0 }, {(PieceType.Rook, false), 0},
        {(PieceType.Pawn, true), 0 }, {(PieceType.Pawn, false), 0}
    };

    // Text for display
    public Text WhiteCaptures;
    public Text BlackCaptures;

    // Materiel Elements
    public Image AdvantageW;
    public Image AdvantageB;
    public Text WhiteMaterielLossText;
    public Text BlackMaterielLossText;

    // Tracks lost materiel value
    private int WhiteLossCount = 0;
    private int BlackLossCount = 0;

    public int CriticalMaterielDelta;

    // Start is called before the first frame update
    void Start()
    {
        elementTransform = gameObject.GetComponent<RectTransform>();
        UpdateTable();
    }

    // Update is called once per frame
    void Update()
    {
        if (!moveCompleted)
        {
            if (collaped == true)
            {
                if (elementTransform.anchoredPosition.x < collapsedPosition)
                    elementTransform.Translate(new Vector3(moveSpeed * Time.deltaTime, 0f, 0f));
                else
                {
                    elementTransform.anchoredPosition = new Vector2(collapsedPosition, elementTransform.anchoredPosition.y);
                    moveCompleted = true;
                }
            }
            else
            {
                if (elementTransform.anchoredPosition.x > 0f)
                    elementTransform.Translate(new Vector3(moveSpeed * Time.deltaTime * -1, 0f, 0f));
                else
                {
                    elementTransform.anchoredPosition = new Vector2(0f, elementTransform.anchoredPosition.y);
                    moveCompleted = true;
                }
            }
        }
    }

    // Used by button
    public void CapturesCollapse()
    {
        collaped = !collaped;
        moveCompleted = false;
        buttonText.text = $"{ (collaped ? "<<" : ">>")}";

    }

    // Method to increment counts
    public void CapturedCounterIncrement(PieceType piece, bool isWhite)
    {
        // If it's the king the game is over so why bother
        if (piece == PieceType.King)
            return;
        int value = MaterielValue(piece);
        if (isWhite)
            WhiteLossCount += value;
        else
            BlackLossCount += value;
        capturedValues[(piece, isWhite)]++;
        UpdateTable();
    }

    // Tool to get materiel value of a piece type
    private int MaterielValue(PieceType piece)
    {
        switch (piece)
        {
            case PieceType.Queen:
                return pieceValues.QueenValue;

            case PieceType.Knight:
                return pieceValues.KnightValue;

            case PieceType.Bishop:
                return pieceValues.BishopValue;

            case PieceType.Rook:
                return pieceValues.RookValue;

            case PieceType.Pawn:
                return pieceValues.PawnValue;

            default:
                Debug.LogWarning("Piece of type " + piece + " not defined in materiel table");
                return 0;
        }
        
    }

    // Updates values displayed
    private void UpdateTable()
    {
        WhiteCaptures.text = UpdateList(true);
        BlackCaptures.text = UpdateList(false);
        WhiteMaterielLossText.text = "-" + WhiteLossCount.ToString();
        BlackMaterielLossText.text = "-" + BlackLossCount.ToString();
        UpdateAdvantageBar();
    }

    // Tool for tables
    private string UpdateList(bool isWhite)
    {
        string tempText = "";
        tempText += "x" + capturedValues[(PieceType.Queen, isWhite)].ToString() + "\n";
        tempText += "x" + capturedValues[(PieceType.Knight, isWhite)].ToString() + "\n";
        tempText += "x" + capturedValues[(PieceType.Bishop, isWhite)].ToString() + "\n";
        tempText += "x" + capturedValues[(PieceType.Rook, isWhite)].ToString() + "\n";
        tempText += "x" + capturedValues[(PieceType.Pawn, isWhite)].ToString() + "\n";

        return tempText;
    }

    // Fill, Rotate, and Color the advantage bar
    private void UpdateAdvantageBar()
    {
        float advantageSize = Mathf.Clamp((float)(Mathf.Abs(BlackLossCount - WhiteLossCount)) / (float)CriticalMaterielDelta, 0f, 1f);
        if (BlackLossCount > WhiteLossCount)
        {
            AdvantageW.color = Color.Lerp(Color.white, Color.green, advantageSize);
            AdvantageB.color = Color.Lerp(Color.white, Color.red, advantageSize);
            AdvantageW.fillAmount = 0.5f + advantageSize/2;
            AdvantageB.fillAmount = 0.5f - advantageSize / 2;
        }
            
        else
        {
            AdvantageW.color = Color.Lerp(Color.white, Color.red, advantageSize);
            AdvantageB.color = Color.Lerp(Color.white, Color.green, advantageSize);
            AdvantageW.fillAmount = 0.5f - advantageSize / 2;
            AdvantageB.fillAmount = 0.5f + advantageSize / 2;
        }
    }
}
