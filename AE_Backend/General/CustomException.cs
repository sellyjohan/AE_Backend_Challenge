namespace AE_Backend.General
{
    public class CustomException
    {
        public class UserNotFoundException : Exception
        {
            public UserNotFoundException(string message) : base(message)
            {
                // Optionally, you can add custom logic here
            }
        }
    }
}
