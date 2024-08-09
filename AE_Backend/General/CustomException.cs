namespace AE_Backend.General
{
    public class CustomException
    {
        public class UserNotFoundException : Exception
        {
            public UserNotFoundException(string message) : base(message)
            {
            }
        }

        public class RoleNotFoundException : Exception
        {
            public RoleNotFoundException(string message) : base(message)
            {
            }
        }

        public class ShipNotFoundException : Exception
        {
            public ShipNotFoundException(string message) : base(message)
            {
            }
        }

        public class PortNotFoundException : Exception
        {
            public PortNotFoundException(string message) : base(message)
            {
            }
        }

        public class UserRoleNotFoundException : Exception
        {
            public UserRoleNotFoundException(string message) : base(message)
            {
            }
        }

        public class UserShipNotFoundException : Exception
        {
            public UserShipNotFoundException(string message) : base(message)
            {
            }
        }
    }
}
