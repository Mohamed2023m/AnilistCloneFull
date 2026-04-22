namespace AnilistClone.Models
{
    public class GraphQLResponse<T>
    {
        public T? Data { get; set; } // This matches the "Media" property in the response
    }
}
