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
		Dictionary<string,MovieStoreModel> models;
		CloudAuthenticationStore authenticationStore;

		#endregion

		#region constructor
		public CloudDataMovieStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");

			models = new Dictionary<string,MovieStoreModel>();
		}
		#endregion

		#region interface methods implementations

		/// <summary>
		/// Gets the movie store async. It Consumes the API to get a list of movies according to the category passed by param.
		/// </summary>
		/// <returns>The movie store async.</returns>
		/// <param name="filter">Filter.</param>
		public async Task<MovieStoreModel> GetMovieStoreAsync(string filter)
		{
			var url = string.Format(AppConfig.DataStoreSearchURL, filter, AppConfig.DataStoreApiKey, SortType.Desc);

            try {
                var json = await client.GetStringAsync (url);
				var storeModel = await Task.Run (() => JsonConvert.DeserializeObject<MovieStoreModel> (json));

				if (models.ContainsKey(filter))
				{
					models[filter] = storeModel;
				}
				else
				{
					models.Add(filter, storeModel);
				}

            } catch (Exception ex) {
                Console.WriteLine ("Was an error trying to get movies from API: ", ex.Message);
            } finally 
            {
                //TODO: check if success to cancel pending request!
                client.CancelPendingRequests ();
            }
			
			return models[filter];
		}

		/// <summary>
		/// Gets the movie async.
		/// </summary>
		/// <returns>The movie async.</returns>
		/// <param name="id">Identifier.</param>
		public async Task<MovieStoreModel> GetMovieAsync(string id)
		{
			var url = string.Format(AppConfig.DataStoreSearchURL, id, AppConfig.DataStoreApiKey, SortType.Desc);

            try
            {
                var json = await client.GetStringAsync (url);
                var movie = await Task.Run (() => JsonConvert.DeserializeObject<MovieModel> (json));

                return new MovieStoreModel { Results = new MovieModel [] { movie } };

            } catch (Exception ex) 
            {
                Console.WriteLine ("Was an error trying to get selected movie from API: ", ex.Message);

                return new MovieStoreModel();
            }
			
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
            var currentMovie = dataStore.Results.Any() ? dataStore.Results.First() : null;

            if (currentMovie == null) 
            {
                Console.WriteLine ("There is no movie for your id");
                return string.Empty;
            }

            var image = string.Format(AppConfig.DataStoreMovieImageURL, "w500", currentMovie.BackdropPath);
			return image;
		}

		public async Task<bool> FavoriteMovie(string id)
		{
			////https://api.themoviedb.org/3/account/9999/favorite?api_key=ab41356b33d100ec61e6c098ecc92140&session_id=9999
			var url = string.Format(AppConfig.PostFavoriteURL, authenticationStore.SessionID, AppConfig.DataStoreApiKey, authenticationStore.SessionID);

			return false;
			//var dataStore = await GetMovieAsync(id);
			//var currentMovie = dataStore.Results.Any() ? dataStore.Results.First() : null;

			//if (currentMovie == null)
			//{
			//	Console.WriteLine("There is no movie for your id");
			//	return string.Empty;
			//}

			//var image = string.Format(AppConfig.DataStoreMovieImageURL, "w500", currentMovie.BackdropPath);
			//return image;
		}

		public Task<IEnumerable<MovieStoreModel>> GetItemsAsync(bool forceRefresh = false)
		{
			throw new NotImplementedException();
		}



		#endregion

	}
}
