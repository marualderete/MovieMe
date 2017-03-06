using MovieMeApp.Helpers;
using MovieMeApp.Models;
using MovieMeApp.Services;

namespace MovieMeApp.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Get the azure service instance
		/// </summary>
		public IDataStore<MovieStoreModel> DataStore => ServiceLocator.Instance.Get<IDataStore<MovieStoreModel>>();
		public IAuthenticationStore AuthenticationStore => ServiceLocator.Instance.Get<IAuthenticationStore>();

		bool isBusy = false;
		public bool IsBusy
		{
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}

		/// <summary>
		/// Private backing field to hold the title
		/// </summary>
		string title = string.Empty;

		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}
	}
}
