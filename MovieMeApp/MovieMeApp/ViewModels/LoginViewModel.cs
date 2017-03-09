using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieMeApp.ViewModels
{
	public class LoginViewModel : BaseViewModel
	{
		string message = string.Empty;
		public string Message
		{
			get { return message; }
			set { message = value; OnPropertyChanged(); }
		}

		public ICommand LoginCommand { get; }
		public ICommand SignUpCommand { get; }

		public async Task SignUp()
		{
			try
			{
				IsBusy = true;
				Message = "Signing Up...";

				// Log the user in
				//await TryLoginAsync();
			}
			finally
			{
				Message = string.Empty;
				IsBusy = false;

			}
		}

		public async Task<bool> TryLoginAsync(string user, string password)
		{
			IsBusy = true;
			try
			{
				var userLogged = await AuthenticationStore.NewLogin(user, password);


			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				IsBusy = false;
			}
			return true;
		}
	}
}
