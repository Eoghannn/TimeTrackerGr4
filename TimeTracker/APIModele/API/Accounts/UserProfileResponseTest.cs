using Newtonsoft.Json;

namespace TimeTracker.APIModel.API.Accounts
{
	public class UserProfileResponseTest
	{
		[JsonProperty("email")]
		public string Email { get; set; }
		
		[JsonProperty("first_name")]
		public string FirstName { get; set; }
		
		[JsonProperty("last_name")]
		public string LastName { get; set; }
	}
}