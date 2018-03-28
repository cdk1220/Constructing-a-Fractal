using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;

    public int maxDepth;            // Indicates the max number of levels to go down recursively
    private int currentDepth;       // Indicates the depth which the current object is at             

	// Use this for initialization
	private void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        // If less than maximum  depth, create more children
        if (currentDepth < maxDepth) {
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this);
        }
	}

    // Used to create child objects that inherit parent properties
    private void Initialize(Fractal parent)
    {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        currentDepth = parent.currentDepth + 1;
        transform.parent = parent.transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
