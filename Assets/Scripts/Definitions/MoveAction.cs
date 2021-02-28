using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace Definitions
{

public class MoveAction : Action
{
   public BoardPosition target { get; }

   public MoveAction(BoardPosition target) {
       this.target = target;
   }
}

} // Definitions
} // Chess