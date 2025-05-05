using Domain.Core.Primitives;

namespace Domain.OrderAggregate;

public static class OrderErrors
{
    public static class Create
    {
        public static Error OrderAlreadyExists = new(
            "OrderErrors.Create",
            "Pending order already exists.");
    }

    public static class Update
    {
        public static class OrderItem
        {
            public static Error NotEnoughTicketsAvailable = new(
                "OrderErrors.Update.OrderItem",
                "Not enough tickets available.");

            public static Error CurrencyMismatch = new(
                "OrderErrors.Update.OrderItem",
                "Currency mismatch.");

            public static Error OrderItemNotFound = new(
                "OrderErrors.Update.OrderItem",
                "Order item not found.");

            public static Error OrderIsNotPending = new(
                "OrderErrors.Update.OrderItem",
                "Order is not pending.");

            public static Error OrderIsNotCancelled = new(
                "OrderErrors.Update.OrderItem",
                "Order is not cancelled.");

            public static Error TicketPoolNotFound = new(
                "OrderErrors.Update.OrderItem",
                "Ticket pool not found.");
        }
    }
}