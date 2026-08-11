# NetCoreSample 專案說明

---

## .github 資料夾結構與說明

- **agents/**
  - `api-design-expert.agent.md`：API 設計與文件產生專家代理人
  - `code-review-expert.agent.md`：自動偵測程式變更並依技能/指引進行 code review 的專家代理人
  - `csharp-expert.agent.md`：.NET/C# 專家，協助開發、審查、除錯
  - `doc-generator-expert.agent.md`：自動產生/維護專案文件與 .github 結構摘要的專家代理人
  - `meta-agentic-project-scaffold.agent.md`：協助專案流程與工作流建立的 meta agent
  - `test-expert.agent.md`：測試策略、單元/整合/自動化測試專家代理人

- **instructions/**
  - `code-review.instructions.md`：專案 code review 檢查重點與結構規範
  - `containerization-docker-best-practices.instructions.md`：Docker/容器化最佳實踐與審查重點
  - `csharp-development.instructions.md`：C# 開發規範與審查重點
  - `doc-structure-collector.instructions.md`：自動產生/更新 .github 結構說明的指引
  - `performance-optimization-best-practices.instructions.md`：效能優化最佳實踐與審查重點
  - `secure-coding-owasp.instructions.md`：安全開發與 OWASP 檢查重點

- **prompts/**
  - `code-review.prompt.md`：Code review 步驟與審查提示
  - `create-implementation-plan.prompt.md`：產生實作計畫的提示
  - `doc-structure-collector.prompt.md`：自動產生/更新 .github 結構說明的提示
  - `dotnet-csharp-best-practices.prompt.md`：.NET/C# 最佳實踐審查提示
  - `project-workflow-documentation-generator.prompt.md`：產生專案流程與架構文件的提示
  - `readme-generator.prompt.md`：產生/更新 README.md 的提示
  - `test-strategy.prompt.md`：測試策略撰寫提示

- **skills/**
  - `api-controller.skill.md`：API Controller 分層與設計技能
  - `api-design-best-practices.skill.md`：API 設計最佳實踐技能
  - `code-review-checklist.skill.md`：Code review 檢查清單技能
  - `dbcontext-multi.skill.md`：多 DbContext 設計技能
  - `doc-structure-collector.skill.md`：自動產生/更新 .github 結構說明技能
  - `dockerization.skill.md`：Docker 化與乾淨建置技能
  - `frontend-assets.skill.md`：前端資產管理與建置技能

- `copilot-instructions.md`：Copilot 與自動化代理人整合說明

---

## 前端編譯/資源流程

1. 安裝依賴（只需一次）
   ```powershell
   npm install
   ```
2. 編譯 TypeScript（JS）
   - 一次性編譯：
     ```powershell
     npx tsc
     ```
   - 持續監控編譯：
     ```powershell
     npx tsc --watch
     ```
3. 編譯 SCSS（CSS）
   - 一次性編譯：
     ```powershell
     npx sass wwwroot/css:wwwroot/css
     ```
   - 持續監控編譯：
     ```powershell
     npx sass --watch wwwroot/css:wwwroot/css
     ```
4. 複製第三方 JS/CSS 到 wwwroot/dist（每次有第三方套件更新時執行）

   ```powershell
   npm run build
   ```

   - 只會將 node_modules 內指定的 JS/CSS 複製到 wwwroot/dist，不會打包 JS/CSS。

5. .cshtml 檔案引用方式：
   ```html
   <script src="~/dist/js/semantic.min.js"></script>
   <link rel="stylesheet" href="~/dist/css/list.min.css" />
   <script src="~/dist/js/simplemde.min.js"></script>
   <script src="~/js/about.js"></script>
   <link rel="stylesheet" href="~/css/about.css" />
   ```

> [!TIP]
> JS 只用 tsc 編譯，CSS 只用 sass 編譯，Webpack 僅負責複製第三方套件到 wwwroot/dist，不做任何打包。
> .cshtml 檔案只能引用 wwwroot/dist、wwwroot/js、wwwroot/css，不可直接引用 node_modules。

---

## 後端編譯流程

- 標準 .NET Core 專案編譯：
  ```powershell
  dotnet build
  dotnet publish -c Release -o out
  ```
- 編譯時只會複製 `wwwroot/dist` 內的前端資源，避免 node_modules 影響編譯速度。

---

## 建立 Docker Image 前的清理步驟

1. 清理 .NET Core 暫存檔案：
   ```powershell
   dotnet clean
   ```
2. 清理前端暫存檔案（建議）：
   ```powershell
   rd /s /q node_modules
   rd /s /q wwwroot\dist
   del package-lock.json
   npm install
   npx tsc
   npx sass wwwroot/css:wwwroot/css
   npm run build
   ```
3. 建議設定 `.dockerignore`，排除 bin/、obj/、node_modules/ 等目錄。

---

## Docker/WSL 容器化執行

1. 建立 Docker/Podman 映像檔（於 WSL 目錄下）：
   ```bash
   podman build -t netcoresample:latest .
   ```
2. 執行容器：
   ```bash
   podman run -d -p 5000:5000 --name netcoresample netcoresample:latest
   ```
3. Windows 本機存取：
   - 直接瀏覽 `http://localhost:5000`
   - 若無法連線，檢查 WSL2 port 轉發或用 WSL IP 存取
4. 停止與移除容器：
   ```bash
   podman stop netcoresample
   podman rm netcoresample
   ```
5. 查看容器 log：
   ```bash
   podman logs netcoresample
   ```

### 注意事項

- WSL2 預設自動將容器 port 轉發到 Windows，可直接用 `localhost` 存取。
- Podman 與 Docker 指令基本相同，若偏好 Docker 可將 `podman` 換成 `docker`。

---

### .dockerignore 範例

建議在專案根目錄建立 `.dockerignore`，內容如下：

```
bin/
obj/
node_modules/
out/
*.log
*.md
.git/
.vscode/
package-lock.json
```

這樣可避免 Docker build 時複製多餘檔案。
