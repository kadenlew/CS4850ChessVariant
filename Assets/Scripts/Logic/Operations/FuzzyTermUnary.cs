using System.Collections.Generic;

namespace Chess
{
namespace AI
{

public abstract class FuzzyTermUnary : FuzzyTerm {

    // the single operand for this unary action
    protected FuzzyTerm operand;

    // all of the specific operations will be defined as terms
    // and the only requirement is that they can output a confidence
    // value for other terms to ingest
    // the outermost fuzzy term which outputs its confidence will therefore
    // output the confidence of the whole sentence
    public override abstract double compute_confidence();

}

} // AI
} // Chess