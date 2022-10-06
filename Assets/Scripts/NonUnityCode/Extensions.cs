using System;

namespace MazeGenerator
{
    using UnityEngine;

    public static class Extensions
    {
        public static WallType GetOpposite(this WallType wallType)
        {
            switch (wallType)
            {
                case WallType.Up:
                    return WallType.Down;
                case WallType.Down:
                    return WallType.Up;
                case WallType.Left:
                    return WallType.Right;
                case WallType.Right:
                    return WallType.Left;
                default:
                    return WallType.Left;
            }
        }

        public static WallType ToWallType(this MoveDir moveDir)
        {
            switch (moveDir)
            {
                case MoveDir.Left:
                    return WallType.Left;
                case MoveDir.Right:
                    return WallType.Right;
                case MoveDir.Up:
                    return WallType.Up;
                case MoveDir.Down:
                    return WallType.Down;
                default:
                    return WallType.Left;
            }
        }

        public static Vector3 ChangePos(this Vector3 pos, WallType type, float amount)
        {
            switch (type)
            {
                case WallType.Up:
                    return pos + new Vector3(0, 0, amount);
                case WallType.Down:
                    return pos + new Vector3(0, 0, -amount);
                case WallType.Left:
                    return pos + new Vector3(-amount, 0, 0);
                case WallType.Right:
                    return pos + new Vector3(amount, 0, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}