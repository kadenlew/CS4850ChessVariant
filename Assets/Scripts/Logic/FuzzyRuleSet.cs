using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

[XmlRoot("RuleBase")]
public class FuzzyRuleSet {
    public List<FuzzyRule> rules { get; set; } = new List<FuzzyRule>();

    internal FuzzyRuleSet () {}
}

}
}