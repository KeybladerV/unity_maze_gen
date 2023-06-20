using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Injection;
using Components.Screens.Main;
using MazeGenerator;

namespace Modules.Maze.Impl
{
    public enum MazeGeneratorType
    {
        Backtracking
    }
    public sealed class MazeGeneratorController : IMazeGeneratorController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        
        private Dictionary<MazeGeneratorType, IMazeGenerator> _generators;
        
        [PostConstruct]
        public void OnPostConstruct()
        {
            _generators = new Dictionary<MazeGeneratorType, IMazeGenerator>();
            Dispatcher.AddListener(MainScreenEvent.OnNewMaze, OnNewMaze);
        }

        private void OnNewMaze(Vector2 data)
        {
            GenerateMaze(data.X, data.Y, MazeGeneratorType.Backtracking);
        }

        public IMaze GenerateMaze(int width, int height, MazeGeneratorType type)
        {
            if(!_generators.TryGetValue(type, out var generator))
                _generators[type] = generator = CreateGenerator(type);
            
            IMaze maze = new MazeGenerator.Maze(width, height);
            generator.Generate(maze);

            Dispatcher.Dispatch(MazeGeneratorEvents.OnMazeGenerated, maze);
            return maze;
        }

        private IMazeGenerator CreateGenerator(MazeGeneratorType type)
        {
            return type switch
            {
                MazeGeneratorType.Backtracking => new BacktrackingGenerator(),
                _ => throw new System.NotImplementedException()
            };
        }
    }
}