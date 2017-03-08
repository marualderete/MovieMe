using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieMeApp.Services
{
	public interface IDataStore<T>
	{
		Task<bool> AddItemAsync(T item);
		Task<bool> UpdateItemAsync(T item);
		Task<bool> DeleteItemAsync(string id);
		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

		Task<T> GetMovieAsync(string id);
		Task<IEnumerable<T>> GetMovieStoreAsync(string filter);
		Task<string> GetMovieCoverURL(string id);
	}
}
