using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public class FT_Or : FuzzyTermBinary {
    public FT_Or(FuzzyTerm left_operand, FuzzyTerm right_operand) {
        this.left_operand = left_operand;
        this.right_operand = right_operand;
    } 

    public override double compute_confidence() { 
        return Math.Max(left_operand.compute_confidence(), right_operand.compute_confidence());
    }

    public override string ToString() => $"({left_operand} OR {right_operand})";
}

} // AI
} // Chess