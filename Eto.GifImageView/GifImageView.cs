
namespace Eto.GifImageView
{
	using Eto.Forms;
	public class GifImageView : ImageView
	{
		GifImage _image;
		public GifImageView()
		{
		}
		public new GifImage Image
		{
			get => _image;

			set
			{
				if (_image != null)
					_image.Dispose();

				_image = value;

				if (_image != null)
					_image.OnFrameChanged += (o, e) => base.Image = _image.CurrentBitmap;
			}
		}
	}
}
