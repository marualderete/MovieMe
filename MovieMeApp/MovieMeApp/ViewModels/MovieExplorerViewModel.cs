using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MovieMeApp.Helpers;

namespace MovieMeApp.ViewModels
{
	/// <summary>
	/// Movie explorer view model.
	/// </summary>
	public class MovieExplorerViewModel : BaseViewModel
	{
		#region public properties

		public ObservableCollection<CategoryModel> Categories { get; set; }
		public Command LoadModelsCommand { get; set; }

		public CategoryModel SimilarMovies { get; set; }
		public MovieModel Movie { get; set; }

		#endregion

		#region constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="T:MovieMeApp.ViewModels.MovieExplorerViewModel"/> class.
		/// </summary>
		public MovieExplorerViewModel()
		{
			Categories = new ObservableCollection<CategoryModel>();

		}
		#endregion

		#region public methods

		/// <summary>
		/// Loads the models.
		/// </summary>
		/// <returns>The models.</returns>
		/// <param name="category">Category.</param>
		public async Task LoadModels(string category)
		{
			IsBusy = true;

			try
			{
				var dataStores = await DataStore.GetMovieStoreAsync(category);
				var movies = dataStores.Results.ToList();

				CategoryModel aCategory = new CategoryModel
				{
					CategoryName = category,
					Movies = new ObservableCollection<MovieModel>(movies)
				};

				Categories.Add(aCategory);

			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		#endregion
	}
}
