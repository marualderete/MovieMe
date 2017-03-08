using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MovieMeApp.Helpers;
using MovieMeApp.Models;

namespace MovieMeApp.ViewModels
{
	public class MovieExplorerViewModel : BaseViewModel
	{
		public ObservableCollection<CategoryModel> Categories { get; set; }

		public Command LoadModelsCommand { get; set; }

		public MovieExplorerViewModel()
		{
			Categories = new ObservableCollection<CategoryModel>();
			//LoadModelsCommand = new Command(async () => await ExecuteLoadModelsCommand());

		}

		public async Task LoadModels(string category)
		{
			IsBusy = true;

			try
			{
				//TopRatedModels.Clear();

				var dataStores = await DataStore.GetMovieStoreAsync(category);
				var movies = dataStores.ToList().First().Results.ToList();

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
				// MessageDialog.SendMessage("Unable to load items.", "Error");
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
