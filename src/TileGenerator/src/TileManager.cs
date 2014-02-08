using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * TODO:
 * Split TileManager away from being a dependency?
 */

namespace TileGenerator
{
    /// <summary>
    /// Stores and manages the tiles
    /// </summary>
    public class TileManager
    {
        int gridWidth, gridHeight, gridTileSize;

        /// <summary>
        /// Get the width of the grid (tiles)
        /// </summary>
        public int Width
        {
            get
            {
                return gridWidth;
            }
            private set
            {
                gridWidth = value;
            }
        }

        /// <summary>
        /// Get the height of the grid (tiles)
        /// </summary>
        public int Height
        {
            get
            {
                return gridHeight;
            }
            private set
            {
                gridHeight = value;
            }
        }

        /// <summary>
        /// Get the size of each tile (pixels)
        /// </summary>
        public int TileSize
        {
            get
            {
                return gridTileSize;
            }
            private set
            {
                gridTileSize = value;
            }
        }

        //TODO: Add tile types? Is this neccesary? For now just bool
        bool[,] tileData;

        /// <summary>
        /// Create a new tile manager
        /// </summary>
        /// <param name="width">Width of the grid (tiles)</param>
        /// <param name="height">Height of the grid (tiles)</param>
        /// <param name="tileSize">Size of each tile (pixels)</param>
        public TileManager(int width, int height, int tileSize)
        {
            this.Width = width;
            this.Height = height;
            this.TileSize = tileSize;

            //Generate a new array to store the tiles
            this.tileData = new bool[Width, Height];
        }

        /// <summary>
        /// Create a new tile Manager
        /// </summary>
        /// <param name="width">Width of the grid (tiles, rounds up)</param>
        /// <param name="height">Height of the grid (tiles, rounds up)</param>
        /// <param name="tileSize">Size of each tile (pixels)</param>
        public TileManager(float width, float height, int tileSize)
            : this((int)Math.Ceiling(width), (int)Math.Ceiling(height), tileSize)
        {
        }

        /// <summary>
        /// Check if grid cell is in bounds
        /// </summary>
        /// <param name="x">X position of cell</param>
        /// <param name="y">Y position of cell</param>
        /// <returns>True if cell in bounds of grid</returns>
        bool IsInBounds(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < gridWidth && y < gridHeight);
        }

        /// <summary>
        /// Add a tile to a cell
        /// </summary>
        /// <param name="x">X position of cell</param>
        /// <param name="y">Y position of cell</param>
        public void AddTile(int x, int y)
        {
            //Return if not in bounds
            if (!IsInBounds(x, y)) return;

            tileData[x, y] = true;
        }

        /// <summary>
        /// Remove a tile from a cell
        /// </summary>
        /// <param name="x">X position of cell</param>
        /// <param name="y">Y position of cell</param>
        public void RemoveTile(int x, int y)
        {
            //Return if not in bounds
            if (!IsInBounds(x, y)) return;

            tileData[x, y] = false;
        }

        /// <summary>
        /// Get the state of a cell
        /// </summary>
        /// <param name="x">X position of cell</param>
        /// <param name="y">Y position of cell</param>
        /// <returns>True if cell contains a tile</returns>
        public bool GetTile(int x, int y)
        {
            //Should except??
            //Return false if not in bounds
            if (!IsInBounds(x, y)) return false;

            return tileData[x, y];
        }

        /// <summary>
        /// Get a 2D array of all tiles
        /// </summary>
        /// <returns>2D array of true/false indicating cell status (filled/unfilled)</returns>
        public bool[,] GetAllTiles()
        {
            return tileData;
        }

        /// <summary>
        /// Get a rectangle encompassing a tile
        /// </summary>
        /// <param name="x">X position of cell</param>
        /// <param name="y">Y position of cell</param>
        /// <returns>A rectangle representing the tile</returns>
        public Rectangle GetTileRectangle(int x, int y)
        {
            //Should except?? Need advice
            //Return empty rectangle if not in bounds
            if (!IsInBounds(x, y)) return new Rectangle();

            //Build a new rectangle from tile size
            return new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
        }
    }
}
