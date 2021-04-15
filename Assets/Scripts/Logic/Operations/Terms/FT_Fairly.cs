using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public class FT_Fairly : FuzzyTermUnary {
    public FT_Fairly(FuzzyTerm operand) {
        this.operand = operand;
    }

    public override double compute_confidence() { 
        return Math.Sqrt(operand.compute_confidence());
    }

    public override string ToString() => $"FAIRLY( {operand} )";

}

} // AI
} // Chess