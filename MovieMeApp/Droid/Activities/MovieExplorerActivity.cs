using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MovieMeApp.Droid.Fragments;
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

		MovieExplorerViewModel viewModel;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			//Layout gets inflated here
			base.OnCreate(savedInstanceState);

			viewModel = new MovieExplorerViewModel();
			await viewModel.LoadModels(AppConfig.TopRated);
			await viewModel.LoadModels(AppConfig.Popular);

			// Create a new fragment and a transaction.
			FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
			MovieCategoryFragment aCategoryFragment = new MovieCategoryFragment();
			aCategoryFragment.ViewModel = viewModel;

			// The fragment will have the ID of Resource.Id.fragment_container.
			fragmentTx.Add(Resource.Id.movie_explorer_container, aCategoryFragment);

			// Commit the transaction.
			fragmentTx.Commit();

			//Turn off back arrows
			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar.SetHomeButtonEnabled(false);

		}
	}
}
