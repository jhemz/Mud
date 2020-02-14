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
        bool gameOver;


        public MainWindow()
        {
            InitializeComponent();

            NewGame(1);
        }

        private void NewGame(int level)
        {
            const int width = 23;
            const int height = 23;
            gameOver = false;
            lblGameOver.Visibility = Visibility.Hidden;
            //SETUP MAP*****************

            //load levels
            Levels levels = new Levels();
            //create map layer
            Layer layer = new Layer();
            //assign data
            layer.Array = levels.LevelMaps[level - 1];
            //create level model and give it the game engine
            Level _level = new Level(new IsometricTileEngine(width, height));
            //create new game map
            _level.Map = new Map();
            //add the layer to the game map
            _level.Map.Layers.Add(layer);
            //store a copy of the game engine locally for local use
            engine = _level.Engine;

            mouse = new SpriteItem(0, 0, @"./Images/mouse.png", 3);
            engine.characterSprite = mouse.sprite;

            engine.UpdateX_Event += UpdateX_EventHandler;
            engine.UpdateY_Event += UpdateY_EventHandler;
            engine.AddNewSprite_Event += AddNewSprite_EventHandler;
            engine.RemoveSprite_Event += RemoveSprite_EventHandler;
            engine.LoseLife_Event += LoseLife_EventHandler;
            engine.ChangeSprite_Event += ChangeSprite_EventHandler;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    int value = layer.Array[y, x];
                    switch (value)
                    {
                        case 1:
                            AddNewSpriteItem(new SpriteItem(x, y, false));
                            break;
                        case 2:
                            AddNewSpriteItem(new SpriteItem(x, y, true));
                            break;
                        case 3:
                            mouse.sprite.X = x;
                            mouse.sprite.Y = y;
                            AddNewSpriteItem(mouse, false);
                            break;
                        case 4:
                            SpriteItem cat = new SpriteItem(x, y, @"./Images/cat.png", true, true, mouse.sprite);
                            cat.sprite.Ai.engine = engine;
                            AddNewSpriteItem(cat);
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

        #region Event Handlers
        private void ChangeSprite_EventHandler(object sender, ChangeSprite_EventArgs e)
        {
            SpriteItem spriteItem = GetSpriteItemFromSprite(e.Sprite);
            spriteItem.image.Source = new BitmapImage(new Uri(@"./Images/sleepingCat.png", UriKind.Relative));
        }

        private void LoseLife_EventHandler(object sender, EventArgs e)
        {
            mouse.sprite.Lives--;

            Tuple<int, int> newLocation = engine.GetRandomFreeLocation();
            UpdateLocation(mouse, newLocation);
            UpdateLivesUI();
            if (mouse.sprite.Lives == 0)
            {
                //game over
                lblGameOver.Visibility = Visibility.Visible;
                dispatcherTimer.Stop();
                main.Children.Remove(mouse);
                gameOver = true;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            engine.MoveCats();
        }

        private void RemoveSprite_EventHandler(object sender, RemoveSprite_EventArgs e)
        {
            SpriteItem spriteItem = GetSpriteItemFromSprite(e.Sprite);
            main.Children.Remove(spriteItem);
        }

        private void AddNewSprite_EventHandler(object sender, AddNewSprite_EventArgs e)
        {
            AddNewSpriteItem(new SpriteItem(e.Sprite.X, e.Sprite.Y, @"./Images/cheese.png", false, false));
        }

        private void UpdateY_EventHandler(object sender, UpdateY_EventArgs e)
        {
            SpriteItem spriteItem = GetSpriteItemFromSprite(e.Sprite);
            UpdateY(spriteItem, e.Change);
        }

        private void UpdateX_EventHandler(object sender, UpdateX_EventArgs e)
        {
            SpriteItem spriteItem = GetSpriteItemFromSprite(e.Sprite);
            UpdateX(spriteItem, e.Change);
        }

        #endregion

        #region Update UI

        private void UpdateLivesUI()
        {
            switch (mouse.sprite.Lives)
            {
                case 3:
                    Life1.Visibility = Visibility.Visible;
                    Life2.Visibility = Visibility.Visible;
                    Life3.Visibility = Visibility.Visible;
                    break;
                case 2:
                    Life1.Visibility = Visibility.Visible;
                    Life2.Visibility = Visibility.Visible;
                    Life3.Visibility = Visibility.Hidden;
                    break;
                case 1:
                    Life1.Visibility = Visibility.Visible;
                    Life2.Visibility = Visibility.Hidden;
                    Life3.Visibility = Visibility.Hidden;
                    break;
                case 0:
                    Life1.Visibility = Visibility.Hidden;
                    Life2.Visibility = Visibility.Hidden;
                    Life3.Visibility = Visibility.Hidden;
                    break;

            }
        }

        private void UpdateLocation(SpriteItem item, Tuple<int, int> location)
        {
            Grid.SetRow(item, location.Item2);
            item.sprite.Y = location.Item2;
            Grid.SetColumn(item, location.Item1);
            item.sprite.X = location.Item1;
        }
        private void UpdateY(SpriteItem item, int change)
        {
            Grid.SetRow(item, item.sprite.Y + change);
            item.sprite.Y = item.sprite.Y + change;
        }
        private void UpdateX(SpriteItem item, int change)
        {
            Grid.SetColumn(item, item.sprite.X + change);
            item.sprite.X = item.sprite.X + change;
        }

        private void AddNewSpriteItem(SpriteItem item, bool addToBlocks = true)
        {
            if (addToBlocks)
            {
                Blocks.Add(item);
            }
            Grid.SetRow(item, item.sprite.Y);
            Grid.SetColumn(item, item.sprite.X);
            main.Children.Add(item);
        }

        #endregion

        #region User Event Handlers

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (gameOver == false)
            {
                if (e.Key == Key.Left)
                {
                    if (engine.MoveSprites(Direction.left))
                    {
                        UpdateX(mouse, -1);
                    }
                }
                if (e.Key == Key.Right)
                {
                    if (engine.MoveSprites(Direction.right))
                    {
                        UpdateX(mouse, 1);
                    }
                }
                if (e.Key == Key.Up)
                {
                    if (engine.MoveSprites(Direction.up))
                    {
                        UpdateY(mouse, -1);
                    }
                }
                if (e.Key == Key.Down)
                {
                    if (engine.MoveSprites(Direction.down))
                    {
                        UpdateY(mouse, 1);
                    }
                }
            }
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            NewGame(1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var myWindow = Window.GetWindow(this);
                myWindow.DragMove();
            }
        }

        #endregion

        #region UI Helpers

        private SpriteItem GetSpriteItemFromSprite(Sprite sprite)
        {
            List<SpriteItem> source = Blocks;

            SpriteItem spriteItem = source.Where(x => sprite.X == x.sprite.X && x.sprite.Y == sprite.Y).FirstOrDefault();

            return spriteItem;
        }

        #endregion




    }
}
