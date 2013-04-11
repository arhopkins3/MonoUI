using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoUI
{
    public static class DeviceManager
    {
        private static GraphicsDevice graphicsDevice;
        private static GraphicsAdapter graphicsAdapter;
        private static SpriteBatch spriteBatch;

        public static GraphicsDevice GraphicsDevice
        {
            get { if (graphicsDevice != null) return graphicsDevice; else throw new NullReferenceException("Graphics Device has not been specified for the Device Manager"); }
            set { graphicsDevice = value; }
        }
        public static GraphicsAdapter GraphicsAdapter
        {
            get { if (graphicsAdapter != null) return graphicsAdapter; else throw new NullReferenceException("Graphics Adapter has not been specified for the Device Manager"); }
            set { graphicsAdapter = value; }
        }
        public static SpriteBatch SpriteBatch
        {
            get { if (spriteBatch != null) return spriteBatch; else throw new NullReferenceException("Sprite Batch has not been specified for the Device Manager"); }
            set { spriteBatch = value; }
        }
    }
}
