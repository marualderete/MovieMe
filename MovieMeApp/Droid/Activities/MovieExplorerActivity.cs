using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Activities
{
	[Activity(Label = "@string/movie_explorer",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MovieExplorerActivity : BaseActivity
	{
		/// <summary>
		/// Specify the layout to inflace
		/// </summary>
		protected override int LayoutResource => Resource.Layout.activity_movie_explorer;

		LinearLayout gallery1_layout, gallery2_layout, gallery3_layout;

		MovieExplorerViewModel viewModel;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			//Layout gets inflated here
			base.OnCreate(savedInstanceState);

			viewModel = new MovieExplorerViewModel();
			await viewModel.LoadModels(AppConfig.TopRated);
			await viewModel.LoadModels(AppConfig.Popular);

			gallery1_layout = FindViewById<LinearLayout>(Resource.Id.gallery1_horizontal_scroll_linear);
			gallery2_layout = FindViewById<LinearLayout>(Resource.Id.gallery2_horizontal_scroll_linear);
			//gallery3_layout = FindViewById<LinearLayout>(Resource.Id.gallery3_horizontal_scroll_linear);

			//foreach (MovieModel movie in viewModel.TopRatedModels)
			//{ 
			//	ImageView image = new ImageView(
			//}
			//Turn off back arrows
			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar.SetHomeButtonEnabled(false);

		}
	}
}
