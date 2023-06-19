using Components.Maze;
using UnityEngine;
using Vector2 = MazeGenerator.Vector2;

namespace Modules.Maze
{
    public interface IMazePresentationController
    {
        void RegisterMazeView(MazeView view);
        Vector3 GetWorldPosByCell(Vector2 cell);
        Vector3 GetWorldPosByCell(int width, int length);
        Vector3 GetEntrancePos();
        Vector3 GetExitPos();
        
        Vector2 GetCellByWorldPos(Vector3 worldPos);
    }
}