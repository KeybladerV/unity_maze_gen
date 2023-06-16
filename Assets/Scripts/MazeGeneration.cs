using System;
using UnityEngine;

namespace MazeGenerator
{
    public class MazeGeneration : MonoBehaviour
    {
        [SerializeField] private int _mazeWidth = 10;
        [SerializeField] private int _mazeLength = 10;
        
        [SerializeField] private MazeView mazeViewPrefab;
        
        [SerializeField] private MazeView _currentMazeView;
        
        public event Action OnMazeGenerated;
        
        public MazeView CurrentMazeView => _currentMazeView;
        
        public void GenerateAndDrawMaze()
        {
            var maze = ScriptableObject.CreateInstance<MazeScriptableObject>();
            DestroyImmediate(_currentMazeView.gameObject);
            maze.Init(_mazeWidth, _mazeLength);
            
            var mazeGenerator = new BacktrackingGenerator();
            mazeGenerator.Generate(maze);
            // #if UNITY_EDITOR
            // SaveMazeToAssets(maze);
            // #endif
            
            _currentMazeView = Instantiate(mazeViewPrefab, transform);
            _currentMazeView.SetMaze(maze);
            _currentMazeView.DoDrawing();
            
            OnMazeGenerated?.Invoke();
        }
        
        public void GenerateAndDrawMaze(int width, int length)
        {
            _mazeWidth = width;
            _mazeLength = length;
            GenerateAndDrawMaze();
        }

        // #if UNITY_EDITOR
        // private void SaveMazeToAssets(MazeScriptableObject maze)
        // {
        //     if(!AssetDatabase.IsValidFolder("Assets/MazeContainers"))
        //         AssetDatabase.CreateFolder("Assets", "MazeContainers");
        //     var uniqueName = AssetDatabase.GenerateUniqueAssetPath($"Assets/MazeContainers/MazeW{maze.Width}L{maze.Length}.asset");
        //     AssetDatabase.CreateAsset(maze, uniqueName);
        //     AssetDatabase.SaveAssets();
        // }
        // #endif
    }
}