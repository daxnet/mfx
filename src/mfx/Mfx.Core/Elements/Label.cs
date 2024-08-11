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

using Mfx.Core.Fonts;
using Mfx.Core.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mfx.Core.Elements;

/// <summary>
///     Represents a static label which shows a static text on the specified scene.
/// </summary>
public class Label : VisibleComponent
{
    #region Private Fields

    private readonly IFontAdapter _fontAdapter;
    private readonly Vector2 _textSize;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="font">The <see cref="SpriteFont" /> instance which specifies the font of the text.</param>
    /// <param name="x">The X coordinate of the text position.</param>
    /// <param name="y">The Y coordinate of the text position.</param>
    /// <param name="color">The <see cref="Color" /> to be used to show the text.</param>
    public Label(string text, IScene scene, SpriteFont font, float x, float y, Color color)
        : this(text, scene, font, new RenderingOptions(color, false), x, y)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="font">The <see cref="SpriteFont" /> instance which specifies the font of the text.</param>
    /// <param name="options">The <see cref="RenderingOptions" /> that specifies the options for rendering the static label.</param>
    public Label(string text, IScene scene, SpriteFont font, RenderingOptions options) : base(scene, font.Texture)
    {
        _fontAdapter = new SpriteFontAdapter(font);
        Text = text;
        Collidable = false;
        Options = options;
        _textSize = font.MeasureString(text);
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="font">The <see cref="SpriteFont" /> instance which specifies the font of the text.</param>
    /// <param name="color">The <see cref="Color" /> to be used to show the text.</param>
    /// <remarks>
    ///     As this constructor doesn't accept X and Y coordinates parameters, the static label will be
    ///     placed at the center of the current <see cref="Viewport" />. Therefore, X and Y properties will always be
    ///     zero (0).
    /// </remarks>
    public Label(string text, IScene scene, SpriteFont font, Color color)
        : this(text, scene, font, new RenderingOptions(color, true), 0, 0)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="font">The <see cref="SpriteFont" /> instance which specifies the font of the text.</param>
    /// <param name="options">The <see cref="RenderingOptions" /> that specifies the options for rendering the static label.</param>
    /// <param name="x">The X coordinate of the text position.</param>
    /// <param name="y">The Y coordinate of the text position.</param>
    public Label(string text, IScene scene, SpriteFont font, RenderingOptions options, float x, float y)
        : base(scene, font.Texture, x, y)
    {
        _fontAdapter = new SpriteFontAdapter(font);
        Text = text;
        Collidable = false;
        Options = options;
        _textSize = font.MeasureString(text);
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="fontAdapter">The <see cref="SpriteFont" /> instance.</param>
    /// <param name="x">The X coordinate of the text position.</param>
    /// <param name="y">The Y coordinate of the text position.</param>
    /// <param name="color">The <see cref="Color" /> to be used to show the text.</param>
    public Label(string text, IScene scene, IFontAdapter fontAdapter, float x, float y, Color color)
        : this(text, scene, fontAdapter, new RenderingOptions(color, false), x, y)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="fontAdapter">The <see cref="IFontAdapter" /> instance.</param>
    /// <param name="options">The <see cref="RenderingOptions" /> that specifies the options for rendering the static label.</param>
    public Label(string text, IScene scene, IFontAdapter fontAdapter, RenderingOptions options) : base(scene, null)
    {
        _fontAdapter = fontAdapter;
        Text = text;
        Collidable = false;
        Options = options;
        _textSize = _fontAdapter.MeasureString(text);
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="fontAdapter">The <see cref="IFontAdapter" /> instance.</param>
    /// <param name="color">The <see cref="Color" /> to be used to show the text.</param>
    /// <remarks>
    ///     As this constructor doesn't accept X and Y coordinates parameters, the static label will be
    ///     placed at the center of the current <see cref="Viewport" />. Therefore, X and Y properties will always be
    ///     zero (0).
    /// </remarks>
    public Label(string text, IScene scene, IFontAdapter fontAdapter, Color color)
        : this(text, scene, fontAdapter, new RenderingOptions(color, true), 0, 0)
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <c>Label</c> class.
    /// </summary>
    /// <param name="text">The text to be shown on the specified scene.</param>
    /// <param name="scene">The scene on which the text should be shown.</param>
    /// <param name="fontAdapter">The <see cref="SpriteFont" /> instance.</param>
    /// <param name="options">The <see cref="RenderingOptions" /> that specifies the options for rendering the static label.</param>
    /// <param name="x">The X coordinate of the text position.</param>
    /// <param name="y">The Y coordinate of the text position.</param>
    public Label(string text, IScene scene, IFontAdapter fontAdapter, RenderingOptions options, float x, float y)
        : base(scene, null, x, y)
    {
        _fontAdapter = fontAdapter;
        Text = text;
        Collidable = false;
        Options = options;
        _textSize = fontAdapter.MeasureString(text);
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    ///     Gets the <see cref="RenderingOptions" />.
    /// </summary>
    public RenderingOptions Options { get; }

    /// <summary>
    ///     Gets or sets the text of the static label.
    /// </summary>
    public string Text { get; set; }

    #endregion Public Properties

    #region Protected Properties

    /// <summary>
    ///     Gets the position where the current static label should be drawn.
    /// </summary>
    /// <remarks>
    ///     If <c>CenterScreen</c> property of <see cref="RenderingOptions" /> has been set to true,
    ///     this property will return the calculated X and Y coordinates. Otherwise, the X and Y values
    ///     will be determined by whether the xCoordCallback and yCoordCallback constructor parameters
    ///     were set during the initialization of the current instance. If both callback functions were specified,
    ///     the X and Y value will be calculated with these callback functions, otherwise, user-specified
    ///     coordinates will be used.
    /// </remarks>
    protected Vector2 DrawingPosition
    {
        get
        {
            if (Options.CenterScreen)
            {
                return new Vector2((Scene.Viewport.Width - _textSize.X) / 2,
                    (Scene.Viewport.Height - _textSize.Y) / 2);
            }

            if (Options.CenterHorizontally)
            {
                return new Vector2((Scene.Viewport.Width - _textSize.X) / 2, Y);
            }

            return new Vector2(X, Y);
        }
    }

    #endregion Protected Properties

    #region Protected Methods

    /// <inheritdoc />
    protected override void ExecuteDraw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        //spriteBatch.Begin();
        spriteBatch.DrawString(_fontAdapter, Text, DrawingPosition, Options.Color);
        //spriteBatch.End();
    }

    #endregion Protected Methods

    #region Public Structs

    /// <summary>
    ///     Represents the options used for rendering a static label.
    /// </summary>
    public readonly struct RenderingOptions
    {
        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <c>RenderingOptions</c> struct.
        /// </summary>
        /// <param name="color">The static label color.</param>
        public RenderingOptions(Color color)
        {
            Color = color;
        }

        /// <summary>
        ///     Initializes a new instance of the <c>RenderingOptions</c> struct.
        /// </summary>
        /// <param name="color">The static label color.</param>
        /// <param name="centerScreen">
        ///     A <see cref="bool" /> value which indicates whether the static label
        ///     should be put at the center of the current <see cref="Viewport" />.
        /// </param>
        public RenderingOptions(Color color, bool centerScreen)
        {
            CenterScreen = centerScreen;
            Color = color;
        }

        public RenderingOptions(Color color, bool centerScreen, bool centerHorizontally)
            : this(color, centerScreen)
        {
            CenterHorizontally = centerHorizontally;
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        ///     Gets a <see cref="bool" /> value which indicates whether the static label
        ///     should be put at the center of the current <see cref="Viewport" />.
        /// </summary>
        public bool CenterScreen { get; } = false;

        public bool CenterHorizontally { get; } = false;

        /// <summary>
        ///     Gets the static label color.
        /// </summary>
        public Color Color { get; }

        #endregion Public Properties
    }

    #endregion Public Structs
}