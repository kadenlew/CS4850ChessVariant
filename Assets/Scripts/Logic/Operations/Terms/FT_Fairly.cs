using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

[XmlType("Fairly")]
public class FT_Fairly : FuzzyTermUnary {
    public FT_Fairly(FuzzyTerm operand) {
        this.operand = operand;
    }

    internal FT_Fairly() {}

    public override double compute_confidence() { 
        return Math.Sqrt(operand.compute_confidence());
    }

    public override string ToString() => $"FAIRLY( {operand} )";

}

} // AI
} // Chess