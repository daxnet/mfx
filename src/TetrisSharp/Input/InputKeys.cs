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
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace TetrisSharp.Input;

internal sealed class InputKeys
{
    #region Private Fields

    private readonly List<Keys> _allKeyboardKeys;
    private readonly List<string> _pressedKeys = new();
    private readonly List<string> _releasedKeys = new();

    #endregion Private Fields

    #region Public Constructors

    public InputKeys()
    {
        _allKeyboardKeys = Enum.GetNames(typeof(Keys)).Select(n => (Keys)Enum.Parse(typeof(Keys), n)).ToList();
    }

    #endregion Public Constructors

    #region Public Methods

    public string[] GetPressedKeys()
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
                _pressedKeys.Add("gamepad.thumbsticks.left.left");
            }

            if (gpd.ThumbSticks.Left.X > 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.left.right");
            }

            if (gpd.ThumbSticks.Left.Y < 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.left.down");
            }

            if (gpd.ThumbSticks.Left.Y > 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.left.up");
            }

            if (gpd.ThumbSticks.Right.X < 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.right.left");
            }

            if (gpd.ThumbSticks.Right.X > 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.right.right");
            }

            if (gpd.ThumbSticks.Right.Y < 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.right.down");
            }

            if (gpd.ThumbSticks.Right.Y > 0)
            {
                _pressedKeys.Add("gamepad.thumbsticks.right.up");
            }

            if (gpd.DPad.Left == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.dpad.left");
            }

            if (gpd.DPad.Right == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.dpad.right");
            }

            if (gpd.DPad.Up == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.dpad.up");
            }

            if (gpd.DPad.Down == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.dpad.down");
            }

            if (gpd.Buttons.X == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.x");
            }

            if (gpd.Buttons.Y == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.y");
            }

            if (gpd.Buttons.A == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.a");
            }

            if (gpd.Buttons.B == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.b");
            }

            if (gpd.Buttons.Back == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.back");
            }

            if (gpd.Buttons.BigButton == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.bigbutton");
            }

            if (gpd.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.left_shoulder");
            }

            if (gpd.Buttons.LeftStick == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.left_stick");
            }

            if (gpd.Buttons.RightShoulder == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.right_shoulder");
            }

            if (gpd.Buttons.RightStick == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.right_stick");
            }

            if (gpd.Buttons.Start == ButtonState.Pressed)
            {
                _pressedKeys.Add("gamepad.buttons.start");
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
                        _pressedKeys.Add($"joystick.hats[{hatIndex}].left");
                    }

                    if (joystick.Hats[hatIndex].Right == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.hats[{hatIndex}].right");
                    }

                    if (joystick.Hats[hatIndex].Up == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.hats[{hatIndex}].up");
                    }

                    if (joystick.Hats[hatIndex].Down == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.hats[{hatIndex}].down");
                    }
                }

                for (var btnIndex = 0; btnIndex < joystick.Buttons.Length; btnIndex++)
                {
                    if (joystick.Buttons[btnIndex] == ButtonState.Pressed)
                    {
                        _pressedKeys.Add($"joystick.buttons[{btnIndex}]");
                    }
                }
            }
        }

        if (_pressedKeys.Count > 0)
        {
        }

        return _pressedKeys.ToArray();
    }

    public string[] GetReleasedKeys()
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
                _releasedKeys.Add("gamepad.thumbsticks.left.left");
                _releasedKeys.Add("gamepad.thumbsticks.left.right");
            }

            if (gpd.ThumbSticks.Left.Y == 0)
            {
                _releasedKeys.Add("gamepad.thumbsticks.left.up");
                _releasedKeys.Add("gamepad.thumbsticks.left.down");
            }

            if (gpd.ThumbSticks.Right.X == 0)
            {
                _releasedKeys.Add("gamepad.thumbsticks.right.left");
                _releasedKeys.Add("gamepad.thumbsticks.right.right");
            }

            if (gpd.ThumbSticks.Right.Y == 0)
            {
                _releasedKeys.Add("gamepad.thumbsticks.right.up");
                _releasedKeys.Add("gamepad.thumbsticks.right.down");
            }

            if (gpd.DPad.Left == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.dpad.left");
            }

            if (gpd.DPad.Right == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.dpad.right");
            }

            if (gpd.DPad.Up == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.dpad.up");
            }

            if (gpd.DPad.Down == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.dpad.down");
            }

            if (gpd.Buttons.X == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.x");
            }

            if (gpd.Buttons.Y == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.y");
            }

            if (gpd.Buttons.A == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.a");
            }

            if (gpd.Buttons.B == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.b");
            }

            if (gpd.Buttons.Back == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.back");
            }

            if (gpd.Buttons.BigButton == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.bigbutton");
            }

            if (gpd.Buttons.LeftShoulder == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.left_shoulder");
            }

            if (gpd.Buttons.LeftStick == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.left_stick");
            }

            if (gpd.Buttons.RightShoulder == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.right_shoulder");
            }

            if (gpd.Buttons.RightStick == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.right_stick");
            }

            if (gpd.Buttons.Start == ButtonState.Released)
            {
                _releasedKeys.Add("gamepad.buttons.start");
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
                        _releasedKeys.Add($"joystick.hats[{hatIndex}].left");
                    }

                    if (joystick.Hats[hatIndex].Right == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.hats[{hatIndex}].right");
                    }

                    if (joystick.Hats[hatIndex].Up == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.hats[{hatIndex}].up");
                    }

                    if (joystick.Hats[hatIndex].Down == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.hats[{hatIndex}].down");
                    }
                }

                for (var btnIndex = 0; btnIndex < joystick.Buttons.Length; btnIndex++)
                {
                    if (joystick.Buttons[btnIndex] == ButtonState.Released)
                    {
                        _releasedKeys.Add($"joystick.buttons[{btnIndex}]");
                    }
                }
            }
        }

        return _releasedKeys.ToArray();
    }

    public bool IsKeyPressed(string key)
    {
        return GetPressedKeys().Contains(key);
    }

    public bool IsKeyReleased(string key)
    {
        return GetReleasedKeys().Contains(key);
    }

    #endregion Public Methods
}