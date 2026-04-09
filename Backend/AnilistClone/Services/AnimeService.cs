using AnilistClone.Models;
using System.Text;
using Newtonsoft.Json;
using AnilistClone.Services.Interfaces;
using GraphQL.Validation;
using Microsoft.Extensions.Configuration;

namespace AnilistClone.Services
{
    public class AnimeService : IAnimeService
    {
        private readonly HttpClient _client;
        private readonly string _anilistApiUrl;

        public AnimeService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _anilistApiUrl = config["AnilistApiUrl"];
        }

        public async Task<Show> GetShow(int id)
        {
            
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

            using var request = new HttpRequestMessage(HttpMethod.Post, _anilistApiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {

                throw new HttpRequestException($"AniList API returned {response.StatusCode}");
            }
            string responseData = await response.Content.ReadAsStringAsync();
            var showResponse = JsonConvert.DeserializeObject<GraphQLResponse<MediaWrapper>>(responseData);

            return showResponse?.Data?.Media
       ?? throw new InvalidOperationException($"No media found for id {id}");




        }

        public async Task<IEnumerable<Show>> GetShows(int currentPage)
        {
      

            string graphQLQuery = @"query( $currentPage: Int) {
Page(page: $currentPage, perPage: 5) {
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

            var variable = new
            {
                currentPage = currentPage,

            };


            var payload = new
            {
                query = graphQLQuery,
                variables = variable

            };

            string jsonPayload = JsonConvert.SerializeObject(payload);



            using var request = new HttpRequestMessage(HttpMethod.Post, _anilistApiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };


            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {

                throw new HttpRequestException($"AniList API returned {response.StatusCode}");
            }

            string responseData = await response.Content.ReadAsStringAsync();
            var showResponse = JsonConvert.DeserializeObject<GraphQLResponse<MediaWrapper>>(responseData);

            return showResponse?.Data?.Page?.media ?? Enumerable.Empty<Show>();


        }

        public async Task<IEnumerable<Show>> SearchShows(string search)
        {
       

            string graphQLQuery = @"query($search: String) {
  Page(page: 1, perPage: 5) {
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
            var variable = new
            {
                search = search,

            };



            var payload = new
            {
                query = graphQLQuery,
                variables = variable
            };

            string jsonPayload = JsonConvert.SerializeObject(payload);



            using var request = new HttpRequestMessage(HttpMethod.Post, _anilistApiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            using var response = await _client.SendAsync(request);
         
            if (!response.IsSuccessStatusCode)
            {
       
                throw new HttpRequestException($"AniList API returned {response.StatusCode}");
            }

            string responseData = await response.Content.ReadAsStringAsync();
            var showResponse = JsonConvert.DeserializeObject<GraphQLResponse<MediaWrapper>>(responseData);
            
            return showResponse?.Data?.Page?.media ?? Enumerable.Empty<Show>();






        }
    }
}
