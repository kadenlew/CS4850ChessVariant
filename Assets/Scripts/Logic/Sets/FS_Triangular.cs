using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public class FS_Triangular : FuzzySet {
    public double left_bound { get; protected set; }
    public double peak { get; protected set; }
    public double right_bound { get; protected set; }

    public override double rep_val { get { return peak; } } 

    public FS_Triangular(string name, double left_bound, double peak, double right_bound) {
        // store the geometry
        this.name = name;
        this.left_bound = left_bound;
        this.peak = peak;
        this.right_bound = right_bound;
    }

    public override double compute_confidence(double crisp_value)
    {
        // this is outside our set
        if(crisp_value <= left_bound || crisp_value >= right_bound)
            return 0;

        if(crisp_value < peak)
        {
            // we are on the left half of the peak
            return (crisp_value - left_bound) / (peak - left_bound);
        }
        else
        {
            // we are on the right half of the peak
            return (right_bound - crisp_value) / (right_bound - peak);
        }
    }
        
}

} // AI
} // Chess