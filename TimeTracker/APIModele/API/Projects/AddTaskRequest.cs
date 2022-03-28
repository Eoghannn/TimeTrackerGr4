using Newtonsoft.Json;

namespace TimeTracker.APIModel.API.Projects
{
    public class AddTaskRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}