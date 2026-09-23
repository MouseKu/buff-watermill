# Buff Watermill

A lightweight RimWorld 1.6 mod for adjusting watermill generator output. It does not require Harmony or any other mods.

## Features

- Default output multiplier: 1.5x (1100W → 1650W)
- Adjustable from 0.5x to 5.0x in increments of 0.1
- `Default` resets the slider to 1.5x; `Apply` immediately applies and saves the value

## Build

If RimWorld is installed at the project's default path (`D:\SteamLibrary\steamapps\common\RimWorld`):

```powershell
dotnet build .\Source\BuffWatermill\BuffWatermill.csproj -c Release
```

For another location, add `-p:RimWorldDir="path to RimWorld"`. The DLL is generated in `dist/BuffWatermill/1.6/Assemblies`.
