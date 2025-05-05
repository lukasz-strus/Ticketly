# Projekt Ticketly

## 🎯 Cel projektu
Celem projektu było stworzenie intuicyjnej i bezpiecznej witryny internetowej umożliwiającej użytkownikom zakup biletów na wydarzenia kulturalne, z pełną obsługą ról (administrator, użytkownik zalogowany i niezalogowany), zarządzaniem pulami biletów i historią zakupów.

## 🛠️ Technologie użyte w projekcie

### 🔙 Backend

- **ASP.NET Core 9** – framework do budowy nowoczesnych aplikacji webowych.
- **Entity Framework Core 9** – ORM do pracy z bazą danych w podejściu code-first.
- **ASP.NET Identity** – system zarządzania użytkownikami, rejestracją i logowaniem.
- **MediatR** – implementacja wzorca Mediator do luźno powiązanej komunikacji między warstwami.
- **FluentValidation** – biblioteka do walidacji danych wejściowych.
- **Swagger (Swashbuckle)** – dokumentacja i testowanie REST API.
- **xUnit + FluentAssertions** – frameworki do testów jednostkowych z czytelnymi asercjami.
- **Autoryzacja**: Implementacja oparta o tokeny JWT, umożliwiająca bezpieczną wymianę danych i autoryzację użytkowników.
- **Architektura**: projekt został zbudowany w oparciu o wzorce **Clean Architecture** oraz **Domain-Driven Design (DDD)**, co zapewnia modularność, czytelność i łatwą możliwość rozbudowy.

### 🔜 Frontend

- **React v19** – biblioteka JavaScript do budowy interfejsów użytkownika.
- **TypeScript** – nadzbiór JavaScript z systemem statycznych typów.
- **React Bootstrap** – komponenty interfejsu użytkownika oparte na Bootstrapie.
- **Axios** – biblioteka do obsługi zapytań HTTP i komunikacji z API.
- **Architektura**: aplikacja frontendowa została zaprojektowana zgodnie z wzorcem **MVVM (Model-View-ViewModel)**, co ułatwia oddzielenie logiki prezentacji od warstwy danych.
