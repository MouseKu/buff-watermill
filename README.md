# Buff Watermill

A lightweight RimWorld 1.6 mod that lets you adjust the power output of watermill generators. It does not require Harmony or any other mods.

## Features

- Uses a 1.5x output multiplier by default (1100 W → 1650 W)
- Supports multipliers from 0.5x to 5.0x in increments of 0.1
- Provides `Default` and `Apply` buttons in the mod settings
- Applies the saved multiplier when RimWorld starts
- Saves changes immediately when `Apply` is selected

## Requirements

- RimWorld 1.6
- .NET SDK capable of building .NET Framework 4.7.2 projects
- A local RimWorld installation containing the required managed assemblies

## Build

Run the following command from the repository root to create a complete mod package:

```powershell
.\scripts\package.ps1
```

The project uses `D:\SteamLibrary\steamapps\common\RimWorld` as the default RimWorld installation path. To use a different location:

```powershell
.\scripts\package.ps1 -RimWorldDir "C:\Games\RimWorld"
```

The package is generated under `dist/BuffWatermill/` with the following structure:

```text
BuffWatermill/
├── About/
│   ├── About.xml
│   └── Preview.png
└── 1.6/
    └── Assemblies/
        └── BuffWatermill.dll
```

The packaging script copies `scripts/About.xml` and converts `scripts/thumnails.jpg` to the RimWorld preview image at `About/Preview.png`.

To build only the DLL:

```powershell
dotnet build .\Source\BuffWatermill\BuffWatermill.csproj -c Release
```

For a RimWorld installation at another location:

```powershell
dotnet build .\Source\BuffWatermill\BuffWatermill.csproj -c Release -p:RimWorldDir="C:\Games\RimWorld"
```

The DLL is written to `dist/BuffWatermill/1.6/Assemblies/`.

## Installation

Copy the generated `dist/BuffWatermill` directory into RimWorld's `Mods` directory, then enable **Buff Watermill** in the in-game mod list.
