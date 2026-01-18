# Copilot Instructions for NetCoreSample

### 典型工作流程範例：

1. 新增功能/修正：

- 依 instructions/ 與 skills/ 撰寫程式
- 參考 prompts/ 產生設計/測試/文件
- PR 時依 PULL_REQUEST_TEMPLATE 填寫

2. Code Review：

- 依 code-review.prompt.md 啟動審查
- 依 code-review-checklist.skill.md 檢查

3. 測試/CI/CD：

- 參考 test-strategy.prompt.md 與 test-expert.agent.md

4. API 設計：

- 參考 api-design-best-practices.skill.md 與 api-design-expert.agent.md

5. 文件產生：

- 參考 doc-generator-expert.agent.md

---

## Project Overview

- **Type:** ASP.NET Core MVC + TypeScript/SCSS frontend
- **Structure:**
  - `Controllers/` — API and MVC controllers (WebApi, WebTokenApi subfolders)
  - `Models/` — Entity models (User, Post, Tag, etc.)
  - `DBContext/` — Multiple DB context classes for different databases
  - `Views/` — Razor views, organized by feature
  - `wwwroot/` — Static assets (js, css, images)
  - `Helper/`, `Tools/` — Utility and extension classes

## Build & Development Workflow

- **Frontend:**
  - Use `npm install` once for dependencies
  - Compile TypeScript: `npx tsc` (or `npx tsc --watch`)
  - Compile SCSS: `npx sass wwwroot/css:wwwroot/css` (or `npx sass --watch ...`)
  - Copy third-party assets: `npm run build` (see `webpack.config.js`)
  - Only reference assets from `wwwroot/dist`, `wwwroot/js`, `wwwroot/css` in `.cshtml` files
- **Backend:**
  - Standard .NET Core build: `dotnet build`
  - Publish: `dotnet publish -c Release -o out`
  - Only `wwwroot/dist` is included in publish output
- **Docker:**
  - Clean with `dotnet clean` and remove `node_modules`, `wwwroot/dist` before build
  - Use `.dockerignore` to exclude `bin/`, `obj/`, `node_modules/`, etc.

## Key Conventions & Patterns

- **No direct node_modules references** in Razor views; always copy to `wwwroot/dist`.
- **Controllers** are grouped by API type (WebApi, WebTokenApi) and inherit from base controllers.
- **Multiple DBContext** classes for different database backends; see `DBContext/`.
- **Helpers/Tools**: Common logic is factored into `Helper/` and `Tools/`.
- **Frontend assets** are not bundled/minified by Webpack—only copied.

## Integration & External Dependencies

- Uses Npgsql, MySQL, Oracle, and other DB providers (see `DBContext/`)
- Third-party JS/CSS managed via npm and copied to `wwwroot/dist`
- Docker/Podman supported for containerization

## Examples

- To add a new API: create a controller in `Controllers/WebApi/`, inherit from `BaseApiController`, and register routes.
- To add a new page: add a Razor view in `Views/Home/`, corresponding controller action in `Controllers/HomeController.cs`, and JS/SCSS in `wwwroot/js`/`wwwroot/css`.

## References

- See `README.md` for full build, clean, and Docker instructions
- See `webpack.config.js` for asset copy logic
- See `Startup.cs` for service registration and middleware

---

For more details, always check the `README.md` and referenced config files. If a pattern is unclear, ask for clarification or check similar files for examples.
