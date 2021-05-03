using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

[XmlRoot("Rule")]
public class FuzzyRule {
    // the first portion of the IF THEN statement
    public FuzzyTerm condition { get; set; }

    // the fuzzy set which will be modified as a result of this rule
    // effectively the output for the fuzzy controller
    public FT_Set result { get; set; }

    public FuzzyRule(FuzzyTerm condition, FT_Set result) {
        this.condition = condition;
        this.result = result;
    }

    internal FuzzyRule() {}

    // evaluate the rule and combine the result of each set in the fuzzy variable
    public void eval() {
        // the resulting output is combined with the result of the condition
        result.combine_confidence(condition.compute_confidence());

        Debug.Log($"condition confidence: {condition.compute_confidence()}");
    }

    public override string ToString() => $"Rule: IF {condition} THEN {result}";
        
}

} // AI
} // Chess