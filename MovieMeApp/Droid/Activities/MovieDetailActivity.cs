using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V4.App;
using MovieMeApp.ViewModels;
using Square.Picasso;

using Uri = Android.Net.Uri;
using Android.Support.V7.Widget;
using MovieMeApp.Droid.Fragments;
using MovieMeApp.Droid.Fragments.Adapters;

namespace MovieMeApp.Droid.Activities
{
	[Activity(Label = "Movie Details", ParentActivity = typeof(MainActivity))]
	[MetaData("android.support.PARENT_ACTIVITY", Value = ".MainActivity")]
	public class MovieDetailActivity : BaseActivity
	{
		/// <summary>
		/// Specify the layout to inflace
		/// </summary>
		protected override int LayoutResource => Resource.Layout.activity_movie_detail;

		MovieDetailViewModel viewModel;
		Spinner spinner;

		protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var data = Intent.GetStringExtra("movie");

			var movie = Newtonsoft.Json.JsonConvert.DeserializeObject<MovieModel>(data);
			viewModel = new MovieDetailViewModel();
			viewModel.Movie = movie;

			await viewModel.GetSimilarMovies(movie.Id);

			FindViewById<TextView>(Resource.Id.movie_overview).Text = movie.Overview;
			FindViewById<TextView>(Resource.Id.movie_title).Text = movie.Title;
			FindViewById<TextView>(Resource.Id.release_date).Text = "Release date: 01/05/2001"; //TODO: format this date!
			FindViewById<TextView>(Resource.Id.votes_amount).Text = Convert.ToString(movie.VoteCount);


			ImageView coverImageView = FindViewById<ImageView>(Resource.Id.movie_cover);
			var url = await viewModel.GetMovieCover(movie.Id);
			try
            {
				Uri uri = Uri.Parse(url);
				Picasso.With(this).Load(uri).Fit().Into(coverImageView);
			}
			catch (Exception ex)
            {
				Console.WriteLine("Error getting the movie cover image: ", ex.Message);

			}

			//// Create a new fragment and a transaction.
			Android.App.FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
			MovieDetailAdapter adapter = new MovieDetailAdapter(this, viewModel);

			MovieCategoryFragment<MovieDetailViewModel> aCategoryFragment = new MovieCategoryFragment<MovieDetailViewModel>(viewModel, adapter);

			////// The fragment will have the ID of Resource.Id.fragment_container.
			fragmentTx.Add(Resource.Id.similar_movies, aCategoryFragment);
			////// Commit the transaction.
			fragmentTx.Commit();

		}


		protected override void OnStart()
		{
			base.OnStart();
			viewModel.PropertyChanged += ViewModel_PropertyChanged;

		}


		protected override void OnStop()
		{
			base.OnStop();
			viewModel.PropertyChanged -= ViewModel_PropertyChanged;
		}


		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{

		}


	}
}
