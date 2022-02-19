
namespace Eto.GifImageView
{
	using System;
	using System.IO;
	using System.Reflection;
	using Eto.Drawing;
	using Eto.GifImageView.Decoding;
	public class GifImage : IDisposable
	{
		private GifDecoder _gifDecoder;
		private GifBackgroundWorker _bgWorker;
		private Bitmap _targetBitmap;
		private bool _isDisposed;

		public Bitmap CurrentBitmap { get => _targetBitmap; }

		public event EventHandler<EventArgs> OnFrameChanged;

		public static GifImage FromFile(string path)
		{
			using var stream = File.OpenRead(path);

			return FromStream(stream);
		}
		public static GifImage FromResource(string resourceName, Assembly assembly = null)
		{
			if (assembly is null)
				assembly = Assembly.GetCallingAssembly();

			return FromStream(assembly.GetManifestResourceStream(resourceName));
		}
		public static GifImage FromStream(Stream stream) => new GifImage(stream);
		private GifImage(Stream stream)
		{
			if (!stream.CanSeek)
				throw new ArgumentException("The stream is not seekable");

			_gifDecoder = new GifDecoder(stream);
			_bgWorker = new GifBackgroundWorker(_gifDecoder);
			_targetBitmap = new Bitmap(_gifDecoder.Header.Dimensions.Width, _gifDecoder.Header.Dimensions.Height, PixelFormat.Format32bppRgba);
			_bgWorker.CurrentFrameChanged += FrameChanged;
			_bgWorker.SendCommand(BgWorkerCommand.Play);
		}

		private void FrameChanged()
		{
			if (_isDisposed) return;

			using (var lockedBitmap = _targetBitmap?.Lock())
				_gifDecoder?.WriteBackBufToFb(lockedBitmap.Data);

			if (OnFrameChanged != null)
				Eto.Forms.Application.Instance.InvokeAsync(() => OnFrameChanged.Invoke(this,EventArgs.Empty));
		}

		public void Dispose()
		{
			_isDisposed = true;
			_bgWorker?.SendCommand(BgWorkerCommand.Dispose);
			_targetBitmap?.Dispose();
		}
	}
}
