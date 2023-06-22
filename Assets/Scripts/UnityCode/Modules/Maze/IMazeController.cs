using MazeGenerator;

namespace Modules.Maze
{
    public interface IMazeController
    {
        Vector2 GetEntrance();
        Vector2 GetExit();
    }
}