using System.Collections.Generic;
using System;

namespace Chess
{
namespace AI
{

public class FS_RightShoulder : FS_Shoulder {

    public FS_RightShoulder(string name, double end_point, double inflection_point, double right_boundary) {
        this.name = name;

        // from left to right
        this.end_point = end_point;
        this.inflection_point = inflection_point;
        this.boundary = right_boundary;
    }

    public override double compute_confidence(double crisp_value) {
        // within the max part of the shoulder
        if(crisp_value >= inflection_point)
            return 1;

        // outside the shoulder completely
        if(crisp_value <= end_point)
            return 0;

        // on the slant of the shoulder
        return (crisp_value - end_point) / (inflection_point - end_point);
                
    }
}

} // AI
} // Chess