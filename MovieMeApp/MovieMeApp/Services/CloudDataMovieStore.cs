using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MovieMeApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;

namespace MovieMeApp.Services
{
	public class CloudDataMovieStore : IDataStore<MovieStoreModel>
	{
		HttpClient client;
		List<MovieStoreModel> models;

		public CloudDataMovieStore()
		{
			client = new HttpClient();
			client.BaseAddress = new Uri($"{AppConfig.DataStoreBaseURL}/");

			models = new List<MovieStoreModel>();
		}

		public async Task<IEnumerable<MovieStoreModel>> GetItemsAsync(string filter)
		{
			//TODO: change appconfig string url's
			var url = string.Format(AppConfig.DataStoreSearchURL, filter, AppConfig.DataStoreApiKey, "&amp;", AppConfig.SortByPopularity);

			var json = await client.GetStringAsync(url);

			var storeModel = await Task.Run(() => JsonConvert.DeserializeObject<MovieStoreModel>(json));

			models.Add(storeModel);

			client.Dispose();
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

		public Task<MovieStoreModel> GetItemAsync(string id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<MovieStoreModel>> GetItemsAsync(bool forceRefresh = false)
		{
			throw new NotImplementedException();
		}
	}
}
