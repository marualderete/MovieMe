using System;
using Android.App;
using Android.Support.V7.Widget;
using MovieMeApp.Droid.Fragments.Holders;
using MovieMeApp.Services;
using MovieMeApp.ViewModels;

namespace MovieMeApp.Droid.Fragments.Adapters
{

	/// <summary>
	/// Movie detail adapter.
	/// </summary>
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
}
