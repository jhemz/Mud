using System;
using System.Collections.Generic;
using System.Text;

namespace MudEngine
{
    public class UpdateX_EventArgs : EventArgs
    {
        public UpdateX_EventArgs(Sprite sprite, int change)
        {
            Sprite = sprite;
            Change = change;
        }
        public Sprite Sprite { get; private set; }
        public int Change { get; private set; }
    }
    public class UpdateY_EventArgs : EventArgs
    {
        public UpdateY_EventArgs(Sprite sprite, int change)
        {
            Sprite = sprite;
            Change = change;
        }

        public Sprite Sprite { get; private set; }
        public int Change { get; private set; }
    }

    public class AddNewSprite_EventArgs : EventArgs
    {
        public AddNewSprite_EventArgs(Sprite sprite)
        {
            Sprite = sprite;
        }

        public Sprite Sprite { get; private set; }
    }

    public class RemoveSprite_EventArgs : EventArgs
    {
        public RemoveSprite_EventArgs(Sprite sprite)
        {
            Sprite = sprite;
        }

        public Sprite Sprite { get; private set; }
    }


}
