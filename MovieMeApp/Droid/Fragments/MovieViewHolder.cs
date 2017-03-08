using System;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Android.Support.V4.Widget;
using Android.App;
using Android.Content;
using MovieMeApp.ViewModels;
using MovieMeApp.Helpers;
using MovieMeApp.Services;
using MovieMeApp.Droid.Activities;

using Uri = Android.Net.Uri;
using System.Linq;
using Square.Picasso;
using System.Collections.ObjectModel;

namespace MovieMeApp.Droid.Fragments
{
	public class MovieCategoryFragment : Android.App.Fragment, IFragmentVisible
	{
		public static MovieCategoryFragment NewInstance() =>
			new MovieCategoryFragment { Arguments = new Bundle() };

		MovieCategoryAdapter adapter;
		SwipeRefreshLayout refresher;

		ProgressBar progress;
		public MovieExplorerViewModel ViewModel
		{
			get;
			set;
		}

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			//ViewModel = new MovieExplorerViewModel();

			//ServiceLocator.Instance.Register<MockDataStore, MockDataStore>();

			View view = inflater.Inflate(Resource.Layout.movie_category_list, container, false);
			var recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);

			recyclerView.HasFixedSize = true;
			recyclerView.SetAdapter(adapter = new MovieCategoryAdapter(Activity, ViewModel));

			refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);

			refresher.SetColorSchemeColors(Resource.Color.accent);

			progress = view.FindViewById<ProgressBar>(Resource.Id.progressbar_loading);
			progress.Visibility = ViewStates.Gone;

			return view;
		}

		public override void OnStart()
		{
			base.OnStart();

			refresher.Refresh += Refresher_Refresh;
			//ViewModel.PropertyChanged += ViewModel_PropertyChanged;
			adapter.ItemClick += Adapter_ItemClick;

			//if (ViewModel.Items.Count == 0)
			//	ViewModel.LoadItemsCommand.Execute(null);
		}

		public override void OnStop()
		{
			base.OnStop();
			refresher.Refresh -= Refresher_Refresh;
			//ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
			adapter.ItemClick -= Adapter_ItemClick;
		}

		private void Adapter_ItemClick(object sender, RecyclerClickEventArgs e)
		{
			var item = ViewModel.Categories[e.Position];
			var intent = new Intent(Activity, typeof(BrowseItemDetailActivity));

			intent.PutExtra("data", Newtonsoft.Json.JsonConvert.SerializeObject(item));
			Activity.StartActivity(intent);
		}

		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
		}

		private async void Refresher_Refresh(object sender, EventArgs e)
		{
			await ViewModel.LoadModels(AppConfig.TopRated);
			//await ViewModel.LoadModels(AppConfig.Popular);
		}

		public void BecameVisible()
		{
		}
	}

	class MovieAdapter : BaseRecycleViewAdapter
	{ 
		Context _context;
		ObservableCollection<MovieModel> _movies;
		CloudDataMovieStore _dataMovieStore;

		public MovieAdapter(ObservableCollection<MovieModel> movies, CloudDataMovieStore movieStore)
		{
			_movies = movies;
			_dataMovieStore = movieStore;
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			//Setup your layout here
			View itemView = null;

			_context = parent.Context;

			var id = Resource.Layout.movie_item;
			itemView = LayoutInflater.From(parent.Context).Inflate(id, parent, false);
			var vh = new MovieViewHolder(itemView, OnClick, OnLongClick);
			return vh;
		}

		public override async void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			var movieHolder = holder as MovieViewHolder;
			var movie = _movies[position];

			var url = await _dataMovieStore.GetMovieCoverURL(movie.Id);
			Uri uri = Uri.Parse(url);
			Picasso.With(_context).Load(uri).Into(movieHolder.MovieCover);

		}

		public override int ItemCount => _movies.Count;
	}

	class MovieCategoryAdapter : BaseRecycleViewAdapter
	{
		MovieExplorerViewModel _viewModel;
		Activity _activity;

		Context _context;

		public MovieCategoryAdapter(Activity activity, MovieExplorerViewModel viewModel)
		{
			_viewModel = viewModel;
			_activity = activity;

			//TODO: necesito un adapter para cada re-utilizacion del fragment????
			_viewModel.Categories.CollectionChanged += (sender, args) =>
			{
				_activity.RunOnUiThread(NotifyDataSetChanged);
			};
		}

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

			//mAdapter.notifyItemInserted(mItems.size() - 1);
			movieCategoryHolder.CategoryTitle.Text = item.CategoryName;

			movieCategoryHolder.CategoryRecyclerView.HasFixedSize = true;
			movieCategoryHolder.CategoryRecyclerView.SetAdapter(new MovieAdapter(item.Movies, (MovieMeApp.Services.CloudDataMovieStore)_viewModel.DataStore));

		}

		public override int ItemCount => _viewModel.Categories.Count;
	}



	public class MovieViewHolder : RecyclerView.ViewHolder
	{
		public ImageView MovieCover { get; set; }

		public MovieViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
							Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
		{

			MovieCover = itemView.FindViewById<ImageView>(Resource.Id.movie_cover);
			MovieCover.Click += (sender, e) => clickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
		}
	}

	public class MovieCategoryViewHolder : RecyclerView.ViewHolder
	{
		//public TextView TextView { get; set; }
		public TextView CategoryTitle { get; set; }
		public RecyclerView CategoryRecyclerView { get; set; }


		public MovieCategoryViewHolder(View itemView, Action<RecyclerClickEventArgs> clickListener,
							Action<RecyclerClickEventArgs> longClickListener) : base(itemView)
		{
			CategoryTitle = itemView.FindViewById<TextView>(Resource.Id.movie_title);
			CategoryRecyclerView = itemView.FindViewById<RecyclerView>(Resource.Id.category_recycler_view);

			//this could be a shorcut to put this movie in to Favorites :)
			//itemView.LongClick += (sender, e) => longClickListener(new RecyclerClickEventArgs { View = itemView, Position = AdapterPosition });
		}
	}
}
