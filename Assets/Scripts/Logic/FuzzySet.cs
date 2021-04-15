using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public abstract class FuzzySet {
    // the confidence of which the passed input belongs to this set
    public double confidence { get; set; } = 0;
    
    public string name { get; set; }
    // in the MaxAv Defuzzification algorthim, the crisp output can be 
    // approximated by relating the confidence of this set to a value 
    // that represents it, often the point where it hits its peak
    public abstract double rep_val { get; }

    // computes the degree of membership a particular crisp value has 
    // for this particular set
    // this depends on the specific geometry of this fuzzy set along
    // the crisp input, and thus is specified by particular 
    // fuzzy set geometry implementations
    public abstract double compute_confidence(double crisp_value);

    // used to combine sets when they apply through multiple rules
    public void combine_confidence(double confidence) {
        this.confidence = Math.Max(this.confidence, confidence);
    }

    public override string ToString() => $"{name} [{confidence:0.##}]";
}

} // AI
} // Chess