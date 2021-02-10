using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public int dimensions = 8;
    public float pieceSize = 2f;
    public GameObject plane;
    public Material[] BoardMaterials;

    public GameObject King;
    public GameObject Queen;
    public GameObject Bishop;
    public GameObject Knight;
    public GameObject Rook;
    public GameObject Pawn;
    public Material[] PieceMaterials;
    public Material[] PieceSelected;


    // Start is called before the first frame update
    void Start()
    {
        InitializeBoard();
        InitializePieces();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeBoard()
    {
        int cycler = 0;
        for (int i = 0; i < dimensions; i++)
        {
            for (int k = 0; k < dimensions; k++)
            {
                GameObject p = Instantiate(plane);
                p.transform.position = PositionHelper(k, i);
                Renderer target = p.GetComponent<Renderer>();
                target.material = BoardMaterials[cycler];
                cycler++;
                if (cycler == BoardMaterials.Length)
                    cycler = 0;
            }
            cycler--;
            if (cycler < 0)
                cycler = BoardMaterials.Length - 1;
        }
    }

    private void InitializePieces()
    {
        //white pawns
        for (int i = 0; i < 8; i++)
        {
            GameObject p = CreatePiece(Pawn, 1);
            p.transform.position = PositionHelper(i, 1);
        }

        //white rooks
        {
            int offset = 0;
            for (int i = 0; i < 2; i++)
            {
                GameObject p = CreatePiece(Rook, 1);
                p.transform.position = PositionHelper(offset, 0);
                offset += 7;
            }
        }

        //white knights
        {
            int offset = 1;
            for (int i = 0; i < 2; i++)
            {
                GameObject p = CreatePiece(Knight, 1);
                p.transform.position = PositionHelper(offset, 0);
                p.transform.rotation = Quaternion.Euler(0, 180, 0);
                offset += 5;
            }
        }

        //white bishop
        {
            int offset = 2;
            for (int i = 0; i < 2; i++)
            {
                GameObject p = CreatePiece(Bishop, 1);
                p.transform.position = PositionHelper(offset, 0);
                offset += 3;
            }
        }

        //white queen
        {
            GameObject p = CreatePiece(Queen, 1);
            p.transform.position = PositionHelper(3, 0);
        }

        //white king
        {
            GameObject p = CreatePiece(King, 1);
            p.transform.position = PositionHelper(4, 0);
        }

        //black pawns
        for (int i = 0; i < 8; i++)
        {
            GameObject p = CreatePiece(Pawn, 0);
            p.transform.position = PositionHelper(i, 6);
        }

        //black rooks
        {
            int offset = 0;
            for (int i = 0; i < 2; i++)
            {
                GameObject p = CreatePiece(Rook, 0);
                p.transform.position = PositionHelper(offset, 7);
                offset += 7;
            }
        }

        //black knights
        {
            int offset = 1;
            for (int i = 0; i < 2; i++)
            {
                GameObject p = CreatePiece(Knight, 0);
                p.transform.position = PositionHelper(offset, 7);
                offset += 5;
            }
        }

        //black bishop
        {
            int offset = 2;
            for (int i = 0; i < 2; i++)
            {
                GameObject p = CreatePiece(Bishop, 0);
                p.transform.position = PositionHelper(offset, 7);
                offset += 3;
            }
        }

        //black queen
        {
            GameObject p = CreatePiece(Queen, 0);
            p.transform.position = PositionHelper(3, 7);
        }

        //black king
        {
            GameObject p = CreatePiece(King, 0);
            p.transform.position = PositionHelper(4, 7);
        }

    }

    private GameObject CreatePiece(GameObject type, int materialIndex)
    {
        GameObject p = Instantiate(type);
        GamePieceBase target = p.GetComponent<GamePieceBase>();
        target.standard = PieceMaterials[materialIndex];
        target.selected = PieceSelected[materialIndex];
        return p;
    }

    private Vector3 PositionHelper(int x, int y)
    {
        if (x < 0)
            Debug.LogWarning("X less than 0");
        if (y < 0)
            Debug.LogWarning("Y less than 0");
        if (x > dimensions - 1)
            Debug.LogWarning("X greater than board size");
        if (y > dimensions - 1)
            Debug.LogWarning("Y greater than board size");
        float offset = -1 * pieceSize * (dimensions / 2) + pieceSize / 2;
        return new Vector3(x * pieceSize + offset, 0, y * pieceSize + offset);
    }
}
