namespace MazeGenerator
{
    public struct Vector2
    {
        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Shorthands
        
        /// <summary>
        ///   <para>Shorthand for writing Vector2Int(0, 0).</para>
        /// </summary>
        public static Vector2 zero => s_Zero;

        /// <summary>
        ///   <para>Shorthand for writing Vector2Int(1, 1).</para>
        /// </summary>
        public static Vector2 one => s_One;

        /// <summary>
        ///   <para>Shorthand for writing Vector2Int(0, 1).</para>
        /// </summary>
        public static Vector2 up => s_Up;

        /// <summary>
        ///   <para>Shorthand for writing Vector2Int(0, -1).</para>
        /// </summary>
        public static Vector2 down => s_Down;

        /// <summary>
        ///   <para>Shorthand for writing Vector2Int(-1, 0).</para>
        /// </summary>
        public static Vector2 left => s_Left;

        /// <summary>
        ///   <para>Shorthand for writing Vector2Int(1, 0).</para>
        /// </summary>
        public static Vector2 right => s_Right;
        
        private static readonly Vector2 s_Zero = new(0, 0);
        private static readonly Vector2 s_One = new(1, 1);
        private static readonly Vector2 s_Up = new(0, 1);
        private static readonly Vector2 s_Down = new(0, -1);
        private static readonly Vector2 s_Left = new(-1, 0);
        private static readonly Vector2 s_Right = new(1, 0);
        
        #endregion

        #region Operators Overloads
        
        public static bool operator ==(Vector2 vec1, Vector2 vec2) => vec1.X == vec2.X && vec1.Y == vec2.Y;
        public static bool operator !=(Vector2 vec1, Vector2 vec2) => !(vec1 == vec2);
        public static Vector2 operator +(Vector2 vec1, Vector2 vec2) => new(vec1.X + vec2.X, vec1.Y + vec2.Y);
        public static Vector2 operator -(Vector2 vec1, Vector2 vec2) => new(vec1.X - vec2.X, vec1.Y - vec2.Y);
        public static Vector2 operator *(Vector2 vec1, Vector2 vec2) => new(vec1.X * vec2.X, vec1.Y * vec2.Y);
        public static Vector2 operator /(Vector2 vec1, Vector2 vec2) => new(vec1.X / vec2.X, vec1.Y / vec2.Y);
        public static Vector2 operator %(Vector2 vec1, Vector2 vec2) => new(vec1.X % vec2.X, vec1.Y % vec2.Y);
        public static Vector2 operator -(Vector2 vec) => new(-vec.X, -vec.Y);
        public static Vector2 operator +(Vector2 vec) => new(+vec.X, +vec.Y);
        public static Vector2 operator ++(Vector2 vec) => new(vec.X + 1, vec.Y + 1);
        public static Vector2 operator --(Vector2 vec) => new(vec.X - 1, vec.Y - 1);
        
        #endregion

        #region Object overrides

        public override bool Equals(object obj)
        {
            if (obj is Vector2 vec)
                return this == vec;
            return false;
        }
        
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
        public override string ToString() => "({X}, {Y})";

        #endregion
        
    }
}