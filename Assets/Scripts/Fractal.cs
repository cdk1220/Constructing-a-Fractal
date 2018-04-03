using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;

    public int maxDepth;                // Indicates the max number of levels to go down recursively
    private int currentDepth = 0;       // Indicates the depth which the current object is at   

    public float childScale;            // How much to scale the child by

    public float spawnProbabaility;     // Make the fractal look more organic

    public float maxRotationSpeed;      
    private float rotationSpeed;

    public Mesh[] meshes;               // Help randomize meshes

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

    // Array to hold materials varying with the depth
    private Material[, ] materials;
     
	// Use this for initialization
	private void Start () {

        // Rotation speed for the this game object
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);

        // Create materials array if not created already
        if (materials == null) {
            InitializeMaterials();
        }
        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[currentDepth, Random.Range(0, 2)];

        // If less than maximum  depth, create more children growing up, to the right, and left
        if (currentDepth < maxDepth) {
            StartCoroutine(CreateChildren());
        }
	}

    // Used to create child objects that inherit parent properties
    private void Initialize(Fractal parent, int childIndex) {
        meshes = parent.meshes;
        materials = parent.materials;

        maxDepth = parent.maxDepth;
        currentDepth = parent.currentDepth + 1;

        childScale = parent.childScale;

        spawnProbabaility = parent.spawnProbabaility;

        maxRotationSpeed = parent.maxRotationSpeed;

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

            // Cutoff branches in a random fashion
            if (Random.value < spawnProbabaility) {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
        }
    }

    // This handles the creation of materials varying with depth
    private void InitializeMaterials() {
        materials = new Material[maxDepth + 1, 2];

        for (int i = 0; i <= maxDepth; i++) {
            float weight = i / (maxDepth - 1f);
            weight *= weight;

            materials[i, 0] = new Material(material);
            materials[i, 1] = new Material(material);

            // First color progression
            materials[i, 0].color =
                Color.Lerp(Color.white, Color.yellow, weight);

            // Second color progression
            materials[i, 1].color =
                Color.Lerp(Color.white, Color.cyan, weight);
        }

        materials[maxDepth, 0].color = Color.magenta;
        materials[maxDepth, 1].color = Color.red;
    }

    private void Update() {
        transform.Rotate(0f, rotationSpeed * 30f * Time.deltaTime, 0f);
    }
}
