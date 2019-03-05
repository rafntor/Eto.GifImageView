﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AvaloniaGif.Decoding;

namespace AvaloniaGif
{
    public class Animator : IDisposable
    {
        private readonly Stream _sourceStream;
        private readonly Uri _sourceUri;
        private readonly bool _isSourceStreamOwner;
        private readonly GifBackgroundWorker _bgWorker;
        private readonly GifDecoder _decoder;

        public GifDecoder Decoder => _decoder;
        public bool NewFrameAvailable => _decoder._hasNewFrame;

        public Animator(Stream sourceStream, Uri sourceUri, Action CurrentFrameChanged)
        {
            _sourceStream = sourceStream;
            _sourceUri = sourceUri;
            _isSourceStreamOwner = sourceUri != null; // stream opened from URI, should close it

            _decoder = new GifDecoder(sourceStream);
            _bgWorker = new GifBackgroundWorker(_decoder);
            _bgWorker.IterationCount = _decoder.Header.IterationCount;
            _bgWorker.SendCommand(BgWorkerCommand.Play);
            _bgWorker.CurrentFrameChanged = CurrentFrameChanged;
        }

        public async void Play()
        {
            _bgWorker.SendCommand(BgWorkerCommand.Play);
        }

        public void Pause()
        {
            _bgWorker.SendCommand(BgWorkerCommand.Pause);
        }

        public bool IsPaused => (_bgWorker.GetState() == BgWorkerState.Paused);

        public bool IsComplete
        {
            get
            {
                return (_bgWorker.GetState() == BgWorkerState.Complete);
            }
        }

        public int CurrentFrameIndex
        {
            get => _bgWorker.CurrentFrameIndex;
            set => _bgWorker.CurrentFrameIndex = value;
        } 

        ~Animator()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private volatile bool _disposing;
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposing = true;
                _bgWorker?.SendCommand(BgWorkerCommand.Dispose);
                if (_isSourceStreamOwner)
                {
                    try
                    {
                        _decoder?.Dispose();
                        _sourceStream?.Dispose();
                    }
                    catch
                    {
                        /* ignored */
                    }
                }
                _disposed = true;
            }
        }
 

        public override string ToString()
        {
            string s = _sourceUri?.ToString() ?? _sourceStream.ToString();
            return "GIF: " + s;
        }

        public void ShowFirstFrameAsync()
        {
            _decoder.RenderFrame(0);
        }

        public async void Rewind()
        {
            _bgWorker.CurrentFrameIndex = 0;
        }

        public GifRepeatBehavior RepeatBehavior
        {
            get => _bgWorker.IterationCount;
            set => _bgWorker.IterationCount = value;
        }
    }
}
