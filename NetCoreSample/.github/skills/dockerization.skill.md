## Skill: Dockerization & Clean Build

- Clean .NET and frontend artifacts before building images: dotnet clean, remove node_modules, wwwroot/dist.
- Use .dockerignore to exclude bin/, obj/, node_modules/, out/, etc.
- Build image: podman build -t netcoresample:latest . (or docker build ...)
- Run: podman run -d -p 5000:5000 --name netcoresample netcoresample:latest
- Only wwwroot/dist is included in publish output for static assets.
- Example .dockerignore is in README.md.
