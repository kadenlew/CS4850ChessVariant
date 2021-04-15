using System.Collections.Generic;
using System;
using UnityEngine;

namespace Chess
{
namespace AI
{

public abstract class FS_Shoulder : FuzzySet {

    public double boundary { get; protected set; }
    public double inflection_point { get; protected set; }
    public double end_point { get; protected set; }

    public override double rep_val { get { return (inflection_point + boundary) / 2; } }

    public abstract override double compute_confidence(double crisp_value);

        
}

} // AI
} // Chess