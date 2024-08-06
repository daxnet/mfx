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

public sealed class BackgroundMusic(IEnumerable<SoundEffect> musicEffects, float volume = 1.0f, bool looped = true)
    : Component
{
    #region Private Fields

    private readonly SoundEffect[] _musicEffects = musicEffects.ToArray();
    private readonly TimeSpan _soundStatusCheckInterval = TimeSpan.FromSeconds(5);
    private int _currentIndex;
    private TimeSpan _elapsedGameTime;
    private SoundEffectInstance? _musicEffectInstance;
    private bool _stopped = true;

    #endregion Private Fields

    #region Public Properties

    public SoundState State
    {
        get
        {
            if (_musicEffectInstance is { IsDisposed: false })
            {
                return _musicEffectInstance.State;
            }

            return SoundState.Stopped;
        }
    }

    public float Volume
    {
        get => _musicEffectInstance?.Volume ?? 0;
        set
        {
            if (_musicEffectInstance is { IsDisposed: false })
            {
                _musicEffectInstance.Volume = value;
            }
        }
    }

    #endregion Public Properties

    #region Public Methods

    public void Pause()
    {
        if (_musicEffectInstance is { IsDisposed: false })
        {
            _musicEffectInstance.Pause();
        }
    }

    public void Play()
    {
        if (_stopped)
        {
            _currentIndex = 0;
            Play(_currentIndex);
            _stopped = false;
        }
    }

    public void Resume()
    {
        if (_musicEffectInstance is { IsDisposed: false })
        {
            _musicEffectInstance.Resume();
        }
    }

    public void Stop()
    {
        Stop(true);
    }

    public override void Update(GameTime gameTime)
    {
        if (_stopped)
        {
            return;
        }

        _elapsedGameTime += gameTime.ElapsedGameTime;
        if (_elapsedGameTime >= _soundStatusCheckInterval && _musicEffectInstance is { IsDisposed: false })
        {
            if (_musicEffectInstance.State == SoundState.Stopped)
            {
                _currentIndex = (_currentIndex + 1) % _musicEffects.Length;
                if (_currentIndex == 0 && !looped)
                {
                    Stop();
                    return;
                }

                Play(_currentIndex);
            }

            _elapsedGameTime = TimeSpan.Zero;
        }
    }

    #endregion Public Methods

    #region Private Methods

    private void Play(int index)
    {
        Stop(false);
        _musicEffectInstance = _musicEffects[index].CreateInstance();
        _musicEffectInstance.IsLooped = false;
        _musicEffectInstance.Volume = volume;
        _musicEffectInstance.Play();
    }

    private void Stop(bool stopAll)
    {
        if (_musicEffectInstance is { IsDisposed: false })
        {
            _musicEffectInstance.Stop(true);
            _musicEffectInstance.Dispose();
        }

        if (stopAll && !_stopped)
        {
            _stopped = true;
            _currentIndex = 0;
        }
    }

    #endregion Private Methods
}