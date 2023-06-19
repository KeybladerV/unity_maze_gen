namespace MazeGenerator
{
    public static class EnumExtensions
    {
        public static CellType GetOpposite(this CellType cellType) =>
            cellType switch
            {
                CellType.Up => CellType.Down,
                CellType.Down => CellType.Up,
                CellType.Left => CellType.Right,
                CellType.Right => CellType.Left,
                _ => CellType.Left
            };


        public static CellType ToWallType(this MoveDir moveDir) =>
            moveDir switch
            {
                MoveDir.Left => CellType.Left,
                MoveDir.Right => CellType.Right,
                MoveDir.Up => CellType.Up,
                MoveDir.Down => CellType.Down,
                _ => CellType.Left
            };
    }
}