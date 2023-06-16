using System.Collections.Generic;
using System.Threading.Tasks;

namespace MazeGenerator
{
    public interface IPathfinder
    {
        Task<Queue<PathNode>> GetRouteAsync(IMaze maze, Vector2 startPos, Vector2 endPos);
        Queue<PathNode> GetRoute(IMaze maze, Vector2 startPos, Vector2 endPos);
    }
}