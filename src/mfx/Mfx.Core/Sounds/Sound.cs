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

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Mfx.Core.Sounds;

public sealed class Sound : Component, IDisposable
{
    #region Private Fields

    private readonly float _initialVolume;

    private readonly SoundEffect _soundEffect;

    private SoundEffectInstance? _soundEffectInstance;

    #endregion Private Fields

    #region Public Constructors

    public Sound(SoundEffect soundEffect, float volume = 1.0f)
    {
        _soundEffect = soundEffect;
        Volume = volume;
        _initialVolume = volume;
    }

    #endregion Public Constructors

    #region Public Properties

    public SoundState State => _soundEffectInstance is not null && !_soundEffectInstance.IsDisposed
        ? _soundEffectInstance.State
        : SoundState.Stopped;

    public float Volume
    {
        get => _soundEffectInstance?.Volume ?? default;
        set
        {
            if (_soundEffectInstance is not null && !_soundEffectInstance.IsDisposed)
            {
                _soundEffectInstance.Volume = value;
            }
        }
    }

    #endregion Public Properties

    #region Public Methods

    public void Dispose()
    {
        Stop();
    }

    public void Play()
    {
        Stop();
        _soundEffectInstance = _soundEffect.CreateInstance();
        _soundEffectInstance.Volume = _initialVolume;
        _soundEffectInstance.Play();
    }

    public void Stop()
    {
        if (_soundEffectInstance is null ||
            _soundEffectInstance.IsDisposed)
        {
            return;
        }

        try
        {
            _soundEffectInstance.Stop(true);
            _soundEffectInstance.Dispose();
        }
        catch
        {
        }
    }

    public override void Update(GameTime gameTime)
    {
    }

    #endregion Public Methods
}