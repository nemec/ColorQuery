using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ColorQuery
{
    public class Palette
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Username { get; set; }

        [JsonProperty("numViews")]
        public int Views { get; set; } 

        [JsonProperty("numVotes")]
        public int Votes { get; set; }

        [JsonProperty("numComments")]
        public int Comments { get; set; }

        [JsonProperty("numHearts")]
        public float Hearts { get; set; }

        public int Rank { get; set; }

        public DateTime Created { get; set; }

        public List<string> Colors { get; set; }

        public string Description { get; set; }
    }
}