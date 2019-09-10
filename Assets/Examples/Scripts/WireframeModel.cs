using UnityEngine;

public class WireframeModel : MonoBehaviour
{
    void Awake ()
    {
        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = CreateQuadMesh();
    }

    Mesh CreateQuadMesh ()
    {
        var mesh = new Mesh();
        mesh.hideFlags = HideFlags.HideAndDontSave;
        mesh.vertices = new Vector3[4] {
            new Vector3(-1, -1, 0),
            new Vector3(-1, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, -1, 0)
        };
        mesh.uv = new Vector2[4] {
            new Vector2(0, 0),
            new Vector2(0, 1),
            new Vector2(1, 1),
            new Vector2(1, 0)
        };
        mesh.uv2 = new Vector2[4] {
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(0, 1)
        };
        mesh.triangles = new int[6] {
            2, 1, 0,
            0, 3, 2
        };
        return mesh;
    }
}