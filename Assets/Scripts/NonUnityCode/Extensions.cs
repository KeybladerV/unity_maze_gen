using System;

namespace MazeGenerator
{
    using UnityEngine;

    public static class Extensions
    {
        public static WallType GetOpposite(this WallType wallType) =>
            wallType switch
            {
                WallType.Up => WallType.Down,
                WallType.Down => WallType.Up,
                WallType.Left => WallType.Right,
                WallType.Right => WallType.Left,
                _ => WallType.Left
            };


        public static WallType ToWallType(this MoveDir moveDir) =>
            moveDir switch
            {
                MoveDir.Left => WallType.Left,
                MoveDir.Right => WallType.Right,
                MoveDir.Up => WallType.Up,
                MoveDir.Down => WallType.Down,
                _ => WallType.Left
            };


        public static Vector3 ChangePos(this Vector3 pos, WallType type, float amount) =>
            type switch
            {
                WallType.Up => pos + new Vector3(0, 0, amount),
                WallType.Down => pos + new Vector3(0, 0, -amount),
                WallType.Left => pos + new Vector3(-amount, 0, 0),
                WallType.Right => pos + new Vector3(amount, 0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
    }
}