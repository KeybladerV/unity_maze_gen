//Left for backuping some code snippets to be used later
// using System;
// using UnityEngine;
//
// namespace MazeGenerator.UnityCode.Runtime
// {
//     public class MazeGenerator : MonoBehaviour
//     {
//         [SerializeField] private MazeView _mazeViewPrefab;
//         
//         public void GenerateAndDrawMaze(int width, int length)
//         {
//             IMaze maze = new Maze(width, length);
//             
//             var mazeGenerator = new BacktrackingGenerator();
//             mazeGenerator.Generate(maze);
//             
//             var mazeView = Instantiate(_mazeViewPrefab, transform);
//             mazeView.SetMaze(maze);
//             mazeView.DoDrawing();
//         }
//
//         // #if UNITY_EDITOR
//         // private void SaveMazeToAssets(MazeScriptableObject maze)
//         // {
//         //     if(!AssetDatabase.IsValidFolder("Assets/MazeContainers"))
//         //         AssetDatabase.CreateFolder("Assets", "MazeContainers");
//         //     var uniqueName = AssetDatabase.GenerateUniqueAssetPath($"Assets/MazeContainers/MazeW{maze.Width}L{maze.Length}.asset");
//         //     AssetDatabase.CreateAsset(maze, uniqueName);
//         //     AssetDatabase.SaveAssets();
//         // }
//         // #endif
//     }
// }