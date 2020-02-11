using System;
using System.Collections.Generic;
using System.Text;

namespace MudEngine
{
    public class AI
    {
        IsometricTileEngine engine;

        public Sprite Sprite { get; set; }
        public Sprite Target { get; set; }

        public AI()
        {
            engine = new IsometricTileEngine();
        }

        public Direction Move(int x, int y)
        {
            int prob = 3;

            List<Direction> directions = new List<Direction>();

            if (Target != null)
            {
                if (!engine.CollisionDetection(x, y, Direction.right))
                {
                    directions.Add(Direction.right);
                }
                if (!engine.CollisionDetection(x, y, Direction.left))
                {
                    directions.Add(Direction.left);
                }
                if (!engine.CollisionDetection(x, y, Direction.up))
                {
                    directions.Add(Direction.up);
                }
                if (!engine.CollisionDetection(x, y, Direction.down))
                {
                    directions.Add(Direction.down);
                }
               

                if (Sprite.X < Target.X)
                {
                    if (!engine.CollisionDetection(x, y, Direction.right))
                    {
                        for(int i = 0; i < prob; i++)
                        {
                            directions.Add(Direction.right);
                        }
                    }
                }
                else
                {
                    if (!engine.CollisionDetection(x, y, Direction.left))
                    {
                        for (int i = 0; i < prob; i++)
                        {
                            directions.Add(Direction.left);
                        }
                    }
                }
                if (Sprite.Y < Target.Y)
                {
                    if (!engine.CollisionDetection(x, y, Direction.down))
                    {
                        for (int i = 0; i < prob; i++)
                        {
                            directions.Add(Direction.down);
                        }
                    }
                }
                else
                {
                    if (!engine.CollisionDetection(x, y, Direction.up))
                    {
                        for (int i = 0; i < prob; i++)
                        {
                            directions.Add(Direction.up);
                        }
                    }
                }
            }
            Random rnd = new Random();

            int dir = rnd.Next(0, directions.Count);

            return directions[dir];
        }
    }
}
