using System;
using System.Collections.ObjectModel;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using MovieMeApp.Droid.Activities;
using MovieMeApp.Droid.Fragments.Holders;
using MovieMeApp.Services;
using Square.Picasso;
using Uri = Android.Net.Uri;

namespace MovieMeApp.Droid.Fragments.Adapters
{
	/// <summary>
	/// Movie adapter.
	/// </summary>
	class MovieAdapter : BaseRecycleViewAdapter
	{
		#region private properties

		Context _context;
		ObservableCollection<MovieModel> _movies;
		CloudDataMovieStore _dataMovieStore;

		#endregion

		#region constructor
		public MovieAdapter(ObservableCollection<MovieModel> movies, CloudDataMovieStore movieStore)
		{
			_movies = movies;
			_dataMovieStore = movieStore;
		}
		#endregion

		#region override methods for MovieAdapter
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			//Setup your layout here
			View itemView = null;

			_context = parent.Context;

			var id = Resource.Layout.movie_item;
			itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
			var vh = new MovieViewHolder(itemView, Image_ItemClick, OnLongClick);
			return vh;
		}

		public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var movieHolder = holder as MovieViewHolder;
			var movie = _movies[position];

			var url = await _dataMovieStore.GetMovieCoverURL(movie.Id);

			try
			{
				Uri uri = Uri.Parse(url);
				Picasso.With(_context).Load(uri).Fit().Into(movieHolder.MovieCover);

			}
			catch (Exception ex)
			{
				Console.WriteLine("Error getting the movie cover image: ", ex.Message);

			}

		}

		public override int ItemCount => _movies.Count;

		#endregion

		#region public methods

		void Image_ItemClick(RecyclerClickEventArgs e)
		{
			var item = _movies[e.Position];

			var intent = new Intent(_context.ApplicationContext, typeof(MovieDetailActivity));

			intent.PutExtra("movie", Newtonsoft.Json.JsonConvert.SerializeObject(item));
			intent.AddFlags(ActivityFlags.NewTask);
			_context.ApplicationContext.StartActivity(intent);
		}
		#endregion
	}
}
