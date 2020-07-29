using System;

namespace Utils
{
    public static class Dimensions
    {
        /// <summary>
        /// Returns rectangle dimensions for nice alignment of passed cells 
        /// </summary>
        public static (int, int) Calculate(int cells)
        {
            var cols = (int)Math.Ceiling(Math.Sqrt(cells));
            while (true)
            {
                var rows = (int)Math.Ceiling((double)cells / cols);
                if (cols * rows == cells)
                    return (cols, rows);
                cols += 1;
            }
        }
    }
}