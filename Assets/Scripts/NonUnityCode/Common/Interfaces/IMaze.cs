namespace MazeGenerator
{
    public interface IMaze
    {
        int Width { get; }
        int Length { get; }
        Vector2 Entrance { get; }
        Vector2 Exit { get; }
        
        WallType this[int i, int j] { get; set; }
        WallType this[Vector2 coord] { get; set; }

        void SetEntrance(Vector2 entrance);
        void SetExit(Vector2 exit);

        void Reset();
    }
}