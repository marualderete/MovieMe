using System;
using System.Collections.ObjectModel;

using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
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
	/// <summary>
	/// Movie category fragment.
	/// </summary>
	public class MovieCategoryFragment : Android.App.Fragment, IFragmentVisible
	{
		#region private properties

		MovieCategoryAdapter adapter;
		SwipeRefreshLayout refresher;
		ProgressBar progress;

		#endregion

		#region public properties
		public static MovieCategoryFragment NewInstance() =>
			new MovieCategoryFragment { Arguments = new Bundle() };

		public MovieExplorerViewModel ViewModel
		{
			get;
			set;
		}

		public void BecameVisible()
		{
		}

		#endregion

		#region override methods for MovieCategoryFragment
		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            View view = inflater.Inflate (Resource.Layout.movie_category_list, container, false);

            refresher = view.FindViewById<SwipeRefreshLayout> (Resource.Id.refresher);
            refresher.SetColorSchemeColors (Resource.Color.accent);
            refresher.Refreshing = true;

            progress = view.FindViewById<ProgressBar> (Resource.Id.progressbar_frame_container);
            progress.Visibility = ViewStates.Visible;

			var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			recyclerView.HasFixedSize = true;
			recyclerView.SetAdapter(adapter = new MovieCategoryAdapter(Activity, ViewModel));

            progress.Visibility = ViewStates.Gone;
            refresher.Refreshing = false;
			return view;
		}

		public override void OnStart()
		{
			base.OnStart();

			refresher.Refresh += Refresher_Refresh;
			adapter.ItemClick += Adapter_ItemClick;
		}

		public override void OnStop()
		{
			base.OnStop();
			refresher.Refresh -= Refresher_Refresh;
			adapter.ItemClick -= Adapter_ItemClick;
		}

		#endregion

		#region private methods
		void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
		{
			var item = ViewModel.Categories[e.Position];
			var intent = new Intent(Activity, typeof(BrowseItemDetailActivity));

			intent.PutExtra("data", Newtonsoft.Json.JsonConvert.SerializeObject(item));
			Activity.StartActivity(intent);
		}

		void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
		}

		async void Refresher_Refresh(object sender, EventArgs e)
		{
            refresher.Refreshing = true;

			ViewModel.Categories.Clear();
			await ViewModel.LoadModels(AppConfig.TopRated);
			await ViewModel.LoadModels(AppConfig.Popular);
            await ViewModel.LoadModels (AppConfig.Similar);

            refresher.Refreshing = false;
		}

		#endregion
	}

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

        void Image_ItemClick (RecyclerClickEventArgs e)
        {
            var item = _movies [e.Position];

            var intent = new Intent (_context.ApplicationContext, typeof (BrowseItemDetailActivity));

            //intent.PutExtra ("data", Newtonsoft.Json.JsonConvert.SerializeObject (item));
            //Activity.StartActivity (intent);
        }
		#endregion
	}

	/// <summary>
	/// Movie category adapter.
	/// </summary>
	class MovieCategoryAdapter : BaseRecycleViewAdapter
	{
		#region private properties

		MovieExplorerViewModel _viewModel;
        StringUtils _utils;
		Activity _activity;
		Context _context;

		#endregion

		#region constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MovieMeApp.Droid.Fragments.MovieCategoryAdapter"/> class.
        /// </summary>
        /// <param name="activity">Activity.</param>
        /// <param name="viewModel">View model.</param>
		public MovieCategoryAdapter(Activity activity, MovieExplorerViewModel viewModel)
		{
			_viewModel = viewModel;
            _utils = new StringUtils ();
			_activity = activity;

			_viewModel.Categories.CollectionChanged += (sender, args) =>
			{
				_activity.RunOnUiThread(NotifyDataSetChanged);
			};
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

		// Replace the contents of a view (invoked by the layout manager)
		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var item = _viewModel.Categories[position];

			// Replace the contents of the view with that element
			var movieCategoryHolder = holder as MovieCategoryViewHolder;

            movieCategoryHolder.CategoryTitle.Text = _utils.GetCategoryName (item.CategoryName);

			movieCategoryHolder.CategoryRecyclerView.HasFixedSize = true;

            var movieAdapter = new MovieAdapter (item.Movies, (CloudDataMovieStore)_viewModel.DataStore);
            movieCategoryHolder.CategoryRecyclerView.SetAdapter(movieAdapter);

		}

		public override int ItemCount => _viewModel.Categories.Count;

        #endregion

	}

	#endregion


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
}
