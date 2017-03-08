using System;
using System.Threading.Tasks;

namespace MovieMeApp.Services
{
	public interface IAuthenticationStore
	{
		Task<bool> NewLogin(string user, string password);

		Task<bool> CreateNewSession();

		Task<bool> IsValidSession();
	}
}
