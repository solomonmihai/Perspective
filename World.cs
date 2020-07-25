using Godot;
using System.Diagnostics;

public class World : Spatial
{
    public Node CurrentLevel { get; private set; }

    public Stopwatch LevelTimer { get; private set; }

    [Export]
    public PackedScene DebugLevel { get; set; }

    public override void _EnterTree()
    {
        if (DebugLevel != null)
        {
            ChangeLevel(DebugLevel);
        }
        else
        {
            ChangeLevel((ResourceLoader.Load("Levels/Level_test.tscn") as PackedScene));
        }
    }

    public override void _Ready()
    {
        GetNode<Player>("Player").Respawn += Restart;
    }

    void ChangeLevel(PackedScene lvl)
    {
        CurrentLevel = lvl.Instance();
        AddChild(CurrentLevel);

        var settings = CurrentLevel.GetNode<LevelSettings>("LevelSettings");
        SetLevelSettings(settings);

        LevelTimer = new Stopwatch();
    }

    void SetLevelSettings(LevelSettings settings)
    {
        var cam = GetNode<Camera>("Camera");
        cam.Environment.BackgroundColor = settings.EnvColor;
        cam.Environment.FogColor = settings.EnvColor;
        cam.Environment.AmbientLightColor = settings.AmbientColor;
        cam.Environment.GlowEnabled = settings.Glow;
        var light = GetNode<DirectionalLight>("DirectionalLight");
        light.LightColor = settings.LightColor;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton evt && evt.Pressed)
        {
            SetLevelSettings(CurrentLevel.GetNode<LevelSettings>("LevelSettings"));
        }
    }

    void Restart()
    {
        LevelTimer.Reset();
        LevelTimer.Start();
    }
}
