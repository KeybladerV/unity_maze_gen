namespace MazeGenerator
{
    public sealed class PathNode
    {
        public int GCost;
        public int HCost;
        public Vector2 Coordinates;

        public PathNode LinkedNode;
        
        public int FCost => GCost + HCost;

        public override string ToString() => Coordinates.ToString();
    }
}