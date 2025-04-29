namespace EventBookingSystem.Constants
{
    public static class ApiRoutes
    {
        private const string Base = "api";

        public static class Events
        {
            public const string Root = $"{Base}/events";
            public const string GetAll = "";
            public const string GetById = "{id}";
            public const string Add = "";
            public const string Update = "{id}";
            public const string Delete = "{id}";
        }

        public static class Bookings
        {
            public const string Root = $"{Base}/booking";
            public const string Book = "{eventId}";
            public const string Cancel = "cancel";
        }
    }
}



