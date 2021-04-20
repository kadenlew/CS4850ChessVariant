using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public class FT_Not : FuzzyTermUnary {
    public FT_Not(FuzzyTerm operand) {
        this.operand = operand;
    }

    internal FT_Not() {}

    public override double compute_confidence() { 
        return 1 - operand.compute_confidence();
    }

    public override string ToString() => $"NOT( {operand} )";
}

} // AI
} // Chess