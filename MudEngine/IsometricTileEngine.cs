using System;
using System.Collections.Generic;
using System.Linq;

namespace MudEngine
{
    public class IsometricTileEngine
    {

        public Sprite characterSprite;
        public List<Sprite> Sprites = new List<Sprite>();
        public int GridWidth;
        public int GridHeight;
        public Map map;

        public IsometricTileEngine(int width, int height)
        {
            GridWidth = width;
            GridHeight = height;
            map = new Map();
            map.Layers.Add(new Layer());
        }


      

        public bool CollisionDetection(int x, int y, Direction direction)
        {
            bool result = false;

            Sprite Sprite = null;

            switch (direction)
            {
                case Direction.down:
                    Sprite = Sprites.Where(i => i.X == x && i.Y == y + 1).FirstOrDefault();
                    break;
                case Direction.up:
                    Sprite = Sprites.Where(i => i.X == x && i.Y == y - 1).FirstOrDefault();
                    break;
                case Direction.left:
                    Sprite = Sprites.Where(i => i.X == x - 1 && i.Y == y).FirstOrDefault();
                    break;
                case Direction.right:
                    Sprite = Sprites.Where(i => i.X == x + 1 && i.Y == y).FirstOrDefault();
                    break;
            }
            if (Sprite != null)
            {
                result = true;
            }
            return result;
        }

        public bool CanMoveSprites(List<Sprite> Sprites, Direction direction)
        {
            bool result = true;

            foreach (Sprite Sprite in Sprites)
            {
                if (!CanMove(Sprite.X, Sprite.Y, direction))
                {
                    result = false;
                }
            }
            return result;
        }

        public bool CanMove(int x, int y, Direction direction)
        {
            bool result = false;

            Sprite Sprite = null;

            Sprite currentSprite = Sprites.Where(i => i.X == x && i.Y == y).FirstOrDefault();

            switch (direction)
            {
                case Direction.down:
                    Sprite = Sprites.Where(i => i.X == x && i.Y == y + 1).FirstOrDefault();
                    if (y + 1 == GridHeight || currentSprite.CanMove == false)
                    {
                        return false;
                    }
                    break;
                case Direction.up:
                    Sprite = Sprites.Where(i => i.X == x && i.Y == y - 1).FirstOrDefault();
                    if (y - 1 == 0 || currentSprite.CanMove == false)
                    {
                        return false;
                    }
                    break;
                case Direction.left:
                    Sprite = Sprites.Where(i => i.X == x - 1 && i.Y == y).FirstOrDefault();
                    if (x - 1 == GridWidth || currentSprite.CanMove == false)
                    {
                        return false;
                    }
                    break;
                case Direction.right:
                    Sprite = Sprites.Where(i => i.X == x + 1 && i.Y == y).FirstOrDefault();
                    if (x + 1 == GridWidth || currentSprite.CanMove == false)
                    {
                        return false;
                    }
                    break;
            }
            if (Sprite != null)
            {
                result = Sprite.CanMove;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public List<Sprite> GetAllSpritesInPath(int x, int y, Direction direction)
        {
            List<Sprite> result = new List<Sprite>();

            switch (direction)
            {
                case Direction.down:
                    result = Sprites.Where(i => i.X == x && i.Y > y).ToList().OrderBy(i => i.Y).ToList();
                    break;
                case Direction.up:
                    result = Sprites.Where(i => i.X == x && i.Y < y).ToList().OrderByDescending(i => i.Y).ToList();
                    break;
                case Direction.left:
                    result = Sprites.Where(i => i.X < x && i.Y == y).ToList().OrderByDescending(i => i.X).ToList();
                    break;
                case Direction.right:
                    result = Sprites.Where(i => i.X > x && i.Y == y).ToList().OrderBy(i => i.X).ToList();
                    break;
            }
            List<Sprite> result2 = new List<Sprite>();
            Sprite lastSprite = null;
            int index = 0;
            switch (direction)
            {
                case Direction.down:
                    foreach (Sprite Sprite in result)
                    {
                        if (!CollisionDetection(Sprite.X, Sprite.Y, Direction.down))
                        {
                            if (lastSprite == null)
                            {
                                lastSprite = Sprite;
                            }
                        }
                    }
                    index = result.IndexOf(lastSprite);
                    result2 = result.Take(index + 1).ToList();
                    break;
                case Direction.up:
                    foreach (Sprite Sprite in result)
                    {
                        if (!CollisionDetection(Sprite.X, Sprite.Y, Direction.up))
                        {
                            if (lastSprite == null)
                            {
                                lastSprite = Sprite;
                            }
                        }
                    }
                    index = result.IndexOf(lastSprite);
                    result2 = result.Take(index + 1).ToList();
                    break;
                case Direction.left:
                    foreach (Sprite Sprite in result)
                    {
                        if (!CollisionDetection(Sprite.X, Sprite.Y, Direction.left))
                        {
                            if (lastSprite == null)
                            {
                                lastSprite = Sprite;
                            }
                        }
                    }
                    index = result.IndexOf(lastSprite);
                    result2 = result.Take(index + 1).ToList();
                    break;
                case Direction.right:
                    foreach (Sprite Sprite in result)
                    {
                        if (!CollisionDetection(Sprite.X, Sprite.Y, Direction.right))
                        {
                            if (lastSprite == null)
                            {
                                lastSprite = Sprite;
                            }
                        }
                    }
                    index = result.IndexOf(lastSprite);
                    result2 = result.Take(index + 1).ToList();
                    break;
            }


            return result2;
        }
    }

    public enum Direction
    {
        none,
        up,
        down,
        left,
        right
    }
}
