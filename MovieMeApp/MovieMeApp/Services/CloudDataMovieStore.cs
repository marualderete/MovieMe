﻿using System;
using System.Collections.Generic;
using System.IO;
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
			//client = new HttpClient();
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
				return models[filter];

            } catch (Exception ex) {
                Console.WriteLine ("Was an error trying to get movies from API: ", ex.Message);
				return null;
            } finally 
            {
                //TODO: check if success to cancel pending request!
                client.CancelPendingRequests ();
            }
			

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
			////https://api.themoviedb.org/3/account/{0}/favorite?api_key={1}&session_id={2}
			var url = string.Format(AppConfig.PostFavoriteURL, authenticationStore.AccountID, AppConfig.DataStoreApiKey, authenticationStore.SessionID);
			using (var content =
					new MultipartFormDataContent())
			{
				using (var message = await client.PostAsync(url, content))
				{
					var input = await message.Content.ReadAsStringAsync();

					return true;
				}
			}
		}

		public async Task<MovieStoreModel> GetSimilarMovies(string id)
		{ 
			var url = string.Format(AppConfig.GetSimilarMoviesURL, id, AppConfig.DataStoreApiKey);
			try
			{
				var json = await client.GetStringAsync(url);
				var storeModel = await Task.Run(() => JsonConvert.DeserializeObject<MovieStoreModel>(json));

				return storeModel;

			}
			catch (Exception ex)
			{
				Console.WriteLine("Was an error trying to get selected movie from API: ", ex.Message);

				return new MovieStoreModel();
			}
		}

		public Task<IEnumerable<MovieStoreModel>> GetItemsAsync(bool forceRefresh = false)
		{
			throw new NotImplementedException();
		}


		#endregion

	}
}
