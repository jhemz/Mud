﻿using Foundation;
using MudEngine;
using System;
using UIKit;


namespace RodentsRevengeiOS
{
    public partial class ViewController : UIViewController
    {
        IsometricTileEngine engine;

        public ViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}