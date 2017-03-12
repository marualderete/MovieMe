using System;
using Android.App;
using Android.Support.V7.Widget;
using MovieMeApp.Droid.Fragments.Holders;
using MovieMeApp.Services;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Fragments.Adapters
{
	/// <summary>
	/// Movie category adapter.
	/// </summary>
	public class MovieCategoryAdapter : BaseMovieCategoryAdapter<MovieExplorerViewModel>
	{
		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MovieMeApp.Droid.Fragments.MovieCategoryAdapter"/> class.
		/// </summary>
		/// <param name="activity">Activity.</param>
		/// <param name="viewModel">View model.</param>
		public MovieCategoryAdapter(Activity activity, MovieExplorerViewModel viewModel) : base(activity, viewModel)
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

			var movieAdapter = new MovieAdapter(item.Movies, (CloudDataMovieStore)ViewModel.DataStore);
			movieCategoryHolder.CategoryRecyclerView.SetAdapter(movieAdapter);

		}

		public override int ItemCount => ViewModel.Categories.Count;

		#endregion

	}
}
