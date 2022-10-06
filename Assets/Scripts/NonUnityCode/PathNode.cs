namespace MazeGenerator
{
    public class PathNode
    {
        public int GCost;
        public int FCost;
        public int HCost;
        public Vector2 Coordinates;

        public PathNode LinkedNode;

        public void CalculateFCost() => FCost = GCost + HCost;

        public override string ToString() => Coordinates.ToString();
    }
}