using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.Meshing.Algorithm;

namespace FallingSloth.ProceduralMazeGenerator
{
    public class DelaunayGraph
    {
        List<Vertex> points;

        public Mesh mesh { get; protected set; }

        public Configuration config;

        Dwyer dwyer = new Dwyer();

        public DelaunayGraph()
        {
            dwyer.UseDwyer = true;
            this.config = new Configuration();
        }
        public DelaunayGraph(Configuration config)
        {
            dwyer.UseDwyer = true;
            this.config = config;
        }

        public void AddPoint(params Vertex[] verteces)
        {
            foreach (Vertex vertex in verteces)
            {
                points.Add(vertex);
                Retriangulate();
            }
        }

        public void Retriangulate()
        {
            if (points.Count >= 3)
                mesh = dwyer.Triangulate(points, config) as Mesh;
        }
    }
}
