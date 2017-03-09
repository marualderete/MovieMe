using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using MovieMeApp.Models;
using MovieMeApp.Utils.Enums;
using Newtonsoft.Json;

namespace MovieMeApp.Services
{
	/// <summary>
	/// Cloud data movie store.
	/// </summary>
	public class CloudDataMovieStore : IDataStore<MovieStoreModel>
	{
		#region private properties
		HttpClient client;
		List<MovieStoreModel> models;
		CloudAuthenticationStore authenticationStore;
		#endregion

		#region constructor
		public CloudDataMovieStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");

			models = new List<MovieStoreModel>();
		}
		#endregion

		#region interface methods implementations

		/// <summary>
		/// Gets the movie store async. It Consumes the API to get a list of movies according to the category passed by param.
		/// </summary>
		/// <returns>The movie store async.</returns>
		/// <param name="filter">Filter.</param>
		public async Task<IEnumerable<MovieStoreModel>> GetMovieStoreAsync(string filter)
		{
			var url = string.Format(AppConfig.DataStoreSearchURL, filter, AppConfig.DataStoreApiKey, SortType.Desc);
			var json = await client.GetStringAsync(url);
			var storeModel = await Task.Run(() => JsonConvert.DeserializeObject<MovieStoreModel>(json));

			models.Add(storeModel);

			//TODO: check if success to cancel pending request!
			client.CancelPendingRequests();
			return models;
		}

		/// <summary>
		/// Gets the movie async.
		/// </summary>
		/// <returns>The movie async.</returns>
		/// <param name="id">Identifier.</param>
		public async Task<MovieStoreModel> GetMovieAsync(string id)
		{
			var url = string.Format(AppConfig.DataStoreSearchURL, id, AppConfig.DataStoreApiKey, SortType.Desc);
			var json = await client.GetStringAsync(url);

			var movie = await Task.Run(() => JsonConvert.DeserializeObject<MovieModel>(json));


			return new MovieStoreModel { Results = new MovieModel[] { movie } };
		}

		/// <summary>
		/// Gets the movie cover URL. This methods consumes the API to retrieve 
		/// the movie cover url according to the movie ID
		/// </summary>
		/// <returns>The movie cover URL.</returns>
		/// <param name="id">Identifier.</param>
		public async Task<string> GetMovieCoverURL(string id)
		{
			var dataStore = await GetMovieAsync(id);
			var image = string.Format(AppConfig.DataStoreMovieImageURL, "w500", dataStore.Results.First().BackdropPath);

			return image;
		}

		public Task<bool> UpdateItemAsync(MovieStoreModel item)
		{
			throw new NotImplementedException();
		}

		public Task<bool> AddItemAsync(MovieStoreModel item)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteItemAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<MovieStoreModel>> GetItemsAsync(bool forceRefresh = false)
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}
