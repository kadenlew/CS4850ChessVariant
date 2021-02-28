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

   public MoveAction(GameObject agent, BoardPosition target) {
       this.agent = agent;
       this.target = target;
   }
}

} // Definitions
} // Chess