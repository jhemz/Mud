using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Block> blocks = new List<Block>();

        int GridWidth = 23;
        int GridHeight = 23;

        Mouse mouse;
        public MainWindow()
        {
            InitializeComponent();


            for (int y = 0; y < 23; y++)
            {
                Block bBlock = new Block(false) { X = 0, Y = y };
                blocks.Add(bBlock);
                Grid.SetRow(bBlock, y);
                Grid.SetColumn(bBlock, 0);
                main.Children.Add(bBlock);
                Block bBlock2 = new Block(false) { X = 22, Y = y };
                blocks.Add(bBlock2);
                Grid.SetRow(bBlock2, y);
                Grid.SetColumn(bBlock2, 22);
                main.Children.Add(bBlock2);
            }
            for (int x = 1; x < 22; x++)
            {
                Block bBlock = new Block(false) { X = x, Y = 0 };
                blocks.Add(bBlock);
                Grid.SetRow(bBlock, 0);
                Grid.SetColumn(bBlock, x);
                main.Children.Add(bBlock);
                Block bBlock2 = new Block(false) { X = x, Y = 22 };
                blocks.Add(bBlock2);
                Grid.SetRow(bBlock2, 22);
                Grid.SetColumn(bBlock2, x);
                main.Children.Add(bBlock2);
            }

            for (int x = 5; x < 18; x++)
            {
                for (int y = 5; y < 18; y++)
                {
                    if (x == 11 && y == 11)
                    {
                        mouse = new Mouse() { X = x, Y = y };
                        Grid.SetRow(mouse, y);
                        Grid.SetColumn(mouse, x);
                        main.Children.Add(mouse);
                    }
                    else if (x == 12 && y == 11)
                    {
                        Block blockNoMove = new Block(false) { X = x, Y = y };
                        blocks.Add(blockNoMove);
                        Grid.SetRow(blockNoMove, y);
                        Grid.SetColumn(blockNoMove, x);
                        main.Children.Add(blockNoMove);
                    }
                    else
                    {
                        Block block = new Block() { X = x, Y = y };
                        blocks.Add(block);
                        Grid.SetRow(block, y);
                        Grid.SetColumn(block, x);
                        main.Children.Add(block);
                    }

                }
            }

        }

      

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (MoveBlocks(Direction.left))
                {
                    int x = mouse.X - 1;
                    Grid.SetColumn(mouse, x);
                    mouse.X = x;
                }
            }
            if (e.Key == Key.Right)
            {
                if (MoveBlocks(Direction.right))
                {
                    int x = mouse.X + 1;
                    Grid.SetColumn(mouse, x);
                    mouse.X = x;
                }
            }
            if (e.Key == Key.Up)
            {
                if (MoveBlocks(Direction.up))
                {
                    int y = mouse.Y - 1;
                    Grid.SetRow(mouse, y);
                    mouse.Y = y;
                }
            }
            if (e.Key == Key.Down)
            {
                if (MoveBlocks(Direction.down))
                {
                    int y = mouse.Y + 1;
                    Grid.SetRow(mouse, y);
                    mouse.Y = y;
                }
            }
        }
        
        
        private bool MoveBlocks(Direction direction)
        {
            bool canMove = false;
            if (CollisionDetection(mouse.X, mouse.Y, direction))
            {
                List<Block> blocks = GetAllBlocksInPath(mouse.X, mouse.Y, direction);

                switch (direction)
                {
                    case Direction.down:
                        if (CanMoveBlocks(blocks, Direction.down))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.up:
                        if (CanMoveBlocks(blocks, Direction.up))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.left:
                        if (CanMoveBlocks(blocks, Direction.left))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.right:
                        if (CanMoveBlocks(blocks, Direction.right))
                        {
                            canMove = true;
                        }
                        break;
                }


                if (canMove)
                {
                    foreach (Block block in blocks)
                    {
                        int _x = 0;
                        int _y = 0;
                        switch (direction)
                        {
                            case Direction.down:
                                _y = block.Y + 1;
                                Grid.SetRow(block, _y);
                                block.Y = _y;
                                break;
                            case Direction.up:
                                _y = block.Y - 1;
                                Grid.SetRow(block, _y);
                                block.Y = _y;
                                break;
                            case Direction.left:
                                _x = block.X - 1;
                                Grid.SetColumn(block, _x);
                                block.X = _x;
                                break;
                            case Direction.right:
                                _x = block.X + 1;
                                Grid.SetColumn(block, _x);
                                block.X = _x;
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

        private bool CollisionDetection(int x, int y, Direction direction)
        {
            bool result = false;

            Block block = null;

            switch (direction)
            {
                case Direction.down:
                    block = blocks.Where(i => i.X == x && i.Y == y + 1).FirstOrDefault();
                    break;
                case Direction.up:
                    block = blocks.Where(i => i.X == x && i.Y == y - 1).FirstOrDefault();
                    break;
                case Direction.left:
                    block = blocks.Where(i => i.X == x - 1 && i.Y == y).FirstOrDefault();
                    break;
                case Direction.right:
                    block = blocks.Where(i => i.X == x + 1 && i.Y == y).FirstOrDefault();
                    break;
            }
            if (block != null)
            {
                result = true;
            }
            return result;
        }

        private bool CanMoveBlocks(List<Block> blocks, Direction direction)
        {
            bool result = true;

            foreach (Block block in blocks)
            {
                if (!CanMove(block.X, block.Y, direction))
                {
                    result = false;
                }
            }
            return result;
        }

        private bool CanMove(int x, int y, Direction direction)
        {
            bool result = false;

            Block block = null;

            Block currentBlock = blocks.Where(i => i.X == x && i.Y == y).FirstOrDefault();

            switch (direction)
            {
                case Direction.down:
                    block = blocks.Where(i => i.X == x && i.Y == y + 1).FirstOrDefault();
                    if(y + 1 == GridHeight || currentBlock.CanMove == false)
                    {
                        return false;
                    }
                    break;
                case Direction.up:
                    block = blocks.Where(i => i.X == x && i.Y == y - 1).FirstOrDefault();
                    if (y - 1 == 0 || currentBlock.CanMove == false)
                    {
                        return false;
                    }
                    break;
                case Direction.left:
                    block = blocks.Where(i => i.X == x - 1 && i.Y == y).FirstOrDefault();
                    if (x - 1 == GridWidth || currentBlock.CanMove == false)
                    {
                        return false;
                    }
                    break;
                case Direction.right:
                    block = blocks.Where(i => i.X == x + 1 && i.Y == y).FirstOrDefault();
                    if (x + 1 == GridWidth || currentBlock.CanMove == false)
                    {
                        return false;
                    }
                    break;
            }
            if (block != null)
            {
                result = block.CanMove;
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool EdgeCollsion(int x, int y, Direction direction)
        {
            bool result = false;
            switch (direction)
            {
                case Direction.down:
                    if (y + 1 == 22)
                    {
                        result = true;
                    }
                    break;
                case Direction.up:
                    if (y - 1 == 0)
                    {
                        result = true;
                    }
                    break;
                case Direction.left:
                    if (x - 1 == 0)
                    {
                        result = true;
                    }
                    break;
                case Direction.right:
                    if (x + 1 == 22)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        private List<Block> GetAllBlocksInPath(int x, int y, Direction direction)
        {
            List<Block> result = new List<Block>();

            switch (direction)
            {
                case Direction.down:
                    result = blocks.Where(i => i.X == x && i.Y > y).ToList().OrderBy(i => i.Y).ToList();
                    break;
                case Direction.up:
                    result = blocks.Where(i => i.X == x && i.Y < y).ToList().OrderByDescending(i => i.Y).ToList();
                    break;
                case Direction.left:
                    result = blocks.Where(i => i.X < x && i.Y == y).ToList().OrderByDescending(i => i.X).ToList();
                    break;
                case Direction.right:
                    result = blocks.Where(i => i.X > x && i.Y == y).ToList().OrderBy(i => i.X).ToList();
                    break;
            }
            List<Block> result2 = new List<Block>();
            Block lastBlock = null;
            int index = 0;
            switch (direction)
            {
                case Direction.down:
                    foreach (Block block in result)
                    {
                        if (!CollisionDetection(block.X, block.Y, Direction.down))
                        {
                            if (lastBlock == null)
                            {
                                lastBlock = block;
                            }
                        }
                    }
                    index = result.IndexOf(lastBlock);
                    result2 = result.Take(index + 1).ToList();
                    break;
                case Direction.up:
                    foreach (Block block in result)
                    {
                        if (!CollisionDetection(block.X, block.Y, Direction.up))
                        {
                            if (lastBlock == null)
                            {
                                lastBlock = block;
                            }
                        }
                    }
                    index = result.IndexOf(lastBlock);
                    result2 = result.Take(index + 1).ToList();
                    break;
                case Direction.left:
                    foreach (Block block in result)
                    {
                        if (!CollisionDetection(block.X, block.Y, Direction.left))
                        {
                            if (lastBlock == null)
                            {
                                lastBlock = block;
                            }
                        }
                    }
                    index = result.IndexOf(lastBlock);
                    result2 = result.Take(index + 1).ToList();
                    break;
                case Direction.right:
                    foreach (Block block in result)
                    {
                        if (!CollisionDetection(block.X, block.Y, Direction.right))
                        {
                            if (lastBlock == null)
                            {
                                lastBlock = block;
                            }
                        }
                    }
                    index = result.IndexOf(lastBlock);
                    result2 = result.Take(index + 1).ToList();
                    break;
            }


            return result2;
        }

        enum Direction
        {
            up,
            down,
            left,
            right
        }
    }
}
