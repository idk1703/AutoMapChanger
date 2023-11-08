# Auto Map Changer
Changes the map to default, when not active
### Installation
1. Install [Metamod:Source](https://www.sourcemm.net/downloads.php/?branch=master)
2. Install [CounterStrike Sharp](https://github.com/roflmuffin/CounterStrikeSharp) 
3. Build `Auto Map Changer`
4. Move dir `AutoChangeMap` in `counterstrikesharp/plugins`
### Build
```
dotnet publish -c release -r linux-x64 --no-self-contained -p:DebugSymbols=false -p:DebugType=None -o AutoChangeMap
```
> [!NOTE]
> Needs `CounterStrikeSharp.API.dll` in parent dir
### Config
The config is created automatically in the same place where the dll is located
```
{
  "Delay": 15,
  "DefaultMap": "de_dust2"
}
```
> [!NOTE]
> For Workshop maps needs prefix `ws:`
> Delay in minutes, after this time map can change if server don't have players
### Commands
`css_acm_reload` - Reload config AutoChangeMap