using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MovieMeApp.Helpers;
using MovieMeApp.Models;

namespace MovieMeApp.ViewModels
{
	public class MovieExplorerViewModel : BaseViewModel
	{
		public ObservableRangeCollection<MovieModel> TopRatedModels { get; set; }
		public ObservableRangeCollection<MovieModel> PopulardModels { get; set; }

		public Command LoadModelsCommand { get; set; }

		public MovieExplorerViewModel()
		{
			TopRatedModels = new ObservableRangeCollection<MovieModel>();
			PopulardModels = new ObservableRangeCollection<MovieModel>();

			//LoadModelsCommand = new Command(async () => await ExecuteLoadModelsCommand());

		}

		public async Task LoadModels(string category)
		{
			IsBusy = true;

			try
			{
				TopRatedModels.Clear();

				var dataStores = await DataStore.GetItemsAsync(category);
				var movies = dataStores.ToList().First().Results.ToList();

				if (category == AppConfig.TopRated)
				{
					TopRatedModels.ReplaceRange(movies);
				}
				if (category == AppConfig.Popular)
				{
					PopulardModels.ReplaceRange(movies);
				}

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
