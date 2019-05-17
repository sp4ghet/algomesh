using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTreeInstancing : MonoBehaviour {

    [SerializeField]
    GameObject quadtree;

    [SerializeField, Range(16, 128)]
    float m_size = 32;

    [SerializeField]
    List<Mesh> meshes;

    QuadTree<XYPos> tree;

    int currentTreeDepth = 0;
    private float speed;

    public float Speed { get => speed; set => speed = value; }

    public class XYPos : IQuadTreeObject {
        private Vector3 m_vPosition;
        public XYPos(Vector3 position) {
            m_vPosition = position;
        }
        public Vector2 GetPosition() {
            //Ignore the Y position, Quad-trees operate on a 2D plane.
            return new Vector2(m_vPosition.x, m_vPosition.y);
        }
    }

    public void NewInstance(float t) {
        if(t > 0) { return; }
        tree = InitQuadTrees();
        List<(Vector3, Vector3)> infos = new List<(Vector3, Vector3)>();

        currentTreeDepth = (currentTreeDepth + 1) % 10;
        tree.CreateObjects(ref infos, currentTreeDepth);

        CreateMesh(infos);
    }


    private QuadTree<XYPos> InitQuadTrees() {
        var quadTree = new QuadTree<XYPos>(1, new Rect(-m_size/2, -m_size/2, m_size, m_size));
        for (int j = 0; j < 20; j++) {
            XYPos newObject = new XYPos(new Vector3(Random.Range(-m_size / 2, m_size / 2), Random.Range(-m_size / 2, m_size / 2), 0));
            quadTree.Insert(newObject);
        }
        return quadTree;
    }

    IEnumerator slideAndKill(GameObject obj, Vector3 velocity, float lifetime) {
        float life = 0;
        while(life < lifetime) {
            var v = Vector3.Scale(velocity, transform.parent.localScale);
            life += Time.deltaTime;
            obj.transform.position = transform.position + (v * life);
            yield return new WaitForEndOfFrame();
        }
        Destroy(obj);
    }

    private void OnDrawGizmos() {
        //tree.DrawDebug();
    }

    private void CreateMesh(List<(Vector3, Vector3)> infos)
    {
        Mesh mesh = new Mesh();
        CombineInstance[] combineInstance = new CombineInstance[infos.Count];
        
        for (int i = 0; i < infos.Count; i++) {
                var (pos, size) = infos[i];
                int shapeIndex = i % meshes.Count;
                var rotPos = pos;
                var trsMatrix = Matrix4x4.TRS(rotPos, Quaternion.Euler(UnityEngine.Random.Range(-1, 2) * 45,
                                                                       UnityEngine.Random.Range(-1, 2) * 45,
                                                                       0), size);
                combineInstance[i].mesh = meshes[shapeIndex];
                combineInstance[i].transform = trsMatrix;
        }
        mesh.CombineMeshes(combineInstance, true, true);
        GameObject newObject = Instantiate(quadtree, transform);
        newObject.GetComponent<MeshFilter>().mesh = mesh;
        var velocity = Vector3.forward * speed;
        velocity = transform.rotation * velocity;
        StartCoroutine(slideAndKill(newObject, velocity, speed/3f));
    }
}