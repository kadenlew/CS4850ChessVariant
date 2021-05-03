using System.Collections.Generic;
using System;
using UnityEngine;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

[XmlType("And")]
public class FT_And : FuzzyTermBinary {
    public FT_And(FuzzyTerm left_operand, FuzzyTerm right_operand) {
        this.left_operand = left_operand;
        this.right_operand = right_operand;
    }

    internal FT_And() {}

    public override double compute_confidence() { 
        return Math.Min(left_operand.compute_confidence(), right_operand.compute_confidence());
    }

    public override string ToString() => $"({left_operand} AND {right_operand})";
        
}

} // AI
} // Chess