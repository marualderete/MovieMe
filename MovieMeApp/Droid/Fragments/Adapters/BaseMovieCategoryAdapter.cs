using System;
using Android.App;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;
using MovieMeApp.Droid.Fragments.Holders;
using MovieMeApp.Utils;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Fragments.Adapters
{
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
			_utils = new StringUtils();
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
}
