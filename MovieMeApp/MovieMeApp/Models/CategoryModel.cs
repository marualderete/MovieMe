using System;
using System.Collections.ObjectModel;

namespace MovieMeApp
{
	/// <summary>
	/// Category model.
	/// </summary>
	public class CategoryModel
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the category.
		/// </summary>
		/// <value>The name of the category.</value>
		public string CategoryName { get; set; }

		/// <summary>
		/// Gets or sets the movies.
		/// </summary>
		/// <value>The movies.</value>
		public ObservableCollection<MovieModel> Movies { get; set; }
	}
}
