using System;
using System.Threading.Tasks;

namespace MovieMeApp.Services
{
	public interface IAuthenticationStore
	{
		Task<bool> ValidateLogin(string user, string password);
	}
}
