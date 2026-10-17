# Running E2E tests locally

From the `src/Bison.Razor` project folder, start the server with the E2E launch profile:

```sh
dotnet run --launch-profile E2E
```

For manual browser testing, install the Playwright browsers after building the solution. Run the platform-specific command from the repository root:

**Windows**

```powershell
pwsh test/Bison.Tests/bin/Debug/net8.0/playwright.ps1 install
```

**Linux**

```bash
pwsh test/Bison.Tests/bin/Debug/net8.0/playwright.ps1 install --with-deps
```

**macOS**

```bash
pwsh test/Bison.Tests/bin/Debug/net8.0/playwright.ps1 install
```
