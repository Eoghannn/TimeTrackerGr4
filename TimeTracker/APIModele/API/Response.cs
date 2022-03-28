
using Newtonsoft.Json;

namespace TimeTracker.APIModel.API
{
    public class Response<T>
    {
		[JsonProperty("data")]
		public T Data { get; set; }

		[JsonProperty("is_success")]
		public bool IsSucess { get; set; }

		[JsonProperty("error_code")]
		public string ErrorCode { get; set; }

		[JsonProperty("error_message")]
		public string ErrorMessage { get; set; }
	}
}