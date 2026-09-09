//using System.Collections;
//sing System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[CustomEditor(typeof(VisionCone))]
public class VisionConeEditor : Editor
{
    private void OnSceneGUI()
    {
        VisionCone vc = (VisionCone)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(vc.transform.position, Vector3.up, Vector3.forward, 360, vc.radius);

        Vector3 AngleA = vc.DirectFromAngle(-vc.angle / 2, false);
        Vector3 AngleB = vc.DirectFromAngle(vc.angle / 2, false);

        Handles.DrawLine(vc.transform.position, vc.transform.position + AngleA * vc.radius);
        Handles.DrawLine(vc.transform.position, vc.transform.position + AngleB * vc.radius);

        Handles.color = Color.red;
        if (vc.player != null)
        {
            Handles.DrawLine(vc.transform.position, vc.player.position);
        }
            
        
    }
}
