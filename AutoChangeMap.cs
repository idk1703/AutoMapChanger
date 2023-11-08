using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Commands;

namespace AutoMapChanger;

public class AutoMapChanger : BasePlugin
{
    public override string ModuleName => "Auto Map Changer";
    public override string ModuleVersion => "1.1.2.1"; 
    public override string ModuleAuthor => "skaen";
    public override string ModuleDescription => "Changes the map to default when not active";
    private static Config? _config = null;
    private static int iChangeTime = 0;
    public override void Load(bool hotReload)
    {
        LoadConfig();

        Log($"[{ModuleVersion}] loaded success");

        RegisterListener<Listeners.OnMapStart>(mapName => {
            Log($"Map {mapName} has started!");
            if(mapName != _config!.DefaultMap && mapName != _config.DefaultMap[3..])
            {
                iChangeTime = 0;
                AddTimer(60.0f, MapChange, TimerFlags.REPEAT|TimerFlags.STOP_ON_MAPCHANGE);
            }
        });
        RegisterListener<Listeners.OnMapEnd>(() => {
            Log($"Map has ended.");
        });
    }

    private void MapChange()
    {
        iChangeTime = Utilities.GetPlayers().Where(p => p.IsBot == false).Any() ? 0 : iChangeTime + 1;
        if(iChangeTime < _config!.Delay) return;

        if(_config.DefaultMap.IndexOf("ws:") != -1)
        {
            Server.ExecuteCommand($"ds_workshop_changelevel {_config.DefaultMap[3..]}");
        }
        else
        {
            Server.ExecuteCommand($"changelevel {_config.DefaultMap}");
        }
        
        Log($"Change level on map \"{_config.DefaultMap}\"");
    }

    [ConsoleCommand("css_acm_reload", "Reload config AutoChangeMap")]
    public void ReloadConfig(CCSPlayerController? controller, CommandInfo command)
    {
        if(controller != null) return;

        LoadConfig();
        iChangeTime = 0;

        Log($"[{ModuleVersion}] loaded config success");
    }

    private void LoadConfig()
    {
        var configPath = Path.Combine(ModuleDirectory, "autochangemap.json");
        if(!File.Exists(configPath)) 
        {
            CreateConfig(configPath);
        }
        else
        {
            _config = JsonSerializer.Deserialize<Config>(File.ReadAllText(configPath))!;
        }
    }

    private void CreateConfig(string configPath)
    {
        _config = new Config
        {
            Delay = 15,
            DefaultMap = "de_dust2",
        };

        File.WriteAllText(configPath, JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true }));

        Log($"The configuration was successfully saved to a file: {configPath}");
    }

    public void Log(string message)
    {
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.WriteLine($"[{ModuleName}] {message}");
        Console.ResetColor();
    }
}

public class Config
{
    public int Delay { get; set; }
    public string DefaultMap { get; set; } = null!;
}