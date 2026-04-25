
# AniList Clone API

A backend API built with ASP.NET Core that fetches anime data from the AniList GraphQL API. It includes caching for performance and exposes endpoints for a simple frontend that displays anime data. The project follows a layered architecture with services, caching, and clean separation of concerns.

---

## How it Works

The backend acts as a wrapper around the AniList GraphQL API.

* Fetches anime by ID, search term, or trending list
* Uses in-memory caching to reduce repeated API calls
* Separates logic into controller, service, and caching layers
* Handles API errors and invalid requests with proper responses

The frontend is a simple JavaScript app that consumes the API and renders anime data dynamically.

* Displays trending anime with pagination
* Supports search functionality
* Shows detailed anime pages using query parameters

---

## Architecture

```
Controller → CachingService → MediaService → AniList API
                     ↓
               Memory Cache
```

---

## Technologies

* ASP.NET Core Web API
* C#
* GraphQL (AniList API)
* In-memory caching (IMemoryCache)
* Vanilla JavaScript frontend

---

## Testing

The project includes unit and integration tests.

### Unit Tests

* Tests caching behavior
* Tests controller responses
* Uses mocks to isolate logic

### Integration Tests

* Tests full API flow
* Uses WireMock to simulate AniList API
* Verifies error handling and real HTTP behavior

---

## Frontend

Simple vanilla JavaScript frontend that:

* Displays anime list (trending)
* Supports search
* Shows anime details page
* Uses fetch API to communicate with backend

---

## Running the Project

```bash
dotnet run
```

Then open the frontend HTML files in a browser.

---

## Future Improvements

* Add Authentication + Authorization (JWT + Roles)
* Add Docker + Deployment (with CI/CD pipeline)
* Rate limiting + API protection layer


---

## Author

Full-stack learning project focused on API design, caching, and real-world backend architecture.
