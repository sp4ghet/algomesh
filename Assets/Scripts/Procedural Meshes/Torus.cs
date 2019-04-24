using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Torus {

    public static Mesh GenerateMesh(float m_outerRadius = 1f, float m_innerRadius = 0.3f, int m_radialSegments = 24, int m_sideSegments = 18) {
        Mesh mesh = new Mesh();

        #region Vertices	
        Vector3[] vertices = new Vector3[(m_radialSegments + 1) * (m_sideSegments + 1)];
        float _2pi = Mathf.PI * 2f;
        for (int seg = 0; seg <= m_radialSegments; seg++) {
            int currSeg = seg == m_radialSegments ? 0 : seg;

            float t1 = (float)currSeg / m_radialSegments * _2pi;
            Vector3 r1 = new Vector3(Mathf.Cos(t1) * m_outerRadius, 0f, Mathf.Sin(t1) * m_outerRadius);

            for (int side = 0; side <= m_sideSegments; side++) {
                int currSide = side == m_sideSegments ? 0 : side;

                float t2 = (float)currSide / m_sideSegments * _2pi;
                Vector3 r2 = Quaternion.AngleAxis(-t1 * Mathf.Rad2Deg, Vector3.up) * new Vector3(Mathf.Sin(t2) * m_innerRadius, Mathf.Cos(t2) * m_innerRadius);

                vertices[side + seg * (m_sideSegments + 1)] = r1 + r2;
            }
        }
        #endregion

        #region Normals		
        Vector3[] normals = new Vector3[vertices.Length];
        for (int seg = 0; seg <= m_radialSegments; seg++) {
            int currSeg = seg == m_radialSegments ? 0 : seg;

            float t1 = (float)currSeg / m_radialSegments * _2pi;
            Vector3 r1 = new Vector3(Mathf.Cos(t1) * m_outerRadius, 0f, Mathf.Sin(t1) * m_outerRadius);

            for (int side = 0; side <= m_sideSegments; side++) {
                normals[side + seg * (m_sideSegments + 1)] = (vertices[side + seg * (m_sideSegments + 1)] - r1).normalized;
            }
        }
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int seg = 0; seg <= m_radialSegments; seg++)
            for (int side = 0; side <= m_sideSegments; side++)
                uvs[side + seg * (m_sideSegments + 1)] = new Vector2((float)seg / m_radialSegments, (float)side / m_sideSegments);
        #endregion

        #region Triangles
        int nbFaces = vertices.Length;
        int nbTriangles = nbFaces * 2;
        int nbIndexes = nbTriangles * 3;
        int[] triangles = new int[nbIndexes];

        int i = 0;
        for (int seg = 0; seg <= m_radialSegments; seg++) {
            for (int side = 0; side <= m_sideSegments - 1; side++) {
                int current = side + seg * (m_sideSegments + 1);
                int next = side + (seg < (m_radialSegments) ? (seg + 1) * (m_sideSegments + 1) : 0);

                if (i < triangles.Length - 6) {
                    triangles[i++] = current;
                    triangles[i++] = next;
                    triangles[i++] = next + 1;

                    triangles[i++] = current;
                    triangles[i++] = next + 1;
                    triangles[i++] = current + 1;
                }
            }
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();

        return mesh;
    }
}
