using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadTreeInstancing : MonoBehaviour {

    [SerializeField]
    GameObject[] prefabs;

    [SerializeField, Range(16, 128)]
    float m_size = 32;

    QuadTree<TestObject> tree;

    Material mat;

    public class TestObject : IQuadTreeObject {
        private Vector3 m_vPosition;
        public TestObject(Vector3 position) {
            m_vPosition = position;
        }
        public Vector2 GetPosition() {
            //Ignore the Y position, Quad-trees operate on a 2D plane.
            return new Vector2(m_vPosition.x, m_vPosition.y);
        }
    }

    private QuadTree<TestObject> InitQuadTrees() {
        var quadTree = new QuadTree<TestObject>(1, new Rect(-m_size/2, -m_size/2, m_size, m_size));
        for (int j = 0; j < 20; j++) {
            TestObject newObject = new TestObject(new Vector3(Random.Range(-m_size / 2, m_size / 2), Random.Range(-m_size / 2, m_size / 2), 0));
            quadTree.Insert(newObject);
        }
        return quadTree;
    }

    private void Start() {
        
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
            
            tree.CreateObjects(ref infos);
            int i = 0;
            foreach (var (pos, size) in infos) {
                i = (i+1) % prefabs.Length;
                var rotPos = transform.rotation * pos + transform.position;
                var newObject = Instantiate(prefabs[i], rotPos, Random.rotation, transform);
                newObject.transform.localScale = size;
                var velocity = Vector3.forward * 30f;
                //velocity = Quaternion.Euler( ((-pos.y * 2f) / m_size) * 45,
                //                                ((-pos.x * 2f) / m_size) * 45,
                //                                0) * velocity;
                //velocity *= (velocity.magnitude / velocity.z);
                velocity = transform.rotation * velocity;
                StartCoroutine(slideAndKill(newObject, velocity, 10));
            }
        }
    }
}