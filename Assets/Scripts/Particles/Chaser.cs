using UnityEngine;
using System.Collections;

public class Chaser : MonoBehaviour {

    [SerializeField, Range(1, 10)]
    float speed;

    [SerializeField, Range(1, 10)]
    float noiseScale;

    Vector3 wallCenter;
    Vector3 wallSize;

    Vector3 prevPos;
    Vector3 velocity;

    public void SetBounds(Vector3 center, Vector3 xyz) {
        wallCenter = center;
        wallSize = xyz;
        transform.position = wallCenter;
    }

    Vector3 EnforceBounds(Vector3 pos, Vector3 force) {
        force.x = pos.x - wallCenter.x > wallSize.x  / 2 ? force.x - 1 : force.x;
        force.x = pos.x - wallCenter.x < -wallSize.x / 2 ? force.x + 1 : force.x;

        force.y = pos.y - wallCenter.y > wallSize.y  / 2 ? force.y - 1 : force.y;
        force.y = pos.y - wallCenter.y < -wallSize.y / 2 ? force.y + 1 : force.y;

        force.z = pos.z - wallCenter.z > wallSize.z  / 2 ? force.z - 1 : force.z;
        force.z = pos.z - wallCenter.z < -wallSize.z / 2 ? force.z + 1 : force.z;

        return force;
    }

    // Use this for initialization
    void Start() {
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update() {
        Vector3 force = new Vector3(
            Mathf.PerlinNoise(Time.time / noiseScale + 3613f, 361f),
            Mathf.PerlinNoise(Time.time / noiseScale + 12341, 123f),
            Mathf.PerlinNoise(Time.time / noiseScale + 13f, 1f)
            ) * 1.6f - Vector3.one * 0.8f;

        force = EnforceBounds(transform.position, force);
        
        velocity += force * speed * Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, speed);
        transform.position += velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(transform.position - prevPos);

        prevPos = transform.position;
    }
}
