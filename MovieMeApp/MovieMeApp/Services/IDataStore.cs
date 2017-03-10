using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieMeApp.Services
{
	public interface IDataStore<T>
	{
		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

		Task<T> GetMovieAsync(string id);
		Task<T> GetMovieStoreAsync(string filter);
		Task<string> GetMovieCoverURL(string id);

		Task<bool> FavoriteMovie(string id);
	}
}
