using Newtonsoft.Json;

namespace TimeTracker.API.Authentications.Credentials
{
	public class PasswordForgottenRequestTest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
	}
}