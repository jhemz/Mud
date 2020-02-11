using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MudEngine;

namespace RodentsRevenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IsometricTileEngine engine;

        SpriteItem mouse;
        List<SpriteItem> Blocks = new List<SpriteItem>();
        List<SpriteItem> Baddies = new List<SpriteItem>();
        DispatcherTimer dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            const int width = 23;
            const int height = 23;

            mouse = new SpriteItem(0, 0, @"./Images/borderblock.png");

            Layer layer = new Layer();
            layer.Array = new int[width, height]
            {
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,3,1,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,2,2,2,2,2,2,2,2,2,2,2,2,2,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
                { 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 },
            };
            Level level = new Level(new IsometricTileEngine(23, 23));
            level.Map = new Map();
            level.Map.Layers.Add(layer);
            engine = level.Engine;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    int value = layer.Array[y, x];
                    switch (value)
                    {
                        case 1:
                            SpriteItem solidBlock = new SpriteItem(x, y, false);
                            Blocks.Add(solidBlock);
                            Grid.SetRow(solidBlock, y);
                            Grid.SetColumn(solidBlock, x);
                            main.Children.Add(solidBlock);
                            break;
                        case 2:
                            SpriteItem pushBlock = new SpriteItem(x, y, true);
                            Blocks.Add(pushBlock);
                            Grid.SetRow(pushBlock, y);
                            Grid.SetColumn(pushBlock, x);
                            main.Children.Add(pushBlock);
                            break;
                        case 3:
                            mouse.sprite.X = x;
                            mouse.sprite.Y = y;
                            Grid.SetRow(mouse, y);
                            Grid.SetColumn(mouse, x);
                            main.Children.Add(mouse);
                            break;
                        case 4:
                            SpriteItem cat = new SpriteItem(2, 2, @"./Images/borderblock.png", true, true, mouse.sprite);
                            cat.sprite.Ai.engine = engine;
                            Grid.SetRow(cat, cat.sprite.Y);
                            Grid.SetColumn(cat, cat.sprite.X);
                            main.Children.Add(cat);
                            Baddies.Add(cat);
                            break;

                    }
                }
            }

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

            engine.Sprites = Blocks.Select(x => x.sprite).ToList();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach (SpriteItem baddie in Baddies)
            {
                Direction directionToMoveBaddie = baddie.sprite.Ai.Move(baddie.sprite.X, baddie.sprite.Y);
                switch (directionToMoveBaddie)
                {
                    case Direction.left:
                        int x = baddie.sprite.X - 1;
                        Grid.SetColumn(baddie, x);
                        baddie.sprite.X = x;
                        break;
                    case Direction.right:
                        x = baddie.sprite.X + 1;
                        Grid.SetColumn(baddie, x);
                        baddie.sprite.X = x;
                        break;
                    case Direction.up:
                        int y = baddie.sprite.Y - 1;
                        Grid.SetRow(baddie, y);
                        baddie.sprite.Y = y;
                        break;
                    case Direction.down:
                        y = baddie.sprite.Y + 1;
                        Grid.SetRow(baddie, y);
                        baddie.sprite.Y = y;
                        break;
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                if (MoveSprites(Direction.left))
                {
                    int x = mouse.sprite.X - 1;
                    Grid.SetColumn(mouse, x);
                    mouse.sprite.X = x;
                }
            }
            if (e.Key == Key.Right)
            {
                if (MoveSprites(Direction.right))
                {
                    int x = mouse.sprite.X + 1;
                    Grid.SetColumn(mouse, x);
                    mouse.sprite.X = x;
                }
            }
            if (e.Key == Key.Up)
            {
                if (MoveSprites(Direction.up))
                {
                    int y = mouse.sprite.Y - 1;
                    Grid.SetRow(mouse, y);
                    mouse.sprite.Y = y;
                }
            }
            if (e.Key == Key.Down)
            {
                if (MoveSprites(Direction.down))
                {
                    int y = mouse.sprite.Y + 1;
                    Grid.SetRow(mouse, y);
                    mouse.sprite.Y = y;
                }
            }
        }

        private bool MoveSprites(Direction direction)
        {
            bool canMove = false;
            if (engine.CollisionDetection(mouse.sprite.X, mouse.sprite.Y, direction))
            {
                List<Sprite> Sprites = engine.GetAllSpritesInPath(mouse.sprite.X, mouse.sprite.Y, direction);

                switch (direction)
                {
                    case Direction.down:
                        if (engine.CanMoveSprites(Sprites, Direction.down))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.up:
                        if (engine.CanMoveSprites(Sprites, Direction.up))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.left:
                        if (engine.CanMoveSprites(Sprites, Direction.left))
                        {
                            canMove = true;
                        }
                        break;
                    case Direction.right:
                        if (engine.CanMoveSprites(Sprites, Direction.right))
                        {
                            canMove = true;
                        }
                        break;
                }

                if (canMove)
                {
                    foreach (SpriteItem Block in GetSpriteItemsFromSprites(Sprites))
                    {
                        int _x = 0;
                        int _y = 0;
                        switch (direction)
                        {
                            case Direction.down:
                                _y = Block.sprite.Y + 1;
                                Grid.SetRow(Block, _y);
                                Block.sprite.Y = _y;
                                break;
                            case Direction.up:
                                _y = Block.sprite.Y - 1;
                                Grid.SetRow(Block, _y);
                                Block.sprite.Y = _y;
                                break;
                            case Direction.left:
                                _x = Block.sprite.X - 1;
                                Grid.SetColumn(Block, _x);
                                Block.sprite.X = _x;
                                break;
                            case Direction.right:
                                _x = Block.sprite.X + 1;
                                Grid.SetColumn(Block, _x);
                                Block.sprite.X = _x;
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

        private List<SpriteItem> GetSpriteItemsFromSprites(List<Sprite> sprites)
        {
            List<SpriteItem> spriteItems = new List<SpriteItem>();
            foreach (SpriteItem Block in Blocks)
            {
                Sprite sprite = sprites.Where(x => x.X == Block.sprite.X && Block.sprite.Y == x.Y).FirstOrDefault();
                if (sprite != null)
                {
                    spriteItems.Add(Block);
                }
            }
            return spriteItems;
        }

    }
}
