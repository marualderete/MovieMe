using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieMeApp.Models;
using MovieMeApp.Utils.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;

namespace MovieMeApp.Services
{
	public class CloudDataMovieStore : IDataStore<MovieStoreModel>
	{
		HttpClient client;
		List<MovieStoreModel> models;
		CloudAuthenticationStore authenticationStore;

		public CloudDataMovieStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");

			models = new List<MovieStoreModel>();
		}

		public async Task<IEnumerable<MovieStoreModel>> GetMovieStoreAsync(string filter)
		{
			//TODO: change appconfig string url's
			var url = string.Format(AppConfig.DataStoreSearchURL, filter, AppConfig.DataStoreApiKey, SortType.Desc);

			var json = await client.GetStringAsync(url);

			var storeModel = await Task.Run(() => JsonConvert.DeserializeObject<MovieStoreModel>(json));

			models.Add(storeModel);

			//TODO: check if success to cancel pending request!
			client.CancelPendingRequests();
			return models;
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

		public async Task<MovieStoreModel> GetMovieAsync(string id)
		{
			//https://api.themoviedb.org/3/movie/{0}?api_key={1}&{2}
			var url = string.Format(AppConfig.DataStoreSearchURL, id, AppConfig.DataStoreApiKey, SortType.Desc);
			var json = await client.GetStringAsync(url);

			var movie = await Task.Run(() => JsonConvert.DeserializeObject<MovieModel>(json));


			return new MovieStoreModel { Results = new MovieModel[]{ movie } };
		}

		public async Task<string> GetMovieCoverURL(string id)
		{
			var dataStore = await GetMovieAsync(id);

			//recuperar una movie por id, y sacar del result la url de la imagen-cover y armar el siguiente url
			//https://image.tmdb.org/t/p/w500/kqjL17yufvn9OVLyXYpvtyrFfak.jpg DataStoreMovieImageURL = "https://image.tmdb.org/t/p/{0}/{1}";
			var image = string.Format(AppConfig.DataStoreMovieImageURL, "w500", dataStore.Results.First().BackdropPath);

			return image; //TODO: return a bitmap
		}
	}
}
