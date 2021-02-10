using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Chess.Definitions;

namespace Chess 
{
namespace Piece
{

public class GamePieceBase : MonoBehaviour
{
    public Material standard;
    public Material selected;

    protected Renderer targetRenderer;

    protected BoardPosition position_;

    // Start is called before the first frame update
    void Start()
    {
        targetRenderer = GetComponentInChildren<Renderer>();
        Deselect();

        position_ = new BoardPosition(1, 1);
        Debug.Log(position_);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        targetRenderer.material = selected;
    }

    public void Deselect()
    {
        targetRenderer.material = standard;
    }

    public void init(BoardPosition starting_position) {
        position_ = starting_position;
    }

}

}
}