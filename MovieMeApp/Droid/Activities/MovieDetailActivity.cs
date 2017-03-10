using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using MovieMeApp.ViewModels;
using MovieMeApp.Models;
using Square.Picasso;
using MovieMeApp.Services;

using Uri = Android.Net.Uri;

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
			viewModel = new MovieDetailViewModel(movie);

			FindViewById<TextView>(Resource.Id.movie_overview).Text = movie.Overview;
			FindViewById<TextView>(Resource.Id.movie_title).Text = movie.Title;
			FindViewById<TextView>(Resource.Id.release_date).Text = "01/05/2001"; //TODO: format this date!
			FindViewById<TextView>(Resource.Id.votes_amount).Text = Convert.ToString(movie.VoteCount);


			ImageView coverImageView = FindViewById<ImageView>(Resource.Id.movie_cover);
			//LinearLayout coverImageView = FindViewById<ImageView>(Resource.Id.similar_movies);

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
