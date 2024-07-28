// =============================================================================
//               __
//              / _|
//    _ __ ___ | |___  __
//   | '_ ` _ \|  _\ \/ /
//   | | | | | | |  >  <
//   |_| |_| |_|_| /_/\_\
//
// MIT License
//
// Copyright (c) 2024 Sunny Chen (daxnet)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// =============================================================================

using System;
using System.Collections.Generic;
using Mfx.Core.Scenes;
using Mfx.Core.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TetrisSharp.Blocks;

namespace TetrisSharp;

/// <summary>
/// Represents the game board.
/// </summary>
internal sealed class GameBoard : Sprite
{
    private readonly Texture2D _fixedTileTexture;
    private readonly int _fixedTileSize;

    public GameBoard(IScene scene, Texture2D? texture, Texture2D fixedTileTexture, int numOfTilesX, int numOfTilesY, float x, float y)
        : base(scene, texture, x, y)
    {
        _fixedTileTexture = fixedTileTexture;
        _fixedTileSize = fixedTileTexture.Width;
        Width = numOfTilesX;
        Height = numOfTilesY;
        BoardMatrix = new int[numOfTilesX, numOfTilesY];
    }

    #region Public Properties

    public int[,] BoardMatrix { get; }

    public override int Height { get; }

    /// <summary>
    ///     Gets the index of the lines that are ready to be removed.
    /// </summary>
    public IEnumerable<int> RemovingLines
    {
        get
        {
            for (var y = 0; y < Constants.NumberOfTilesY; y++)
            {
                var numOfFilledTiles = 0;
                for (var x = 0; x < Constants.NumberOfTilesX; x++)
                {
                    if (BoardMatrix[x, y] == 1)
                    {
                        numOfFilledTiles++;
                    }
                }

                if (numOfFilledTiles == Constants.NumberOfTilesX)
                {
                    yield return y;
                }
            }
        }
    }

    public override int Width { get; }

    #endregion Public Properties

    #region Public Methods

    public int CleanupFilledRows(Action<int> beforeRemoveRowCallback)
    {
        var rows = 0;
        for (var y = 0; y < Constants.NumberOfTilesY; y++)
        {
            var isFilledRow = true;
            for (var x = 0; x < Constants.NumberOfTilesX; x++)
            {
                if (BoardMatrix[x, y] == 0)
                {
                    isFilledRow = false;
                    break;
                }
            }

            if (isFilledRow)
            {
                beforeRemoveRowCallback(y);
                for (var my = y - 1; my > 0; my--)
                {
                    for (var mx = 0; mx < Constants.NumberOfTilesX; mx++)
                    {
                        BoardMatrix[mx, my + 1] = BoardMatrix[mx, my];
                    }
                }

                rows++;
            }
        }

        return rows;
    }

    public void Merge(BlockRotation rotation, int x, int y, Action mergeCallback)
    {
        for (var tileY = 0; tileY < rotation.Height; tileY++)
        {
            for (var tileX = 0; tileX < rotation.Width; tileX++)
            {
                if (BoardMatrix[tileX + x, tileY + y] == 0 &&
                    rotation.Matrix is not null &&
                    rotation.Matrix[tileX, tileY] == 1)
                {
                    BoardMatrix[tileX + x, tileY + y] = 1;
                }
            }
        }

        mergeCallback();
    }

    #endregion Public Methods

    protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, new Vector2(X, Y), Color.White);
        for (var by = 0; by < Height; by++)
        {
            for (var bx = 0; bx < Width; bx++)
            {
                if (BoardMatrix[bx, by] == 1)
                {
                    var px = bx * _fixedTileSize + X;
                    var py = by * _fixedTileSize + Y;
                    spriteBatch.Draw(_fixedTileTexture, new Vector2(px, py), Color.White);
                }
            }
        }
    }
}