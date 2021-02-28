using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

// collection of unity materials and prefabs used in making spawning more unified
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

    public GameObject Tile;
    public Material[] tileMaterials;
}

} // Definitions
} // Chess