Guitar Transposition Tool
=========================

This is a Windows Forms app targeting .NET 10 (Windows). It requires Windows to run the GUI.

Running locally on Windows (recommended)
- Install .NET 10 SDK on Windows (or Visual Studio with .NET 10 workloads).
- Clone this repo and open it in VS Code (install C# extension for best experience).
- From a terminal in the repo root run:

```bash
dotnet run --project GuitarTranspositionTool/GuitarTranspositionTool.csproj
```

This will run the app on your Windows machine.

Cross-publish a Windows exe from Linux/macOS
- Use the provided script to produce a self-contained Windows publish and zip it:

```bash
./scripts/publish-windows.sh [rid] [self-contained:true|false] [single-file:true|false]
# Example (default):
./scripts/publish-windows.sh
# Single-file self-contained example:
./scripts/publish-windows.sh win-x64 true true
```

- The script publishes to `publish/<rid>/` and creates `publish/GuitarTranspositionTool-<rid>.zip`.
- Copy the zip or the folder to a Windows machine and run `GuitarTranspositionTool.exe`.

Notes
- The project file sets `UseWindowsForms` and `TargetFramework` to a Windows TFM, so running `dotnet run` on Linux will fail.
- For debugging and development, run on Windows (Visual Studio or `dotnet run`).
