using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace AI
{

class FuzzyVariable {
    // mapped to a string so that eventually external files can 
    // be utilized to codify a rule base for a particular piece
    public Dictionary<string, FuzzySet> member_sets;

    public string name { get; set; }    

    public double lower_bound { get; set; }
    public double upper_bound { get; set; }


    public FuzzyVariable(string name, double lower_bound, double upper_bound) {
        this.member_sets = new Dictionary<string, FuzzySet>();
        this.name = name;

        this.lower_bound = lower_bound;
        this.upper_bound = upper_bound;
    }

    // used to update all of the fuzzy sets inside this variable 
    // by determining the confidence of membership of each set
    public void fuzzify(double crisp_input) {
        // string output = $"{name}: {crisp_input} => {{";
        // see the level of confidence this input belongs to each set 
        foreach(FuzzySet set in member_sets.Values)
        {
            // update its confidence by computing it
            set.confidence = set.compute_confidence(crisp_input);
            // output += $"{set}, ";
        } 
        // Debug.Log($"{output}}}");
    }

    // given the current status of all our sets, perform some centroid computation
    // to convert the fuzzy set memberships to a crisp output
    // this is using the MaxAv approximation technique
    public double defuzzify() {
        double numerator = 0;
        double denominator = 0;
        
        // string output = $"{name}: {{";
        // add each set to the initial sums
        foreach(FuzzySet set in member_sets.Values)
        {
            // output += $"{set} ({set.rep_val}), ";

            numerator += set.rep_val * set.confidence;
            denominator += set.confidence;
        }

        // output += $"}} => ({numerator} / {denominator}) => {numerator / denominator}";
        // Debug.Log($"{output}");

        // get the centroid
        return numerator / denominator;
    }

    // clears out all of the confidence thresholds of each set
    // to 0, particularly useful when reseting the output variable
    // for applying the rule base against
    public void clear_sets() {
        foreach(FuzzySet set in member_sets.Values)
        {
            set.confidence = 0;
        }
    }

    public FT_Set add_set_left_shoulder(string name, double inflection_point, double end_point) {
        member_sets.Add(name, new FS_LeftShoulder(name, lower_bound, inflection_point, end_point));
        return new FT_Set(member_sets[name]);
    }
    public FT_Set add_set_right_shoulder(string name, double end_point, double inflection_point) {
        member_sets.Add(name, new FS_RightShoulder(name, end_point, inflection_point, upper_bound));
        return new FT_Set(member_sets[name]);
    }
    public FT_Set add_set_triangular(string name, double left_bound, double peak, double right_bound) {
        member_sets.Add(name, new FS_Triangular(name, left_bound, peak, right_bound));
        return new FT_Set(member_sets[name]);
    }

}

} // AI
} // Chess