namespace Presentation.Contracts;

internal static class ApiRoutes
{
    internal static class Authentication
    {
        internal const string Register = "api/auth/register";
        internal const string Login = "api/auth/login";
    }

    internal static class User
    {
        internal const string Me = "api/users/me";
        internal const string GetOrders = "api/users/me/orders";
    }

    internal static class Event
    {
        internal const string Create = "api/events";
        internal const string Get = "api/events";
        internal const string GetByCategoryId = "api/categories/{id:guid}/events";
        internal const string GetById = "api/events/{id:guid}";
        internal const string Update = "api/events/{id:guid}";
        internal const string Delete = "api/events/{id:guid}";
        internal const string AddTicketPool = "api/events/{eventId:guid}/ticket-pool";
        internal const string UpdateTicketPool = "api/events/{eventId:guid}/ticket-pool/{ticketPoolId:guid}";
        internal const string DeleteTicketPool = "api/events/{eventId:guid}/ticket-pool/{ticketPoolId:guid}";
    }

    internal static class Category
    {
        internal const string Create = "api/categories";
        internal const string Get = "api/categories";
        internal const string GetById = "api/categories/{id:guid}";
        internal const string Delete = "api/categories/{id:guid}";
    }

    internal static class Enum
    {
        internal const string GetCurrencies = "api/currencies";
    }

    internal static class Order
    {
        internal const string Create = "api/orders";
        internal const string Get = "api/orders";
        internal const string GetPending = "api/orders/pending";
        internal const string CompleteOrder = "api/orders/complete/{id:guid}";
        internal const string CancelOrder = "api/orders/cancel/{id:guid}";
        internal const string OpenOrder = "api/orders/open/{id:guid}";
        internal const string AddOrderItem = "api/orders/{orderId:guid}/order-item";
        internal const string UpdateOrderItem = "api/orders/{orderId:guid}/order-item/{orderItemId:guid}";
        internal const string DeleteOrderItem = "api/orders/{orderId:guid}/order-item/{orderItemId:guid}";
    }
}