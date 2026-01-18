## Skill: API Controller Pattern

- Place new API controllers in Controllers/WebApi/ or Controllers/WebTokenApi/.
- Inherit from BaseApiController (WebApi) or BaseTokenAPiController (WebTokenApi).
- Register routes using attribute routing (e.g., [Route("api/[controller]")]).
- Use dependency injection for services and DB contexts.
- Example: See Controllers/WebApi/UserController.cs
- Keep business logic out of controllers; use Helper/ or Tools/ for shared logic.
