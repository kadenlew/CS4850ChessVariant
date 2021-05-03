using System.Collections.Generic;
using UnityEngine;

namespace Chess
{
namespace AI
{

class FuzzyController {
    // maps string names for fuzzy logic input variables to their actual objects
    // these are to be set before trying to get an output
    public Dictionary<string, FuzzyVariable> input_variables { get; protected set; }

    // associates a string to a fuzzy variable for the output variable, or the one that
    // is going to be defuzzified after all of the rules are applied to get the
    // crisp output   
    public KeyValuePair<string, FuzzyVariable> output_variable { get; protected set; }

    // list of all the rules that are to be applied when using this controller
    // each rule will be applied sequentially, affecting the confidence of the sets within
    // the output variable
    public FuzzyRuleSet rule_base { get; protected set; }

    public FuzzyController() {
        input_variables = new Dictionary<string, FuzzyVariable>();
        rule_base = new FuzzyRuleSet(); 
    }

    // will call the fuzzify function of the input variable specified
    public void set_input(string input_name, double crisp_val) {
        input_variables[input_name].fuzzify(crisp_val);
    }

    public void set_input(FuzzyVariable variable, double crisp_val) {
        set_input(variable.name, crisp_val);
    }

    // will go and process the entire rule base against the current confidences of each of the
    // fuzzy sets in the input variables against the output
    public double get_output() {
        // reset the output
        output_variable.Value.clear_sets();

        // evaluate each rule
        foreach(FuzzyRule rule in rule_base.rules) {
            // Debug.Log($"{rule}"); 
            rule.eval();
        }

        // return the result of deffuzifying the output 
        return output_variable.Value.defuzzify();
    }
    
    public FuzzyVariable add_input_variable(string name, double lower_bound, double upper_bound) {
        input_variables.Add(name, new FuzzyVariable(name, lower_bound, upper_bound));
        return input_variables[name];
    }

    public FuzzyVariable create_output_variable(string name, double lower_bound, double upper_bound) {
        output_variable = new KeyValuePair<string, FuzzyVariable>(name, new FuzzyVariable(name, lower_bound, upper_bound));
        return output_variable.Value;
    }

    public void add_rule(FuzzyTerm condition, FT_Set output_set) {
        rule_base.rules.Add(new FuzzyRule(condition, output_set));
    }

    public FuzzySet find_set_by_name(string name) {
        // try the inputs
        foreach(KeyValuePair<string, FuzzyVariable> fv_name in input_variables) {
            foreach(KeyValuePair<string, FuzzySet> fs_name in fv_name.Value.member_sets) {
                if(fs_name.Key == name)
                    return fs_name.Value;
            }
        }

        // the the output
        foreach(KeyValuePair<string, FuzzySet> fs_name in output_variable.Value.member_sets) {
            if(fs_name.Key == name)
                return fs_name.Value;
        }
        
        // could not find
        throw new System.ArgumentException("Could not find the specified set");
    }

    public void set_rules_from_xml(string file) {
        this.rule_base = Helpers.XMLHelper.Deserialize<FuzzyRuleSet>(file);

        // find each set reference and pair them up
        foreach(FuzzyRule rule in this.rule_base.rules)
        {
            associate_with_ref(rule.condition);
            associate_with_ref(rule.result);
        }
    }

    public void associate_with_ref(FuzzyTerm ft) {

        // recursively search for ft_sets
        if(ft is FuzzyTermBinary) {
            associate_with_ref((ft as FuzzyTermBinary).left_operand);
            associate_with_ref((ft as FuzzyTermBinary).right_operand);
        }

        // recursively search for ft_sets
        if(ft is FuzzyTermUnary) {
            associate_with_ref((ft as FuzzyTermUnary).operand);
        }

        // associate ref with ft_set
        if(ft is FT_Set) {
            (ft as FT_Set).surrogate = find_set_by_name((ft as FT_Set).surrogate_name);
            Debug.Log($"Associated set {(ft as FT_Set).surrogate_name}.");
        }
    }


}

} // AI
} // Chess