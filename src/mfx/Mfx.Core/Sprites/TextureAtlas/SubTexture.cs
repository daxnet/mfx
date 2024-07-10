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

using System.Xml.Serialization;

namespace Mfx.Core.Sprites.TextureAtlas;

public sealed class SubTexture
{
    #region Public Properties

    [XmlAttribute("frameHeight")] public int FrameHeight { get; set; }

    [XmlAttribute("frameWidth")] public int FrameWidth { get; set; }

    [XmlAttribute("frameX")] public int FrameX { get; set; }

    [XmlAttribute("frameY")] public int FrameY { get; set; }

    [XmlAttribute("height")] public int Height { get; set; }

    [XmlAttribute("name")] public string? Name { get; set; }

    [XmlAttribute("width")] public int Width { get; set; }

    [XmlAttribute("x")] public int X { get; set; }

    [XmlAttribute("y")] public int Y { get; set; }

    #endregion Public Properties

    #region Public Methods

    public override string? ToString() => Name;

    #endregion Public Methods
}