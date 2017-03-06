using Newtonsoft.Json;
using System;

using MovieMeApp.Helpers;
using MovieMeApp.Interfaces;

namespace MovieMeApp.Models
{
	public class BaseDataObject : ObservableObject, IBaseDataObject
	{
		public BaseDataObject()
		{
			Id = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Id for item
		/// </summary>
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		/// <summary>
		/// Azure created at time stamp
		/// </summary>
		[JsonProperty(PropertyName = "createdAt")]
		public DateTimeOffset CreatedAt { get; set; }
	}
}
