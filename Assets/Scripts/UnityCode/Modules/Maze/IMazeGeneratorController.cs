using MazeGenerator;
using Modules.Maze.Impl;

namespace Modules.Maze
{
    public interface IMazeGeneratorController
    {
        public IMaze GenerateMaze(int width, int height, MazeGeneratorType type);
    }
}