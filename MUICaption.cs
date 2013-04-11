using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    public class MUICaption : MUIControl
    {
        public string Caption;
        public Color ForegroundColour;
        public Color ShadowColour;
        public bool Shadow = true;
        public bool Enabled = true;

        private FontRenderer fontRenderer;

        public MUICaption(string caption, FontRenderer fr, int x, int y, int w = 100, int h = 100) : base(x, y, w, h)
        {
            Caption = caption; // Set the caption property
            ForegroundColour = Color.White;
            ShadowColour = Color.Black;
            fontRenderer = fr;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Shadow)
            {
                fontRenderer.DrawText(sb, (int)Position.X + 2, (int)Position.Y + 2, Caption, ShadowColour);
            }

            fontRenderer.DrawText(sb, (int) Position.X, (int) Position.Y, Caption, ForegroundColour);
            base.Draw(sb);
        }
    }
}
