using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace MazeGenerator
{
    public class MazeGeneration : MonoBehaviour
    {
        [SerializeField] private int _mazeWidth = 10;
        [SerializeField] private int _mazeLength = 10;
        
        [SerializeField] private MazeView mazeViewPrefab;

        [Button]
        private void GenerateAndDrawMaze()
        {
            var maze = ScriptableObject.CreateInstance<MazeScriptableObject>();
            maze.Init(_mazeWidth, _mazeLength);
            MazeMaker.MakeMaze(maze);
            SaveMazeToAssets(maze);
            
            var mazeView = Instantiate(mazeViewPrefab, transform);
            mazeView.SetMaze(maze);
            mazeView.DoDrawing();
        }

        private void SaveMazeToAssets(MazeScriptableObject maze)
        {
            if(!AssetDatabase.IsValidFolder("Assets/MazeContainers"))
                AssetDatabase.CreateFolder("Assets", "MazeContainers");
            var uniqueName = AssetDatabase.GenerateUniqueAssetPath($"Assets/MazeContainers/MazeW{maze.Width}L{maze.Length}.asset");
            AssetDatabase.CreateAsset(maze, uniqueName);
            AssetDatabase.SaveAssets();
        }
    }
}