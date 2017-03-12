using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using MovieMeApp.Droid.Fragments;
using MovieMeApp.Droid.Fragments.Adapters;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Activities
{
	[Activity(Label = "@string/movie_explorer",
		ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MovieExplorerActivity : BaseActivity
	{
		#region protected properties
		/// <summary>
		/// Specify the layout to inflace
		/// </summary>
		protected override int LayoutResource => Resource.Layout.activity_movie_explorer;
		#endregion

		#region private properties
		MovieExplorerViewModel viewModel;
		#endregion

		#region override methods
		/// <summary>
		/// Ons the create.
		/// </summary>
		/// <param name="savedInstanceState">Saved instance state.</param>
		protected override async void OnCreate(Bundle savedInstanceState)
		{
			//Layout gets inflated here
            base.OnCreate(savedInstanceState);

			viewModel = new MovieExplorerViewModel();

			await viewModel.LoadModels(AppConfig.TopRated);
			await viewModel.LoadModels(AppConfig.Popular);
            await viewModel.LoadModels (AppConfig.NowPlaying);

			// Create a new fragment and a transaction.
			FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
			MovieCategoryAdapter adapter = new MovieCategoryAdapter(this, viewModel);

			MovieCategoryFragment<MovieExplorerViewModel> aCategoryFragment = new MovieCategoryFragment<MovieExplorerViewModel>(viewModel, adapter, async () => 
			{
				viewModel.Categories.Clear();
				await viewModel.LoadModels(AppConfig.TopRated);
				await viewModel.LoadModels(AppConfig.Popular);
				await viewModel.LoadModels(AppConfig.NowPlaying);
			});

			// The fragment will have the ID of Resource.Id.fragment_container.
			fragmentTx.Add(Resource.Id.movie_explorer_container, aCategoryFragment);

			// Commit the transaction.
			fragmentTx.Commit();

			//Turn off back arrows
			SupportActionBar.SetDisplayHomeAsUpEnabled(false);
			SupportActionBar.SetHomeButtonEnabled(false);

		}
		#endregion

	}
}
