using System.Text;
using System.Text.Json;
using AnilistClone.Exceptions;
using AnilistClone.Models;
using AnilistClone.Services.Interfaces;

namespace AnilistClone.Services
{
    public class MediaService : IMediaService
    {
        private readonly HttpClient _client;
        private readonly string _anilistApiUrl;
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public MediaService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _anilistApiUrl = config["AnilistApiUrl"];
        }

        public async Task<Media> GetMedia(int id)
        {
            string graphQLQuery = """
query ($id: Int) {
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
}
""";

            var variable = new { id = id };

            var payload = new { query = graphQLQuery, variables = variable };

            string jsonPayload = JsonSerializer.Serialize(payload);

            using var request = new HttpRequestMessage(HttpMethod.Post, _anilistApiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"),
            };

            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"AniList API returned {response.StatusCode}");
            }

            var showResponse = await response.Content.ReadFromJsonAsync<
                GraphQLResponse<MediaWrapper>
            >(JsonOptions);

            if (showResponse?.Data?.Media == null)
            {
                throw new MediaNotFoundException(id);
            }
            return showResponse?.Data?.Media;
        }

        public async Task<IEnumerable<Media>> GetAllMedia(int currentPage)
        {
            string graphQLQuery = """
query ($currentPage: Int) {
  Page(page: $currentPage, perPage: 5) {
    media(sort: POPULARITY_DESC type: ANIME countryOfOrigin: JP status: RELEASING) {
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
}
""";

            var variable = new { currentPage = currentPage };

            var payload = new { query = graphQLQuery, variables = variable };

            string jsonPayload = JsonSerializer.Serialize(payload);

            using var request = new HttpRequestMessage(HttpMethod.Post, _anilistApiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"),
            };

            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"AniList API returned {response.StatusCode}");
            }

            var showResponse = await response.Content.ReadFromJsonAsync<
                GraphQLResponse<MediaWrapper>
            >(JsonOptions);

            return showResponse?.Data?.Page?.media ?? Enumerable.Empty<Media>();
        }

        public async Task<IEnumerable<Media>> SearchMedia(string search)
        {
            string graphQLQuery = """
query ($search: String) {
  Page(page: 1, perPage: 5) {
    media(search: $search type: ANIME countryOfOrigin: JP) {
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
}
""";
            var variable = new { search = search };

            var payload = new { query = graphQLQuery, variables = variable };

            string jsonPayload = JsonSerializer.Serialize(payload);

            using var request = new HttpRequestMessage(HttpMethod.Post, _anilistApiUrl)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json"),
            };

            using var response = await _client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"AniList API returned {response.StatusCode}");
            }
            var showResponse = await response.Content.ReadFromJsonAsync<
                GraphQLResponse<MediaWrapper>
            >(JsonOptions);

            return showResponse?.Data?.Page?.media ?? Enumerable.Empty<Media>();
        }
    }
}
