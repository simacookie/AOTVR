using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rope : MonoBehaviour
{
	// Start is called before the first frame update

	[SerializeField]
	float length;
	float lengthToUse;
    [SerializeField]
    GameObject anchorGO;

	MeshFilter meshFilter;
    void Start()
	{
		RenderRope();
	}

	private void RenderRope()
	{

		length = (transform.parent.position - anchorGO.transform.position).magnitude;  
		meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = new Mesh(); ;
		// scale length to fit real world scale
		lengthToUse = length / transform.localScale.x;

		transform.parent.parent.LookAt(anchorGO.transform);

		List<Vector3> newVerticesList = new List<Vector3>();
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(20, -98), new Vector3(-20, -98)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-20, -98), new Vector3(-56, -83)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-56, -83), new Vector3(-83, -56)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-83, -56), new Vector3(-98, -20)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-98, -20), new Vector3(-98, 20)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-98, 20), new Vector3(-83, 56)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-83, 56), new Vector3(-56, 83)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-56, 83), new Vector3(-20, 98)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(-20, 98), new Vector3(20, 98)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(20, 98), new Vector3(56, 83)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(56, 83), new Vector3(83, 56)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(83, 56), new Vector3(98, 20)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(98, 20), new Vector3(98, -20)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(98, -20), new Vector3(83, -56)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(83, -56), new Vector3(56, -83)));
		newVerticesList.AddRange(CreateRectangleVertices(new Vector3(56, -83), new Vector3(20, -98)));
		Vector3[] newVertices = newVerticesList.ToArray();
		////new Vector3(20,-98),
		////new Vector3(-20,-98),
		////new Vector3(-56,-83),
		////new Vector3(-83,-56),
		////new Vector3(-98,-20),
		////new Vector3(-98,20),
		//new Vector3(-83,56),
		//new Vector3(-56,83),
		//new Vector3(-20,98),
		//new Vector3(20,98),
		//new Vector3(56,83),
		//new Vector3(83,56),
		//new Vector3(98,20),
		//new Vector3(98,-20),
		//new Vector3(83,-56),
		//new Vector3(56,-83) 
		List<Vector2> newUVList = new List<Vector2>();
		for (int i = 0; i < 16; i++)
		{
			newUVList.AddRange(CreateUVsForRectangle(0));

		}
		Vector2[] newUV = new Vector2[newVertices.Length];
		int numberOfVertices = 96;
		int[] newTriangles = new int[numberOfVertices];

		for (int i = 0; i < numberOfVertices; i++)
		{
			newTriangles[i] = i;
		}
		for (int i = 0; i < newUV.Length; i++)
		{
			newUV[i] = new Vector2(newVertices[i].x, newVertices[i].z);
		}
		Mesh meshFilterMesh = meshFilter.mesh;
		meshFilterMesh.vertices = newVertices;
		meshFilterMesh.uv = newUV;
		meshFilterMesh.triangles= newTriangles;
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, - length );
	}

	// Update is called once per frame
	void Update()
    {
		RenderRope();
    }
    private Vector3[] CreateRectangleVertices(Vector3 vert1, Vector3 vert2)
    {

		return new Vector3[] {
            vert2 + new Vector3(0, 0, lengthToUse),
            vert2,
            vert1,
            vert1 + new Vector3(0, 0, lengthToUse),
            vert2 + new Vector3(0, 0, lengthToUse),
            vert1,
        };
    }
    private Vector2[] CreateUVsForRectangle(int rectIndex)
    {
		float rectWidth = 40;
		float newXPos = 20 - (rectWidth * rectIndex);
		float newXPos2 = newXPos - 40;
        return new Vector2[] {
			new Vector2(newXPos2, - lengthToUse),
			new Vector2(newXPos2, 0),
			new Vector2(newXPos, 0),
			new Vector2(newXPos, -lengthToUse),
			new Vector2(newXPos2, -lengthToUse),
			new Vector2(newXPos, 0),
		};
    }
}
