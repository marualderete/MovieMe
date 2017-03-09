using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

namespace MovieMeApp.Models
{
	/// <summary>
	/// Movie store model.
	/// </summary>
	public class MovieStoreModel : BaseDataObject
	{

		/// <summary>
		/// The page.
		/// </summary>
		int page = 1;

		[JsonProperty(PropertyName = "page")]
		public int Page
		{
			get { return page; }
			set { SetProperty(ref page, value); }
		}

		/// <summary>
		/// The total results.
		/// </summary>
		int total_results = 0;

		[JsonProperty(PropertyName = "total_results")]
		public int TotalResults
		{
			get { return total_results; }
			set { SetProperty(ref total_results, value); }
		}

		/// <summary>
		/// The total pages.
		/// </summary>
		int total_pages = 1;

		[JsonProperty(PropertyName = "total_pages")]
		public int TotalPages
		{
			get { return total_pages; }
			set { SetProperty(ref total_pages, value); }
		}

		/// <summary>
		/// The results.
		/// </summary>
		List<MovieModel> results = new List<MovieModel>();

		[JsonProperty(PropertyName = "results")]
		public MovieModel[] Results
		{
			get { return results.ToArray(); }
			set { SetProperty(ref results, value.ToList()); }
		}
	}
}
