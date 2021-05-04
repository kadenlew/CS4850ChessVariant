using System.Collections.Generic;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

public abstract class FuzzyTermBinary : FuzzyTerm {

    // the two operands of this binary operation
    public FuzzyTerm left_operand;
    public FuzzyTerm right_operand;

    // all of the specific operations will be defined as terms
    // and the only requirement is that they can output a confidence
    // value for other terms to ingest
    // the outermost fuzzy term which outputs its confidence will therefore
    // output the confidence of the whole sentence
    public override abstract double compute_confidence();

}

} // AI
} // Chess