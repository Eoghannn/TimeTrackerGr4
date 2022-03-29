using Newtonsoft.Json;

namespace TimeTracker.API
{
	public class ResponseStandard
	{
		[JsonProperty("is_success")]
		public bool IsSucess { get; set; }

		[JsonProperty("error_code")]
		public string ErrorCode { get; set; }

		[JsonProperty("error_message")]
		public string ErrorMessage { get; set; }
	}
}