using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public class FT_Very : FuzzyTermUnary {
    public FT_Very(FuzzyTerm operand) {
        this.operand = operand;
    } 

    public override double compute_confidence() { 
        return Math.Pow(operand.compute_confidence(), 2);
    }

    public override string ToString() => $"VERY( {operand} )";
}

} // AI
} // Chess