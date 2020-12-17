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

        public event EventHandler<UpdateX_EventArgs> UpdateX_Event;
        public event EventHandler<UpdateY_EventArgs> UpdateY_Event;
        public event EventHandler<AddNewSprite_EventArgs> AddNewSprite_Event;
        public event EventHandler<RemoveSprite_EventArgs> RemoveSprite_Event;
        public event EventHandler LoseLife_Event;
        public event EventHandler<ChangeSprite_EventArgs> ChangeSprite_Event;

        public IsometricTileEngine(int width, int height)
        {
            GridWidth = width;
            GridHeight = height;
            map = new Map();
            map.Layers.Add(new Layer());
        }

        public Tuple<int, int> GetRandomFreeLocation()
        {
            List<Tuple<int, int>> emptySpaces = new List<Tuple<int, int>>();

            for (int y = 0; y < GridWidth; y++)
            {
                for (int x = 0; x < GridHeight; x++)
                {
                    if (Sprites.Where(a => a.X == x && a.Y == y).FirstOrDefault() == null)
                    {
                        emptySpaces.Add(new Tuple<int, int>(x, y));
                    }
                }
            }
            Random rnd = new Random();
            int index = rnd.Next(emptySpaces.Count);
            return emptySpaces[index];
        }

        public void MoveCats()
        {
            List<Sprite> Baddies = Sprites.Where(x => x.Ai != null).ToList();
            foreach (Sprite baddie in Baddies)
            {
                Direction directionToMoveBaddie = baddie.Ai.Move(baddie.X, baddie.Y);
                switch (directionToMoveBaddie)
                {
                    case Direction.left:
                        UpdateX_Event?.Invoke(this, new UpdateX_EventArgs(baddie, -1));
                        if (KilledMouse(baddie))
                        {
                            LoseLife_Event?.Invoke(this, new EventArgs());
                        }
                        break;
                    case Direction.right:
                        UpdateX_Event?.Invoke(this, new UpdateX_EventArgs(baddie, 1));
                        if (KilledMouse(baddie))
                        {
                            LoseLife_Event?.Invoke(this, new EventArgs());
                        }
                        break;
                    case Direction.up:
                        UpdateY_Event?.Invoke(this, new UpdateY_EventArgs(baddie, -1));
                        if (KilledMouse(baddie))
                        {
                            LoseLife_Event?.Invoke(this, new EventArgs());
                        }
                        break;
                    case Direction.down:
                        UpdateY_Event?.Invoke(this, new UpdateY_EventArgs(baddie, 1));
                        if (KilledMouse(baddie))
                        {
                            LoseLife_Event?.Invoke(this, new EventArgs());
                        }
                        break;
                    case Direction.none:
                        //cat died
                        if (baddie.Lives > 0)
                        {
                            baddie.Lives--;
                        }
                        if (!Baddies.Where(a => a.Lives > 0).ToList().Any())
                        {
                            int _x = baddie.X;
                            int _y = baddie.Y;
                            RemoveSprite_Event?.Invoke(this, new RemoveSprite_EventArgs(baddie));
                            AddNewSprite_Event?.Invoke(this, new AddNewSprite_EventArgs(new Sprite() { X = _x, Y = _y }));
                        }
                        else
                        {
                            ChangeSprite_Event?.Invoke(this, new ChangeSprite_EventArgs(baddie));
                        }

                        break;
                }
            }
        }

        public bool KilledMouse(Sprite baddie)
        {
            bool result = false;
            if (baddie.X == characterSprite.X && baddie.Y == characterSprite.Y)
            {
                result = true;
            }
            return result;
        }

        public bool MoveSprites(Direction direction)
        {
            bool canMove = false;
            if (CollisionDetection(characterSprite.X, characterSprite.Y, direction))
            {
                List<Sprite> Sprites = GetAllSpritesInPath(characterSprite.X, characterSprite.Y, direction);

                switch (direction)
                {
                    case Direction.down:
                        if (CanMoveSprites(Sprites, Direction.down))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.up:
                        if (CanMoveSprites(Sprites, Direction.up))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.left:
                        if (CanMoveSprites(Sprites, Direction.left))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.right:
                        if (CanMoveSprites(Sprites, Direction.right))
                        {
                            canMove = true;
                        }
                        break;
                }

                if (canMove)
                {
                    foreach (Sprite sprite in Sprites)
                    {
                        switch (direction)
                        {
                            case Direction.down:
                                UpdateY_Event?.Invoke(this, new UpdateY_EventArgs(sprite, 1));
                                break;
                            case Direction.up:
                                UpdateY_Event?.Invoke(this, new UpdateY_EventArgs(sprite, -1));
                                break;
                            case Direction.left:
                                UpdateX_Event?.Invoke(this, new UpdateX_EventArgs(sprite, -1));
                                break;
                            case Direction.right:
                                UpdateX_Event?.Invoke(this, new UpdateX_EventArgs(sprite, 1));
                                break;
                        }
                    }
                }
            }
            else
            {
                canMove = true;
            }
            return canMove;
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
