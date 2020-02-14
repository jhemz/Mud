using System.Collections.Generic;

namespace MudEngine
{
    public class Map
    {
        public Map()
        {
            Layers = new List<Layer>();
        }
        public List<Layer> Layers { get; set; }
    }
    public class Layer
    {
        //public Layer(int width, int height)
        //{
        //    Array = new int[width, height];
        //    for (int x = 5; x < 18; x++)
        //    {
        //        for (int y = 5; y < 18; y++)
        //        {
        //            Array[x, y] = 0;
        //        }
        //    }
        //}

        public int[,] Array { get; set; }
    }
}
