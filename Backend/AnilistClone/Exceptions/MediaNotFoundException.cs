namespace AnilistClone.Exceptions
{
    public class MediaNotFoundException : Exception
    {
        public MediaNotFoundException(int id)
            : base($"Media not found for id {id}") { }
    }
}
