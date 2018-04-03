﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;

    public int maxDepth;                // Indicates the max number of levels to go down recursively
    private int currentDepth = 0;       // Indicates the depth which the current object is at   

    public float childScale;            // How much to scale the child by

    // Directions the children would grow in
    private static Vector3[] childDirections = {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    // Orientations for the children
    private static Quaternion[] childOrientations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 0f, -90f),
        Quaternion.Euler(0f, 0f, 90f),
        Quaternion.Euler(90f, 0f, 0f),
        Quaternion.Euler(-90f, 0f, 0f)
    };
     
	// Use this for initialization
	private void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;

        // If less than maximum  depth, create more children growing up, to the right, and left
        if (currentDepth < maxDepth) {
            StartCoroutine(CreateChildren());
        }
	}

    // Used to create child objects that inherit parent properties
    private void Initialize(Fractal parent, int childIndex) {
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
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);

        // Rotate the right and left children's up so that parent's up is not in line
        transform.localRotation = childOrientations[childIndex];
    }

    // Method to pass to StartCoroutine to watch the child objects get created recursively
    private IEnumerator CreateChildren() {
        for (int i = 0; i < childDirections.Length; i++) {
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
        }
    }

}
