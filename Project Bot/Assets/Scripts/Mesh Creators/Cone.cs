using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class Cone : MonoBehaviour
{
    public GameObject colidedObj;

    public int subdivisions = 10;
    public float radius = 1f;
    public float height = 2f;

    public bool regenerateMesh;
    public bool renderMesh;
    public bool generateCollider;
    public bool usedForCollision;

    public List<GameObject> sparkObjs;

    void Update()
    {
        if(renderMesh && gameObject.GetComponent<MeshRenderer>() == null)
        {
            gameObject.AddComponent<MeshRenderer>();
        }else if(!renderMesh && gameObject.GetComponent<MeshRenderer>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<MeshRenderer>());
        }

        if(generateCollider && gameObject.GetComponent<MeshCollider>() == null)
        {
            gameObject.AddComponent<MeshCollider>();
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().sharedMesh;
            GetComponent<MeshCollider>().convex = true;
        }else if(!generateCollider && gameObject.GetComponent<MeshCollider>() != null)
        {
            DestroyImmediate(gameObject.GetComponent<MeshCollider>());
        }

        if(regenerateMesh)
        {
            regenerateMesh = false;

            GetComponent<MeshFilter>().sharedMesh = Create(subdivisions, radius, height);
        }
    }

    Mesh Create(int subdivisions, float radius, float height)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[subdivisions + 2];
        Vector2[] uv = new Vector2[vertices.Length];

        int[] triangles = new int[(subdivisions * 2) * 3];

        vertices[0] = Vector3.zero;
        uv[0] = new Vector2(0.5f, 0f);

        for (int i = 0, n = subdivisions - 1; i < subdivisions; i++)
        {
            float ratio = (float)i / n;
            float r = ratio * (Mathf.PI * 2f);
            float x = Mathf.Cos(r) * radius;
            float z = Mathf.Sin(r) * radius;

            vertices[i + 1] = new Vector3(x, 0f, z);

            Debug.Log(ratio);

            uv[i + 1] = new Vector2(ratio, 0f);
        }

        vertices[subdivisions + 1] = new Vector3(0f, height, 0f);
        uv[subdivisions + 1] = new Vector2(0.5f, 1f);

        for (int i = 0, n = subdivisions - 1; i < n; i++)
        {
            int offset = i * 3;

            triangles[offset] = 0;
            triangles[offset + 1] = i + 1;
            triangles[offset + 2] = i + 2;
        }

        int bottomOffset = subdivisions * 3;

        for (int i = 0, n = subdivisions - 1; i < n; i++)
        {
            int offset = i * 3 + bottomOffset;

            triangles[offset] = i + 1;
            triangles[offset + 1] = subdivisions + 1;
            triangles[offset + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        print("E");

        if(usedForCollision)
        {
            colidedObj = other.gameObject;
        }

        /*if(other.gameObject.CompareTag("FuseBox") || other.gameObject.CompareTag("Enemy"))
        {
            if(!sparkObjs.Contains(other.gameObject))
            {
                sparkObjs.Add(other.gameObject);
            }
        }*/

        if (!sparkObjs.Contains(other.gameObject))
        {
            sparkObjs.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        colidedObj = null;
    }
}
