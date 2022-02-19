
namespace Eto.GifImageView.Demo
{
	using Eto.Forms;
	using Eto.Drawing;
	using System.Linq;
	using System.Collections.Generic;

	public partial class MainForm : Form
	{
		public MainForm()
		{
			this.InitializeComponent();

			var assembly = System.Reflection.Assembly.GetExecutingAssembly();

			var gif_file_resources = assembly.GetManifestResourceNames();

			var gif_short_names = new List<string>();
			gif_file_resources.ToList().ForEach(x => gif_short_names.Add(x.Split('.').SkipLast(1).Last()));

			var dropdown = new DropDown() { DataStore = gif_short_names };
			var gifview = new GifImageView();

			dropdown.SelectedValueChanged += (o, e) => gifview.Image = GifImage.FromResource(gif_file_resources[dropdown.SelectedIndex]);

			Content = new DynamicLayout(new DynamicRow(new StackLayout(dropdown), gifview));
		}
	}
}
