﻿using MudEngine;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RodentsRevenge
{
    /// <summary>
    /// Interaction logic for Sprite.xaml
    /// </summary>
    public partial class SpriteItem : UserControl
    {

        public Sprite sprite { get; set; }
        public SpriteItem(int x, int y, bool canMove = true)
        {
            InitializeComponent();
            sprite = new Sprite() { X = x, Y = y};
            sprite.CanMove = canMove;
            if (canMove)
            {
                image.Source = new BitmapImage(new Uri(@"./Images/block.png", UriKind.Relative));
            }
            else
            {
                image.Source = new BitmapImage(new Uri(@"./Images/borderblock.png", UriKind.Relative));
            }
        }



        public SpriteItem(int x, int y, string uri, int lives, bool canMove = true)
        {
            InitializeComponent();
            sprite = new Sprite() { X = x, Y = y };
            sprite.CanMove = canMove;
            sprite.Lives = lives;
            image.Source = new BitmapImage(new Uri(uri, UriKind.Relative));

        }

        public SpriteItem(int x, int y, string uri, bool aiControlled, bool canMove = true, Sprite target = null)
        {
            InitializeComponent();
            sprite = new Sprite() { X = x, Y = y };
            sprite.CanMove = canMove;

            image.Source = new BitmapImage(new Uri(uri, UriKind.Relative));

            if (aiControlled)
            {
                sprite.Lives = 1;
                sprite.Ai = new AI() { Target = target, Sprite = sprite };
            }

        }

       
}
}
