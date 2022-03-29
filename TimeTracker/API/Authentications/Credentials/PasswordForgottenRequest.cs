using Newtonsoft.Json;

namespace TimeTracker.API.Authentications.Credentials
{
	public class PasswordForgottenRequest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
	}
}