using System;
using System.Collections;
using System.Collections.Generic;


namespace Chess
{
namespace AI
{

public class FuzzyRuleBase : IEnumerable {
    List<FuzzyRule> rules;

    // expose count property
    public int Count { get { return rules.Count; } }
    
    // expose indexing
    public FuzzyRule this[int index] { get { return rules[index]; } set { rules[index] = value; }}
    public FuzzyRuleBase() {
        rules = new List<FuzzyRule>();
    } 

    // expose Add operation
    public void Add(FuzzyRule rule) {
        this.rules.Add(rule);
    }

    // allow ForEach operations
    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator) GetEnumerator();
    }

    public FuzzyRuleBaseEnum GetEnumerator()
    {
        return new FuzzyRuleBaseEnum(rules);
    } 
}

public class FuzzyRuleBaseEnum : IEnumerator {
    protected List<FuzzyRule> rules;
    int position = -1;

    public FuzzyRuleBaseEnum(List<FuzzyRule> rules) {
        this.rules = rules;
    }

    public bool MoveNext()
    {
        position++;
        return (position < rules.Count);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current { get { return Current; } }

    public FuzzyRule Current { get { try { return rules[position]; } catch(IndexOutOfRangeException) { throw new InvalidOperationException(); }}}

}

}
}