using System;
using System.Collections.ObjectModel;

namespace MovieMeApp
{
	public class CategoryModel
	{
		public string Id { get; set; }
		public string CategoryName { get; set; }
		public ObservableCollection<MovieModel> Movies { get; set; }
	}
}
