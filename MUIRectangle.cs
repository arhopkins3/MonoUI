using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    public static class MUIRectangle
    {
        public static void Draw(SpriteBatch sb, Rectangle rect, Color colour)
        {
            sb.Draw(TexturePixel.Texture, rect, colour);
        }
    }
}
