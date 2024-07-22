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

namespace Mfx.Core;

public sealed class MfxGameSettings
{

    #region Public Fields

    public static readonly MfxGameSettings FullScreen = new()
    {
        IsFullScreen = true
    };

    public static readonly MfxGameSettings NormalScreenFixedSize = new()
    {
        AllowResizing = false,
        Width = DefaultWidth,
        Height = DefaultHeight,
        IsFullScreen = false,
        MouseVisible = false
    };

    public static readonly MfxGameSettings NormalScreenFixedSizeShowMouse = new()
    {
        AllowResizing = false,
        Width = DefaultWidth,
        Height = DefaultHeight,
        IsFullScreen = false,
        MouseVisible = true
    };

    public static readonly MfxGameSettings NormalScreenShowMouse = new()
    {
        AllowResizing = true,
        Width = DefaultWidth,
        Height = DefaultHeight,
        IsFullScreen = false,
        MouseVisible = true
    };

    #endregion Public Fields

    #region Private Fields

    private const int DefaultHeight = 768;
    private const string DefaultTitle = "MfxGame";
    private const int DefaultWidth = 1024;

    #endregion Private Fields

    #region Public Properties

    public bool AllowResizing { get; set; }
    public int Height { get; set; }
    public bool IsFullScreen { get; set; }

    public bool MouseVisible { get; set; }
    public string Title { get; set; } = DefaultTitle;
    public int Width { get; set; }

    #endregion Public Properties

    #region Public Methods

    public static MfxGameSettings DefaultWithTitle(string title, MfxGameSettings? settings = null)
    {
        var result = settings ?? NormalScreenShowMouse;
        result.Title = title;
        return result;
    }

    #endregion Public Methods

}