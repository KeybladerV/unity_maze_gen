using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeGenerator.UnityCode.Runtime
{
    public struct MazeMeshes
    {
        public Mesh Floor;
        public Mesh Walls;
    }
    
    [Obsolete("Just for fun. No real use. Impossible to easily change maze with such draw method.")]
    public sealed class MazeDrawer
    {
        public MazeMeshes DrawMaze(IMaze maze)
        {
            return new MazeMeshes
            {
                Floor = DrawFloor(maze.Width, maze.Length),
                Walls = DrawWalls(maze)
            };
        }
        
        private Mesh DrawFloor(int width, int length)
        {
            var vertices = new Vector3[]
            {
                new(0,0,0),
                new(width, 0, 0),
                new(0, 0, length),
                new(width, 0, length),
            };
            var triangles = new[]
            {
                0, 2, 1,
                2, 3, 1
            };
            var uv = new UnityEngine.Vector2[]
            {
                new(0, 0),
                new(1, 0),
                new(0, 1),
                new(1, 1),
            };

            var mesh = new Mesh
            {
                vertices = vertices,
                triangles = triangles,
                uv = uv
            };
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }

        private Mesh DrawWalls(IMaze maze)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            List<UnityEngine.Vector2> uv = new List<UnityEngine.Vector2>();
            
            for (var i = 0; i < maze.Width; i++)
            {
                for (var j = 0; j < maze.Length; j++)
                {
                    Vector2 pos = new Vector2(i, j);
                    CellType cell;
                    if (!maze.TryGetCell(pos, out cell))
                        continue;

                    if ((cell & CellType.Left) != 0)
                    {
                        AddWallMeshData(vertices, triangles, uv, new Vector3(i, 0, j), new Vector3(i, 1, j), new Vector3(i, 0, j + 1));
                    }

                    if ((cell & CellType.Right) != 0)
                    {
                        AddWallMeshData(vertices, triangles, uv, new Vector3(i + 1, 0, j), new Vector3(i + 1, 1, j), new Vector3(i + 1, 0, j + 1));
                    }
                }
            }
            
            for (var j = 0; j < maze.Length; j++)
            {
                for (var i = 0; i < maze.Width; i++)
                {
                    Vector2 pos = new Vector2(i, j);
                    CellType cell;
                    if (!maze.TryGetCell(pos, out cell))
                        continue;

                    if ((cell & CellType.Up) != 0)
                    {
                        AddWallMeshData(vertices, triangles, uv, new Vector3(i, 0, j + 1), new Vector3(i, 1, j + 1), new Vector3(i + 1, 0, j + 1));
                    }

                    if ((cell & CellType.Down) != 0)
                    {
                        AddWallMeshData(vertices, triangles, uv, new Vector3(i, 0, j), new Vector3(i, 1, j), new Vector3(i + 1, 0, j));
                    }
                }
            }
            
            Mesh mesh = new Mesh();
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            return mesh;
        }
        
        private void AddWallMeshData(List<Vector3> vertices, List<int> triangles, List<UnityEngine.Vector2> uv, Vector3 bottomLeft, Vector3 topLeft, Vector3 bottomRight)
        {
            int vertexIndex = vertices.Count;
            vertices.Add(bottomLeft); // bottom left
            vertices.Add(topLeft); // top left
            vertices.Add(bottomRight); // bottom right
            vertices.Add(bottomRight + Vector3.up); // top right

            triangles.Add(vertexIndex); // bottom left
            triangles.Add(vertexIndex + 2); // bottom right
            triangles.Add(vertexIndex + 1); // top left

            triangles.Add(vertexIndex + 2); // bottom right
            triangles.Add(vertexIndex + 3); // top right
            triangles.Add(vertexIndex + 1); // top left

            uv.Add(new UnityEngine.Vector2(bottomLeft.x, bottomLeft.z));
            uv.Add(new UnityEngine.Vector2(topLeft.x, topLeft.z));
            uv.Add(new UnityEngine.Vector2(bottomRight.x, bottomRight.z));
            uv.Add(new UnityEngine.Vector2(bottomRight.x + 1, bottomRight.z));
        }
    }
}