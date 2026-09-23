# Repository Guidelines

## Project Structure & Module Organization

This repository contains a lightweight RimWorld 1.6 mod. Production C# code lives in `Source/BuffWatermill/`; `BuffWatermillMod.cs` contains the settings UI, persistence, and watermill power adjustment logic. `BuffWatermill.csproj` defines the .NET Framework 4.7.2 build and references assemblies from a local RimWorld installation. Mod metadata is stored in `About/About.xml`. Release binaries are emitted to `1.6/Assemblies/`. Treat `Source/**/bin/` and `Source/**/obj/` as generated output and do not commit them.

## Build, Test, and Development Commands

Run commands from the repository root:

```powershell
dotnet build .\Source\BuffWatermill\BuffWatermill.csproj -c Release
dotnet build .\Source\BuffWatermill\BuffWatermill.csproj -c Release -p:RimWorldDir="C:\Games\RimWorld"
dotnet clean .\Source\BuffWatermill\BuffWatermill.csproj
```

The first command uses the default RimWorld path configured in the project. Use the second form when RimWorld is installed elsewhere. A successful release build places `BuffWatermill.dll` in `1.6/Assemblies/`.

## Coding Style & Naming Conventions

Follow the existing C# 7.3 style: four-space indentation, braces on separate lines, and one type per logical responsibility. Use `PascalCase` for types, methods, and constants; use `camelCase` for fields, parameters, and locals. Keep the `BuffWatermill` namespace. Prefer explicit, readable RimWorld API calls and guard missing definitions or reflected fields with clear `[Buff Watermill]` log messages. No formatter or linter is configured, so match nearby code.

## Testing Guidelines

There is currently no automated test project. Before submitting changes, build in `Release`, launch RimWorld 1.6 with only this mod enabled, and verify that the settings slider, `Default`, `Apply`, persistence after restart, and watermill output all behave correctly. Check the RimWorld log for errors. If tests are added, place them under `Source/BuffWatermill.Tests/` and name files `*Tests.cs`.

## Commit & Pull Request Guidelines

Repository history is not available in this checkout, so use short, imperative commit subjects such as `Fix multiplier persistence`. Keep commits focused. Pull requests should explain the behavior change, list manual test steps, and note the RimWorld version tested. Link relevant issues and include screenshots for settings UI changes. Do not include generated `bin/` or `obj/` files; include updated release assemblies only when the project's release workflow requires them.
