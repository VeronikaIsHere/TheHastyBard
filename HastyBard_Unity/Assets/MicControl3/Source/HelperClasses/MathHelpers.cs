using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathH
{

    //remaps a float from one range to another.
    public static float RemapFloat (float input, float fromMin, float fromMax, float toMin, float toMax)
    {
        return toMin + (input - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }
    public static double RemapDouble (double input, double fromMin, double fromMax, double toMin, double toMax)
    {
        return toMin + (input - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    public static Vector3 RemapVector3 (Vector3 input, float fromMin, float fromMax, float toMin, float toMax)
    {
        float x = RemapFloat (input.x, fromMin, fromMax, toMin, toMax);
        float y = RemapFloat (input.y, fromMin, fromMax, toMin, toMax);
        float z = RemapFloat (input.z, fromMin, fromMax, toMin, toMax);

        return new Vector3 (x, y, z);

    }

    //remaps an int from one range to another.
    public static int RemapInt (int input, int fromMin, int fromMax, int toMin, int toMax)
    {
        return toMin + (input - fromMin) * (toMax - toMin) / (fromMax - fromMin);
    }

    //creates a new position based on the rotation around a pivot point.
    public static Vector3 PositionAroundPoint (Vector3 point, Vector3 pivot, Quaternion angle)
    {

        Vector3 finalPos = point - pivot;

        //offset the angle
        angle.eulerAngles = new Vector3 (angle.eulerAngles.x, angle.eulerAngles.y - 90, angle.eulerAngles.z);
        //Center the point around the origin
        finalPos = angle * finalPos;
        //Rotate the point.

        finalPos += pivot;
        //Move the point back to its original offset. 
        return finalPos;
    }

}