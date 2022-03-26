using Newtonsoft.Json;

namespace TimeTracker.API.Projects
{
    public class AddTaskRequest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}