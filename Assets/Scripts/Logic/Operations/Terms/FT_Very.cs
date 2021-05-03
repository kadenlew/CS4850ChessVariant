using System.Collections.Generic;
using System;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

[XmlType("Very")]
public class FT_Very : FuzzyTermUnary {
    public FT_Very(FuzzyTerm operand) {
        this.operand = operand;
    } 

    internal FT_Very() {}

    public override double compute_confidence() { 
        return Math.Pow(operand.compute_confidence(), 2);
    }

    public override string ToString() => $"VERY( {operand} )";
}

} // AI
} // Chess