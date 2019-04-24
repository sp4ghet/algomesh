using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RegularPolygon
{

    public static Mesh GenerateMesh(float radius, int subdivisions=3, Mesh mesh=null) {
        if(mesh == null){
            mesh = new Mesh();
        }else{
            mesh.Clear();
        }
        int trianglePerSubdivision = 24;
        int edgesPerSubdivision = 4;
        Vector3[] vertices = new Vector3[subdivisions*4];
        int[] tris = new int[subdivisions * 24];
        float side = 0.5f;

        for (int i = 0; i < subdivisions; i++) {
            Quaternion rot = Quaternion.Euler(0, 0, (i / (float)subdivisions) * 360);
            vertices[i * edgesPerSubdivision] = rot * new Vector3(0, radius - side, 0);
            vertices[i * edgesPerSubdivision + 1] = rot * new Vector3(0, radius - side, side * 2);
            vertices[i * edgesPerSubdivision + 2] = rot * new Vector3(0, radius + side, 0);
            vertices[i * edgesPerSubdivision + 3] = rot * new Vector3(0, radius + side, side * 2);

            tris[i * trianglePerSubdivision + 0] = (i * edgesPerSubdivision + 0) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 1] = (i * edgesPerSubdivision + 4) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 2] = (i * edgesPerSubdivision + 2) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 3] = (i * edgesPerSubdivision + 2) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 4] = (i * edgesPerSubdivision + 4) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 5] = (i * edgesPerSubdivision + 6) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 6] = (i * edgesPerSubdivision + 2) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 7] = (i * edgesPerSubdivision + 6) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 8] = (i * edgesPerSubdivision + 3) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 9] = (i * edgesPerSubdivision + 3) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 10] = (i * edgesPerSubdivision + 6) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 11] = (i * edgesPerSubdivision + 7) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 12] = (i * edgesPerSubdivision + 3) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 13] = (i * edgesPerSubdivision + 7) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 14] = (i * edgesPerSubdivision + 1) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 15] = (i * edgesPerSubdivision + 1) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 16] = (i * edgesPerSubdivision + 7) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 17] = (i * edgesPerSubdivision + 5) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 18] = (i * edgesPerSubdivision + 1) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 19] = (i * edgesPerSubdivision + 5) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 20] = (i * edgesPerSubdivision + 0) % (subdivisions * 4);

            tris[i * trianglePerSubdivision + 21] = (i * edgesPerSubdivision + 0) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 22] = (i * edgesPerSubdivision + 5) % (subdivisions * 4);
            tris[i * trianglePerSubdivision + 23] = (i * edgesPerSubdivision + 4) % (subdivisions * 4);
        }

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }

}
