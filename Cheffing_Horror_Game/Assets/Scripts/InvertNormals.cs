using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertNormals : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the MeshFilter component attached to this GameObject
        MeshFilter filter = GetComponent<MeshFilter>();
        if (filter != null && filter.mesh != null)
        {
            Mesh mesh = filter.mesh;

            // Invert normals
            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = -normals[i];
            }
            mesh.normals = normals;

            // Invert triangles
            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    // Swap order of triangle vertices
                    int temp = triangles[i];
                    triangles[i] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }
    }
}

   

