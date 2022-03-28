using Newtonsoft.Json;

namespace TimeTracker.API.Projects
{
    public class AddProjectRequestTest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}