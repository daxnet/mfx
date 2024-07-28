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

using Microsoft.Xna.Framework.Input;

namespace Mfx.Core.Input;

/// <summary>
///     Represents the extension methods that extend the existing <see cref="KeyboardState" /> object.
/// </summary>
public static class KeyboardStateExtensions
{
    #region Private Fields

    private static readonly Dictionary<Keys, bool> _keyHeldState = new();

    #endregion Private Fields

    #region Public Methods

    /// <summary>
    ///     An extension method which checks if a key has been pressed once.
    /// </summary>
    /// <param name="state">The <see cref="KeyboardState" /> instance to be extended.</param>
    /// <param name="keys">The <see cref="Keys" /> to be checked.</param>
    /// <returns>True if the key has been pressed once, otherwise, false.</returns>
    public static bool HasPressedOnce(this KeyboardState state, Keys keys)
    {
        if (state.IsKeyDown(keys))
        {
            if (_keyHeldState.TryGetValue(keys, out var v1) && v1)
            {
                return false;
            }

            _keyHeldState.TryAdd(keys, true);
            return true;
        }

        if (_keyHeldState.TryGetValue(keys, out var v2) && v2)
        {
            _keyHeldState.Remove(keys);
        }

        return false;
    }

    #endregion Public Methods
}