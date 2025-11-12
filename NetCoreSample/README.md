# NetCoreSample 專案說明

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
