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

/// <summary>
///     Represents the boundary type that a component has hit on.
/// </summary>
[Flags]
public enum Boundary
{
    /// <summary>
    ///     Indicates that the component hasn't reached the boundary.
    /// </summary>
    None = 0,

    /// <summary>
    ///     Indicates that the component has reached the boundary at the top.
    /// </summary>
    Top = 1,

    /// <summary>
    ///     Indicates that the component has reached the boundary at the left side.
    /// </summary>
    Left = 2,

    /// <summary>
    ///     Indicates that the component has reached the boundary at the right side.
    /// </summary>
    Right = 4,

    /// <summary>
    ///     Indicates that the component has readched the boundary at the bottom.
    /// </summary>
    Bottom = 8
}