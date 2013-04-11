using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    public static class TexturePixel
    {
        private static readonly Texture2D rectangle;

        static TexturePixel()
        {
            rectangle = new Texture2D(DeviceManager.GraphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });
        }

        public static Texture2D Texture
        {
            get
            {
                return rectangle;
            }
        }
    }
}
