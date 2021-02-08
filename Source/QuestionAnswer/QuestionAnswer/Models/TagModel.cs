using System.Text.Json.Serialization;

namespace QuestionAnswer.Models
{
    public class TagModel
    {
        [JsonPropertyName("tag_id")]
        public long tag_id { get; set; }
        [JsonPropertyName("tag_name")]
        public string tag_name { get; set; }
        public TagModel()
        {

        }
    }
}
