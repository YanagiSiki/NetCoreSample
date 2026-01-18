## Skill: Frontend Asset Workflow

- Use npm to manage dependencies (npm install).
- Compile TypeScript: npx tsc (or npx tsc --watch).
- Compile SCSS: npx sass wwwroot/css:wwwroot/css (or --watch).
- Copy third-party JS/CSS: npm run build (see webpack.config.js).
- Only reference assets from wwwroot/dist, wwwroot/js, wwwroot/css in .cshtml files.
- Do not reference node_modules directly in Razor views.
- Example: See Views/Home/_.cshtml and wwwroot/js/_.ts
