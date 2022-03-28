using Newtonsoft.Json;

namespace TimeTracker.API.Authentications.Credentials
{
	public class SetPasswordRequestTest
	{
		[JsonProperty("old_password")]
		public string OldPassword { get; set; }

		[JsonProperty("new_password")]
		public string NewPassword { get; set; }
	}
}