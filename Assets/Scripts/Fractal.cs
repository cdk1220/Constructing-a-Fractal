﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;

    public int maxDepth;                // Indicates the max number of levels to go down recursively
    private int currentDepth = 0;       // Indicates the depth which the current object is at   

    public float childScale;            // How much to scale the child by

	// Use this for initialization
	private void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        // If less than maximum  depth, create more children growing up and to the right
        if (currentDepth < maxDepth) {
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.up);
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, Vector3.right);
        }
	}

    // Used to create child objects that inherit parent properties
    private void Initialize(Fractal parent, Vector3 direction) {
        mesh = parent.mesh;
        material = parent.material;
        maxDepth = parent.maxDepth;
        currentDepth = parent.currentDepth + 1;
        childScale = parent.childScale;

        // To make Fractal parent the parent of what is to be created
        transform.parent = parent.transform;

        // Scale the child 
        transform.localScale = Vector3.one * childScale;

        // Move the child so that once moved that are in contact
        transform.localPosition = direction * (0.5f + 0.5f * childScale);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
