using System;
using System.Collections.Generic;
using System.Text;

namespace MudEngine
{
    public class Level
    {
        public IsometricTileEngine Engine;
        public Map Map;

        public Level(IsometricTileEngine engine)
        {
            Engine = engine;
        }
    }
}
