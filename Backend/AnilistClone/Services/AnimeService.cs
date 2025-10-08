using AnilistClone.Models;
using System.Text;
using Newtonsoft.Json;
using AnilistClone.Services.Interfaces;
using GraphQL.Validation;

namespace AnilistClone.Services
{
    public class AnimeService : IAnimeService
    {
        private HttpClient _client;

        public AnimeService(HttpClient client)
        {
            _client = client;
        }

        public async Task<Show> GetShow(int id)
        {
            String apiUrl = "https://graphql.anilist.co";


        
           

                string graphQLQuery = @"query ($id: Int) {
  Media(id: $id) {
    id
coverImage {
extraLarge
large
medium
color
}
bannerImage
episodes
    title {
      romaji
      english
    }
genres
    seasonYear
    description
  }
}";


                var variable = new { id = id };

                var payload = new
                {
                    query = graphQLQuery,
                    variables = variable
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);



            using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl) {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

                using var response = await _client.SendAsync(request);

                    string responseData = await response.Content.ReadAsStringAsync();
                var showResponse = JsonConvert.DeserializeObject<GraphQLResponse<MediaWrapper>>(responseData);



            return showResponse?.Data?.Media
       ?? throw new InvalidOperationException($"No media found for id {id}");




        }

        public async Task<IEnumerable<Show>> GetShows()
        {
            String apiUrl = "https://graphql.anilist.co";

            string graphQLQuery = @"query {Page(page: 1, perPage: 50) {
    media( sort: POPULARITY_DESC type: ANIME countryOfOrigin: JP status: RELEASING) {
      id
      coverImage {
        extraLarge
        large
        medium
        color

      }
      title {
        romaji
        english
      }
      
   
    }
  }
}";
            var payload = new
            {
                query = graphQLQuery,
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);



            using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };


            using var response = await _client.SendAsync(request);

            string responseData = await response.Content.ReadAsStringAsync();
            var showResponse = JsonConvert.DeserializeObject<GraphQLResponse<MediaWrapper>>(responseData);

            return showResponse?.Data?.Page?.media ?? Enumerable.Empty<Show>();






        }

        public async Task<IEnumerable<Show>> SearchShows(string search)
        {
            String apiUrl = "https://graphql.anilist.co";

            string graphQLQuery = @"query($search: String) {
  Page(page: 1, perPage: 10) {
     media( search: $search type: ANIME countryOfOrigin: JP) {
      id
      coverImage {
        extraLarge
        large
        medium
        color
      }
     startDate {
        year
        month
        day
      }
      endDate {
        year
        month
        day
      }
      episodes
      title {
        romaji
        english
      }
      seasonYear
      description
    }
  }
}";
            var variable = new { search = search };


            var payload = new
            {
                query = graphQLQuery,
                variables = variable
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);



            using var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);

            string responseData = await response.Content.ReadAsStringAsync();
            var showResponse = JsonConvert.DeserializeObject<GraphQLResponse<MediaWrapper>>(responseData);

            return showResponse?.Data?.Page?.media ?? Enumerable.Empty<Show>();






        }
    }
}
