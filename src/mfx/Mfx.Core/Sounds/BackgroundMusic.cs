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
using Microsoft.Xna.Framework.Media;

namespace Mfx.Core.Sounds;

public sealed class BackgroundMusic : Component
{
    #region Private Fields

    private readonly bool _looped;

    private readonly Song[] _songs;

    private readonly TimeSpan _soundStatusCheckInterval = TimeSpan.FromSeconds(5);

    private int _currentIndex;

    private TimeSpan _elapsedGameTime;

    private bool _stopped = true;

    #endregion Private Fields

    #region Public Constructors

    public BackgroundMusic(IEnumerable<Song> songs, float volume = 1.0f, bool looped = true)
    {
        _songs = songs.ToArray();
        Volume = volume;
        _looped = looped;
    }

    #endregion Public Constructors

    #region Public Properties

    public MediaState State => MediaPlayer.State;

    public float Volume
    {
        get => MediaPlayer.Volume;
        set => MediaPlayer.Volume = value;
    }

    #endregion Public Properties

    #region Public Methods

    public void Pause()
    {
        MediaPlayer.Pause();
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
        MediaPlayer.Resume();
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
        if (_elapsedGameTime >= _soundStatusCheckInterval)
        {
            if (MediaPlayer.State == MediaState.Stopped)
            {
                _currentIndex = (_currentIndex + 1) % _songs.Length;
                if (_currentIndex == 0 && !_looped)
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
        var song = _songs[index];
        MediaPlayer.Play(song);
    }

    private void Stop(bool stopAll)
    {
        MediaPlayer.Stop();
        if (stopAll && !_stopped)
        {
            _stopped = true;
            _currentIndex = 0;
        }
    }

    #endregion Private Methods
}