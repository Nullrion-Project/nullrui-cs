using raygui;
using raygui.math;
using Raylib_cs;

public class Program
{
    public static void Main()
    {
        Raylib.SetConfigFlags(ConfigFlags.ResizableWindow);
        Raylib.InitWindow(1280, 720, "Demo Gui");
        Raylib.SetTargetFPS(120);

        // Instantiate raygui
        Frame newFrame = new()
        {
            Position = new(){X=0.5f, Y=0.5f},
            Size = new(){X=200, Y=200},
            Color = Color.Blue,
            Origin = new(){X=0.5f, Y=0.5f},

            RelativePosAxis = Axis.Both,
            RelativeOriginAxis = Axis.Both,
        };
        Buttton newButton = new()
        {
            Text = "My Button!",
            Position = new(){X=100f, Y=500f},
            Size = new(){X=200, Y=100},
            Color = Color.Blue,
            Origin = new(){X=0f, Y=0f},
            ScaleText = true,
            OnHoverEnter = (btn) => btn.Text = "Youre Hovering!",
            OnHoverLeave = (btn) => btn.Text = "Youre Leaving!"
        };
        // End of raygui Demo

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.White);
                
                newFrame.DrawElement();
                newButton.DrawElement();
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}