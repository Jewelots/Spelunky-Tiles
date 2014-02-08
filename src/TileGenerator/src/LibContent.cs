using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace TileGenerator
{
    public static class LibContent
    {
        static ContentManager Content;

        /// <summary>
        /// Load the content from an embedded resource in the Tile Generator library
        /// </summary>
        /// <param name="game">Current game calling this</param>
        public static void LoadContent(Game game)
        {
            Content = new ResourceContentManager(game.Services, Resources.ResourceManager);

            Utility.PrimitiveRenderer.LoadContent(Content);
        }
    }
}
