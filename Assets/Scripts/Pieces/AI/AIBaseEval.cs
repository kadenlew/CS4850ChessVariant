using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace Chess
{
namespace Piece
{
namespace AI
{

public class AIActionEval : MonoBehaviour
{
    protected Piece.GamePieceBase piece_ref { get; set; }
    // Start is called before the first frame update
    void Start()
    {
       Debug.Log($"You've added a Standard AI Component to {GetComponent<GamePieceBase>()}");
    }

    public void init () {
        // store a reference to the game piece information
        piece_ref = GetComponent<GamePieceBase>();
    }

    public (Definitions.Action action, double desireability) eval(ref Definitions.ActionDatabase database) {
        
        // get the actions we need to evaluate for this piece  
        HashSet<Definitions.Action> possible_actions;
        if(database.get_actions(piece_ref, out possible_actions))
        {
            // Do the evaluation, for now we randomly select 
            return (
                possible_actions.ElementAt(Random.Range(0, possible_actions.Count)),
                Random.Range(0f, 10f)
            );
        }

        // there are no actions this piece can take, return
        return (null, double.MinValue);
    }

}

} // AI
} // Piece
} // Chess
