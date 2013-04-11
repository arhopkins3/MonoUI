using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MonoUI
{
    public static class TextureLoader
    {

        public static GraphicsDevice graphicsDevice;

        public static Texture2D LoadTexture(string filename)
        {
            if (graphicsDevice == null)
            {
                graphicsDevice = DeviceManager.GraphicsDevice;
            }

            string contentFile = "Assets";
            contentFile += "/" + filename;

            var fileStream = new FileStream(contentFile, FileMode.Open);
            Texture2D returnTexture = Texture2D.FromStream(graphicsDevice, fileStream);
            fileStream.Close();
            return returnTexture;
        }
    }
}
