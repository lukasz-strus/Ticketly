using Domain.Core.Primitives;

namespace Domain.EventAggregate;

public static class EventErrors
{
    public static Error EventDateMustBeGreaterThanMaxTicketPoolSaleDate = new(
        "Event.Update",
        "Event date must be greater than max ticket pool sale date.");

    public static class TicketPools
    {
        public static Error TicketPoolStartSaleDateMustBeLowerThanEndSaleDate = new(
            "Event.Update.TicketPools",
            "Ticket pool start sale date must be lower than end sale date.");

        public static Error TicketPoolStartSaleDateMustBeLessThanEventDate = new(
            "Event.Update.TicketPools",
            "Ticket pool start sale date must be less than event date.");

        public static Error TicketPoolEndSaleDateMustBeLessThanEventDate = new(
            "Event.Update.TicketPools",
            "Ticket pool end sale date must be less than event date.");

        public static Error TicketPoolNotFound = new(
            "Event.Update.TicketPools",
            "Ticket pool not found.");

        public static Error NotEnoughTicketsAvailable = new(
            "Event.Update.TicketPool",
            "Not enough tickets available.");

        public static Error AvailableTicketsCountMustBeGreaterThan0 = new(
            "Event.Update.TicketPool",
            "Available tickets count must be greater than 0.");

        public static Error PriceMustBeGreaterThan0 = new(
            "Event.Update.TicketPool",
            "Price must be greater than 0.");
    }

    public static class Category
    {
        public static Error NameAlreadyExists = new(
            "Event.Category",
            "Category name already exists.");
    }
}