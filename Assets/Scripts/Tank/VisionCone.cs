using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class VisionCone : MonoBehaviour
{
    
    private GuardController controller;

    public float radius;
    public float angle;

    public LayerMask playerMask;
    public LayerMask ObstacleMask;

    
    //public float meshResolution;

    

    [HideInInspector]
    public Transform player;

    public float meshResolution;

    public MeshFilter meshFilter;
    Mesh mesh;

    /*
    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    */

    public void Start()
    {
        
        mesh = new Mesh();
        mesh.name = "Vision Mesh";
        meshFilter.mesh = mesh;
        /*
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;
        */

        controller = this.GetComponentInParent<GuardController>();
        

    }
    private void Update()
    {
        PlayerVisible();
        
    }
    private void LateUpdate()
    {
        DrawVisionCone();
    }

    private void PlayerVisible()
    {
        bool spotted = false;
        
        player = null;
        Collider[] objInView = Physics.OverlapSphere(transform.position, radius,playerMask);

        for (int i = 0; i <objInView.Length; i++)
        {
            Transform obj = objInView[i].transform;
            Vector3 dirToObj = (obj.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToObj) < angle / 2)
            {
                float distToObj = Vector3.Distance(transform.position, obj.position);
                if (!Physics.Raycast(transform.position, dirToObj, distToObj, ObstacleMask))
                {
                    player = obj;
                    
                    if (player.GetComponent<TankBody>().health > 0)
                    {
                        
                        spotted = true;
                    }
                    
                    
                }


            }
            
        }
        
        if (spotted)
        {

            controller.spotted();

        }
        else if (!spotted)
        {

            controller.notSpotted();
        }
    }

    private void DrawVisionCone()
    {
        int stepCount = Mathf.RoundToInt(angle * meshResolution);
        float stepAngleSize = angle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        
        for (int i = 0; i <= stepCount; i++) 
        {
            float drawangle = transform.eulerAngles.y - angle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(drawangle);
            
            viewPoints.Add(newViewCast.point);
        }

        int vertcount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertcount];
        int[] tri = new int[(vertcount - 2) * 3];
        

        vertices[0] = Vector3.zero;
        for (int i = 0; i<vertcount-1; i++)
        {
            vertices[i+1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertcount - 2)
            {
                
                tri[i * 3] = 0;
                tri[i * 3 + 1] = i + 1;
                tri[i * 3 + 2] = i + 2;
            } 
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tri;
        
        mesh.RecalculateNormals();
        

    }

    ViewCastInfo ViewCast(float angle)
    {
        Vector3 direct = DirectFromAngle(angle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direct, out hit, radius, ObstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, angle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + direct * radius, hit.distance, angle);
        }
    }

    public Vector3 DirectFromAngle(float angle, bool angleIsGlobal)
    {
        if (!angleIsGlobal) 
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));

    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dist;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
        {
            hit = _hit;
            point = _point;
            dist = _dist;
            angle = _angle;
        }
    }
}
