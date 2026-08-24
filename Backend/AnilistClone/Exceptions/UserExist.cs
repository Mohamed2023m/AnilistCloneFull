namespace AnilistClone.Exceptions
{
    public class UserExist : Exception
    {
        public UserExist()
            : base($"User exist") { }
    }
}
