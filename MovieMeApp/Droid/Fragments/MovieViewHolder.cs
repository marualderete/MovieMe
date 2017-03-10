using System;
using System.Collections.ObjectModel;

using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.App;
using Android.Content;

using MovieMeApp.ViewModels;
using MovieMeApp.Services;
using MovieMeApp.Utils;
using MovieMeApp.Droid.Activities;

using Uri = Android.Net.Uri;
using Square.Picasso;

namespace MovieMeApp.Droid.Fragments
{
	#region HOLDERS

	/// <summary>
	/// Movie view holder.
	/// </summary>
	public class MovieViewHolder : RecyclerView.ViewHolder
	{
		#region public properties
		public ImageView MovieCover { get; set; }
		#endregion

		#region constructor
		public MovieViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
							Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
		{
			MovieCover = itemView.FindViewById<ImageView>(Resource.Id.movie_cover);
			MovieCover.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
		}
		#endregion
	}

	/// <summary>
	/// Movie category view holder.
	/// </summary>
	public class MovieCategoryViewHolder : RecyclerView.ViewHolder
	{
		#region public properties
		public TextView CategoryTitle { get; set; }
		public RecyclerView CategoryRecyclerView { get; set; }
		#endregion

		#region constructor
		public MovieCategoryViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
							Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
		{
			CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.movie_title);
			CategoryRecyclerView = itemView.FindViewById<RecyclerView>(Resource.Id.category_recycler_view);

		}
		#endregion
	}

	#endregion

	#region ADAPTERS
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
                Uri uri = Uri.Parse (url);
                Picasso.With (_context).Load (uri).Fit ().Into (movieHolder.MovieCover);

            } catch (Exception ex) 
            {
                Console.WriteLine ("Error getting the movie cover image: ", ex.Message);

            }

		}

		public override int ItemCount => _movies.Count;

		#endregion

		#region public methods

        void Image_ItemClick (RecyclerClickEventArgs e)
        {
            var item = _movies [e.Position];

			var intent = new Intent (_context.ApplicationContext, typeof (MovieDetailActivity));

            intent.PutExtra ("movie", Newtonsoft.Json.JsonConvert.SerializeObject (item));
			intent.AddFlags(ActivityFlags.NewTask);
			_context.ApplicationContext.StartActivity (intent);
        }
		#endregion
	}

	/// <summary>
	/// Movie category adapter.
	/// </summary>

	public abstract class BaseMovieCategoryAdapter<T> : BaseRecycleViewAdapter where T : BaseViewModel
	{
		#region private properties

		protected T ViewModel;
		protected Activity Activity;

		Context _context;
		StringUtils _utils;
		#endregion

		#region constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MovieMeApp.Droid.Fragments.MovieCategoryAdapter"/> class.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="viewModel">View model.</param>
		public BaseMovieCategoryAdapter(Activity activity, T viewModel)
		{
			ViewModel = viewModel;
            _utils = new StringUtils ();
			Activity = activity;

		}
		#endregion

		#region override methods for MovieCategoryAdapter
		// Create new views (invoked by the layout manager)
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			//Setup your layout here
			View itemView = null;
			var id = Resource.Layout.movie_category;
			_context = parent.Context;
			itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);

			var vh = new MovieCategoryViewHolder(itemView, OnClick, OnLongClick);
			return vh;
		}

        #endregion

		#region public methods

		public string GetCategoryName(string id)
		{
			return _utils.GetCategoryName(id);
		}

		#endregion

	}

	public class MovieDetailAdapter : BaseMovieCategoryAdapter<MovieDetailViewModel>
	{
		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MovieMeApp.Droid.Fragments.MovieCategoryAdapter"/> class.
		/// </summary>
		/// <param name="activity">Activity.</param>
		/// <param name="viewModel">View model.</param>
		public MovieDetailAdapter(Activity activity, MovieDetailViewModel viewModel) : base(activity, viewModel)
		{
			
		}
		#endregion

		#region override methods for MovieCategoryAdapter

		// Replace the contents of a view (invoked by the layout manager)
		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var item = ViewModel.SimilarMovies;

			// Replace the contents of the view with that element
			var movieCategoryHolder = holder as MovieCategoryViewHolder;

			movieCategoryHolder.CategoryTitle.Text = GetCategoryName(item.CategoryName);
			movieCategoryHolder.CategoryRecyclerView.HasFixedSize = true;

			var movieAdapter = new MovieAdapter(item.Movies, (CloudDataMovieStore)ViewModel.DataStore);
			movieCategoryHolder.CategoryRecyclerView.SetAdapter(movieAdapter);

		}

		public override int ItemCount => ViewModel.SimilarMovies.Movies.Count;

		#endregion

	}

	public class MovieCategoryAdapter : BaseMovieCategoryAdapter<MovieExplorerViewModel>
	{
		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MovieMeApp.Droid.Fragments.MovieCategoryAdapter"/> class.
		/// </summary>
		/// <param name="activity">Activity.</param>
		/// <param name="viewModel">View model.</param>
		public MovieCategoryAdapter(Activity activity, MovieExplorerViewModel viewModel) : base(activity,viewModel)
		{
			ViewModel.Categories.CollectionChanged += (sender, args) =>
			{
				activity.RunOnUiThread(NotifyDataSetChanged);
			};
		}
		#endregion

		#region override methods for MovieCategoryAdapter

		// Replace the contents of a view (invoked by the layout manager)
		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var item = ViewModel.Categories[position];

			// Replace the contents of the view with that element
			var movieCategoryHolder = holder as MovieCategoryViewHolder;

	        movieCategoryHolder.CategoryTitle.Text = GetCategoryName(item.CategoryName);
			movieCategoryHolder.CategoryRecyclerView.HasFixedSize = true;

          	var movieAdapter = new MovieAdapter (item.Movies, (CloudDataMovieStore)ViewModel.DataStore);
          	movieCategoryHolder.CategoryRecyclerView.SetAdapter(movieAdapter);

		}

		public override int ItemCount => ViewModel.Categories.Count;

		#endregion

	}

	#endregion



}
