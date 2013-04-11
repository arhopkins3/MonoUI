using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoUI
{
    public class DebugRect
    {
        private Texture2D pixel;
        public Color Colour;
        public bool Visible = false;
        public MUIControl Parent;
        public DebugRect(Color col)
        {
            Colour = col;
            pixel = new Texture2D(DeviceManager.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        public void Draw(SpriteBatch sb)
        {
            if (!Visible) return;

            sb.Draw(pixel, Parent.Bounds, Colour);
        }
    }
}
