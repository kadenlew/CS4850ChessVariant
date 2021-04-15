using System.Collections.Generic;
using System;
using UnityEngine;

namespace Chess
{
namespace AI
{

public class FS_LeftShoulder : FS_Shoulder {

    public FS_LeftShoulder(string name, double left_boundary, double inflection_point, double end_point) {
        this.name = name;

        // from left to right
        this.boundary = left_boundary;
        this.inflection_point = inflection_point;
        this.end_point = end_point;
    }

    public override double compute_confidence(double crisp_value) {
        // within the max part of the shoulder
        if(crisp_value <= inflection_point)
            return 1;

        // outside the shoulder completely
        if(crisp_value >= end_point)
            return 0;

        // on the slant of the shoulder
        return (end_point - crisp_value) / (end_point - inflection_point);
                
    }
}

} // AI
} // Chess