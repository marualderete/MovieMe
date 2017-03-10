using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieMeApp.Models;

namespace MovieMeApp.ViewModels
{
	/// <summary>
	/// Movie detail view model.
	/// </summary>
	public class MovieDetailViewModel : BaseViewModel
	{
		#region public properties
		public CategoryModel SimilarMovies { get; set; }

		public MovieModel Movie { get; set; }

		#endregion

		#region constructor
		public MovieDetailViewModel(MovieModel current = null)
		{
			if (current != null)
			{
				Title = current.Title;
				Movie = current;
			}
		}
		#endregion

		#region public methods

		/// <summary>
		/// Loads the models.
		/// </summary>
		/// <returns>The models.</returns>
		/// <param name="category">Category.</param>
		public async Task<string> GetMovieCover(string id)
		{
			IsBusy = true;

			try
			{
				var uri = await DataStore.GetMovieCoverURL(id);
				return uri;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
				return string.Empty;
			}
			finally
			{
				IsBusy = false;
			}
		}

		#endregion
	}
}
