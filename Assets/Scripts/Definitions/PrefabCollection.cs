using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

[System.Serializable]
public class PrefabCollection 
{
    public GameObject King;  
    public GameObject Queen; 
    public GameObject Bishop;
    public GameObject Knight; 
    public GameObject Rook;
    public GameObject Pawn;
    public Material[]  pieceColors;

    public Material[] pieceColorsSelected;
}

} // Definitions
} // Chess
