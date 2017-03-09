using Newtonsoft.Json;
using System;

using MovieMeApp.Helpers;
using MovieMeApp.Interfaces;

namespace MovieMeApp.Models
{
	/// <summary>
	/// Base data object.
	/// </summary>
	public class BaseDataObject : ObservableObject, IBaseDataObject
	{
		#region constructor
		public BaseDataObject()
		{
			Id = Guid.NewGuid().ToString();
		}
		#endregion

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
