using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Utility
{
    //TODO: Add more primitive types
    //TODO: Split away into own library/project? Seems bad nestled in here

    /// <summary>
    /// A renderer to help draw primitive types with a unified api
    /// </summary>
    public class PrimitiveRenderer
    {
        /// <summary>
        /// 1px texture used to fill XNA default primitives with
        /// </summary>
        static Texture2D fillTex;

        /// <summary>
        /// Load the content needed (must be called before drawing)
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadContent(ContentManager Content)
        {
            fillTex = Content.Load<Texture2D>("pixel");
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw primitive on</param>
        /// <param name="rect">Rectangle coordinates to draw</param>
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect)
        {
            DrawRectangle(spriteBatch, rect, Color.White);
        }

        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="spriteBatch">Spritebatch to draw primitive on</param>
        /// <param name="rect">Rectangle coordinates to draw</param>
        /// <param name="color">Color to draw primitive</param>
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            spriteBatch.Draw(fillTex, rect, color);
        }
    }
}
