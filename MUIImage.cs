using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    public class MUIImage : MUIControl
    {
        public bool ScaleToFit = true;
        public float Scale = 1.0f;
        private Texture2D image;
        private bool enabled = true;
        public Color TintColour = Color.White;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value)
                {
                    // Setting to true
                    enabled = true;
                }
                else
                {
                    // Setting to false
                    enabled = false;
                }
            }
        }
        public Texture2D Image
        {
            get { return image; }
            set
            {

                if (value != null)
                {
                    if (ScaleToFit)
                    {
                        // Scale to fit so adjust w/h according to texture
                        Width = value.Width;
                        Height = value.Height;
                        image = value;    
                    }
                    else
                    {
                        // Not scaled to fit, so scale by scale value
                        Width = (int) (value.Width*Scale);
                        Height = (int) (value.Height*Scale);
                        image = value;

                    }
                }
                else
                {
                    // Texture passed in is null so use defaults
                    Width = 50;
                    Height = 50;
                    image = null;
                }
            }
        }

        public MUIImage(Texture2D texture, int x, int y, int w = 100, int h = 100) : base(x, y, w, h)
        {
            Scale = 1.0f;
            ScaleToFit = false; 
            Image = null;
        }

        public MUIImage(Texture2D texture, int x, int y) : base(x, y)
        {
            Scale = 1.0f;
            ScaleToFit = true;
            Image = texture;
        }
        public MUIImage(Texture2D texture, int x, int y, float scale)
            : base(x, y)
        {
            Scale = scale;
            ScaleToFit = false;
            Image = texture;
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Image != null)
            {
                Color currentColour = Color.Red;
                currentColour = enabled ? TintColour : new Color(0.1f, 0.1f, 0.1f); // Set colour to White if enabled, nearly black if disabled

                if (ScaleToFit) sb.Draw(Image, Position, currentColour);
                else sb.Draw(Image, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), currentColour);
            }
            base.Draw(sb);
        }
    }
}
