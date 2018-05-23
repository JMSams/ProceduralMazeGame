using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TriangleNet.Geometry;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class GraphVisualizer : MonoBehaviour
    {
        DelaunayGraph graph;

        public RectTransform pointPrefab;

        List<RectTransform> points;

        public MeshFilter meshFilter;

        void Start()
        {
            graph = new DelaunayGraph();
        }

        public void AddRandomPoint()
        {
            Vertex newVertex = new Vertex(Random.Range(1, 640), Random.Range(1, 336));
            graph.AddPoint(newVertex);

            RectTransform temp = Instantiate(pointPrefab, this.transform);
            temp.localPosition = new Vector3((float)newVertex.X, (float)newVertex.Y);
            points.Add(temp);
            
        }
    }
}