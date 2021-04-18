using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace AI
{

// this class acts as a wrapper for the FuzzySet so that it can inately work with
// the recursive nature of the term definition
// this simply points to a surrogate, real fuzzy set where it gets its data from
// this acts as the sort of, acutal end point for the data in these sentences,
// in a similar way to how EBNF for an interpreter is constructed for expressions
// on variables and such
public class FT_Set : FuzzyTerm {

    // the surrogate set that this object is shadowing
    private FuzzySet surrogate;

    public FT_Set(FuzzySet surrogate) {
        this.surrogate = surrogate;
    }
    
    public override double compute_confidence() { 
        return surrogate.confidence;
    }

    public void combine_confidence(double confidence) {
        surrogate.combine_confidence(confidence);
    }

    public double get_confidence() {
        return surrogate.confidence;
    }


    public override string ToString() => $"{surrogate}";
}

} // AI
} // Chess