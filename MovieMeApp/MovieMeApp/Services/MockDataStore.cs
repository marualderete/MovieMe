using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieMeApp.Models;

namespace MovieMeApp.Services
{
	/// <summary>
	/// Mock data store. This is a mock class, to make tests at future when this project has a unit test project!
	/// </summary>
	public class MockDataStore : IDataStore<MovieStoreModel>
	{
		bool isInitialized;
		List<MovieStoreModel> items;

		public MockDataStore()
		{
			items = new List<MovieStoreModel>();

			var _items = new List<MovieStoreModel>
			{
				new MovieStoreModel 
				{ 
					Id = Guid.NewGuid().ToString(), 
					Page = 1, 
					TotalResults= 30, 
					Results = new MovieModel[]{new MovieModel()}
				},

			};

			foreach (MovieStoreModel item in _items)
			{
				items.Add(item);
			}
		}

		public Task<bool> FavoriteMovie(string id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<MovieStoreModel>> GetItemsAsync(bool forceRefresh = false)
		{
			throw new NotImplementedException();
		}

		public Task<MovieStoreModel> GetMovieAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<string> GetMovieCoverURL(string id)
		{
			throw new NotImplementedException();
		}

		public Task<MovieStoreModel> GetMovieStoreAsync(string filter)
		{
			throw new NotImplementedException();
		}

		public Task<MovieStoreModel> GetSimilarMovies(string id)
		{
			throw new NotImplementedException();
		}
	}
}
