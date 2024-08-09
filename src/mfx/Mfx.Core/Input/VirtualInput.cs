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

public static class VirtualInput
{
    #region Private Fields

    private static readonly List<Keys> _allKeyboardKeys;

    private static readonly List<string> _pressedKeys = new();

    private static readonly List<string> _releasedKeys = new();

    private static readonly Dictionary<string, bool> _keyHeldState = new();

    #endregion Private Fields

    #region Public Constructors

    static VirtualInput()
    {
        _allKeyboardKeys = Enum.GetNames(typeof(Keys)).Select(n => (Keys)Enum.Parse(typeof(Keys), n)).ToList();
    }

    #endregion Public Constructors

    #region Public Methods

    public static string[] GetPressedVirtualKeys()
    {
        _pressedKeys.Clear();
        var kbd = Keyboard.GetState();
        foreach (var key in _allKeyboardKeys.Where(kbd.IsKeyDown))
        {
            _pressedKeys.Add($"key.{Enum.GetName(typeof(Keys), key)}");
        }

        var gpd = GamePad.GetState(0, GamePadDeadZone.Circular);
        if (gpd.IsConnected)
        {
            if (gpd.ThumbSticks.Left.X < 0)
            {
                _pressedKeys.Add("gamepad.1.left");
            }

            if (gpd.ThumbSticks.Left.X > 0)
            {
                _pressedKeys.Add("gamepad.1.right");
            }

            if (gpd.ThumbSticks.Left.Y < 0)
            {
                _pressedKeys.Add("gamepad.1.down");
            }

            if (gpd.ThumbSticks.Left.Y > 0)
            {
                _pressedKeys.Add("gamepad.1.up");
            }

            if (gpd.ThumbSticks.Right.X < 0)
            {
                _pressedKeys.Add("gamepad.2.left");
            }

            if (gpd.ThumbSticks.Right.X > 0)
            {
                _pressedKeys.Add("gamepad.2.right");
            }

            if (gpd.ThumbSticks.Right.Y < 0)
            {
                _pressedKeys.Add("gamepad.2.down");
            }

            if (gpd.ThumbSticks.Right.Y > 0)
            {
                _pressedKeys.Add("gamepad.2.up");
            }

            if (gpd.DPad.Left == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.left");
            }

            if (gpd.DPad.Right == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.right");
            }

            if (gpd.DPad.Up == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.up");
            }

            if (gpd.DPad.Down == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.down");
            }

            if (gpd.Buttons.X == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.x");
            }

            if (gpd.Buttons.Y == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.y");
            }

            if (gpd.Buttons.A == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.a");
            }

            if (gpd.Buttons.B == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.b");
            }

            if (gpd.Buttons.Back == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.back");
            }

            if (gpd.Buttons.BigButton == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.bigbutton");
            }

            if (gpd.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.lshoulder");
            }

            if (gpd.Buttons.LeftStick == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.lstick");
            }

            if (gpd.Buttons.RightShoulder == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.rshoulder");
            }

            if (gpd.Buttons.RightStick == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.rstick");
            }

            if (gpd.Buttons.Start == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.start");
            }
        }

        if (Joystick.IsSupported)
        {
            var joystick = Joystick.GetState(0);
            if (joystick.IsConnected)
            {
                for (var hatIndex = 0; hatIndex < joystick.Hats.Length; hatIndex++)
                {
                    if (joystick.Hats[hatIndex].Left == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.[{hatIndex}].left");
                    }

                    if (joystick.Hats[hatIndex].Right == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.[{hatIndex}].right");
                    }

                    if (joystick.Hats[hatIndex].Up == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.[{hatIndex}].up");
                    }

                    if (joystick.Hats[hatIndex].Down == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.[{hatIndex}].down");
                    }
                }

                for (var btnIndex = 0; btnIndex < joystick.Buttons.Length; btnIndex++)
                {
                    if (joystick.Buttons[btnIndex] == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.btn[{btnIndex}]");
                    }
                }
            }
        }

        if (_pressedKeys.Count > 0)
        {
        }

        return _pressedKeys.ToArray();
    }

    public static string[] GetReleasedVirtualKeys()
    {
        _releasedKeys.Clear();
        var kbd = Keyboard.GetState();
        foreach (var key in _allKeyboardKeys.Where(kbd.IsKeyUp))
        {
            _releasedKeys.Add($"key.{Enum.GetName(typeof(Keys), key)}");
        }

        var gpd = GamePad.GetState(0, GamePadDeadZone.Circular);
        if (gpd.IsConnected)
        {
            if (gpd.ThumbSticks.Left.X == 0)
            {
                _releasedKeys.Add("gamepad.1.left");
                _releasedKeys.Add("gamepad.1.right");
            }

            if (gpd.ThumbSticks.Left.Y == 0)
            {
                _releasedKeys.Add("gamepad.1.up");
                _releasedKeys.Add("gamepad.1.down");
            }

            if (gpd.ThumbSticks.Right.X == 0)
            {
                _releasedKeys.Add("gamepad.2.left");
                _releasedKeys.Add("gamepad.2.right");
            }

            if (gpd.ThumbSticks.Right.Y == 0)
            {
                _releasedKeys.Add("gamepad.2.up");
                _releasedKeys.Add("gamepad.2.down");
            }

            if (gpd.DPad.Left == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.left");
            }

            if (gpd.DPad.Right == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.right");
            }

            if (gpd.DPad.Up == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.up");
            }

            if (gpd.DPad.Down == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.down");
            }

            if (gpd.Buttons.X == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.x");
            }

            if (gpd.Buttons.Y == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.y");
            }

            if (gpd.Buttons.A == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.a");
            }

            if (gpd.Buttons.B == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.b");
            }

            if (gpd.Buttons.Back == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.back");
            }

            if (gpd.Buttons.BigButton == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.bigbutton");
            }

            if (gpd.Buttons.LeftShoulder == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.lshoulder");
            }

            if (gpd.Buttons.LeftStick == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.lstick");
            }

            if (gpd.Buttons.RightShoulder == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.rshoulder");
            }

            if (gpd.Buttons.RightStick == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.rstick");
            }

            if (gpd.Buttons.Start == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.start");
            }
        }

        if (Joystick.IsSupported)
        {
            var joystick = Joystick.GetState(0);
            if (joystick.IsConnected)
            {
                for (var hatIndex = 0; hatIndex < joystick.Hats.Length; hatIndex++)
                {
                    if (joystick.Hats[hatIndex].Left == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.[{hatIndex}].left");
                    }

                    if (joystick.Hats[hatIndex].Right == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.[{hatIndex}].right");
                    }

                    if (joystick.Hats[hatIndex].Up == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.[{hatIndex}].up");
                    }

                    if (joystick.Hats[hatIndex].Down == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.[{hatIndex}].down");
                    }
                }

                for (var btnIndex = 0; btnIndex < joystick.Buttons.Length; btnIndex++)
                {
                    if (joystick.Buttons[btnIndex] == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.btn[{btnIndex}]");
                    }
                }
            }
        }

        return _releasedKeys.ToArray();
    }

    public static bool IsVirtualKeyPressed(string virtualKey) => GetPressedVirtualKeys().Contains(virtualKey);

    public static bool IsVirtualKeyReleased(string virtualKey) => GetReleasedVirtualKeys().Contains(virtualKey);

    public static bool HasPressedOnce(string virtualKey)
    {
        if (IsVirtualKeyPressed(virtualKey))
        {
            if (_keyHeldState.TryGetValue(virtualKey, out var v1) && v1)
            {
                return false;
            }

            _keyHeldState.TryAdd(virtualKey, true);
            return true;
        }

        if (_keyHeldState.TryGetValue(virtualKey, out var v2) && v2)
        {
            _keyHeldState.Remove(virtualKey);
        }

        return false;
    }

    #endregion Public Methods
}