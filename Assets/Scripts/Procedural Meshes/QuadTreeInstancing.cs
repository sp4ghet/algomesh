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

    private QuadTree<XYPos> InitQuadTrees() {
        var quadTree = new QuadTree<XYPos>(1, new Rect(-m_size/2, -m_size/2, m_size, m_size));
        for (int j = 0; j < 20; j++) {
            XYPos newObject = new XYPos(new Vector3(Random.Range(-m_size / 2, m_size / 2), Random.Range(-m_size / 2, m_size / 2), 0));
            quadTree.Insert(newObject);
        }
        return quadTree;
    }

    private void Start() {
        // InitMeshes();
    }

    IEnumerator slideAndKill(GameObject obj, Vector3 velocity, float lifetime) {
        float life = 0;
        while(life < lifetime) {
            life += Time.deltaTime;
            obj.transform.position += velocity * Time.deltaTime;
            //obj.transform.localScale = Vector3.one * obj.transform.position.magnitude*2/m_size;
            yield return new WaitForEndOfFrame();
        }
        Destroy(obj);
    }

    private void OnDrawGizmos() {
        //tree.DrawDebug();
    }

    private void Update() {
        if(Time.frameCount % 20 == 0) {
            tree = InitQuadTrees();
            List<(Vector3, Vector3)> infos = new List<(Vector3, Vector3)>();
            
            currentTreeDepth = (currentTreeDepth+1) % 10;
            tree.CreateObjects(ref infos, currentTreeDepth);
            
            CreateMesh(infos);
        }
    }

    private void CreateMesh(List<(Vector3, Vector3)> infos)
    {
        Mesh mesh = new Mesh();
        CombineInstance[] combineInstance = new CombineInstance[infos.Count];
        
        for (int i = 0; i < infos.Count; i++) {
                var (pos, size) = infos[i];
                int shapeIndex = i % meshes.Count;
                var rotPos = transform.rotation * pos + transform.position;
                var trsMatrix = Matrix4x4.TRS(rotPos, Quaternion.Euler(UnityEngine.Random.Range(-1, 2) * 45,
                                                                       UnityEngine.Random.Range(-1, 2) * 45,
                                                                       0), size);
                combineInstance[i].mesh = meshes[shapeIndex];
                combineInstance[i].transform = trsMatrix;
        }
        mesh.CombineMeshes(combineInstance, true, true);
        GameObject newObject = Instantiate(quadtree);
        newObject.GetComponent<MeshFilter>().mesh = mesh;
        var velocity = Vector3.forward * 30f;
        velocity = transform.rotation * velocity;
        StartCoroutine(slideAndKill(newObject, velocity, 10));
    }
}