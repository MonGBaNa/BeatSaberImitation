using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type {
    WireCube,
    Cube,
    WireSphere,
    Sphere,
    Axis
}

public class DrawGizoms : MonoBehaviour {

    public Color color = Color.red;
    public Vector3 size = Vector3.one;
    public float radius = 0.2f;
    public Type type = Type.WireCube;

    private void OnDrawGizmos() {
        Gizmos.color = color;
        switch (type) {
            case Type.WireCube:
                Gizmos.DrawWireCube(transform.position, size);
                break;
            case Type.Cube:
                Gizmos.DrawCube(transform.position, size);
                break;
            case Type.WireSphere:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;
            case Type.Sphere:
                Gizmos.DrawSphere(transform.position, radius);
                break;
            case Type.Axis:
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position + transform.right * size.x * 0.5f, transform.position - transform.right * size.x * 0.5f);

                Gizmos.color = Color.green; 
                Gizmos.DrawLine(transform.position + transform.up * size.y * 0.5f, transform.position - transform.up * size.y * 0.5f);

                Gizmos.color = Color.blue;
                Gizmos.DrawLine(transform.position + transform.forward * size.z * 0.5f, transform.position - transform.forward * size.z * 0.5f);
                break;
        }        
    }
}
