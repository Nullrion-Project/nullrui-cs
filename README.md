# Nullrui-cs
A Drop-in user-interface library that work with existing raylib-cs codebase, it leverages native raylib-cs bindings and wrap it with a straightforward and instantiable "Elements"

## - Code Example
This wrapper is taking reference from some popular user-interface library such as [osu-framework](https://github.com/ppy/osu-framework) and roblox [roact](https://github.com/roblox/roact) and [Wind-UI](https://github.com/Footagesus/WindUI), it's straightforward.

```csharp
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
```

## - How To Use?
To use this module, simply go to [src/](https://github.com/Aethertenshi/nullrui-cs/tree/master/src) and download the one file [main-ui.cs](https://github.com/Aethertenshi/nullrui-cs/blob/master/src/main-ui.cs), Thats it!

## - Documentation
Currently, we dont have dedicated external documentation, you can see the example program [here](https://github.com/Aethertenshi/nullrui-cs/blob/master/Program.cs) and for more thorough documentation, you can always read the source code.

## - Notice
This project is built without using any generative-ai in some sort of way, this README.md document is written by a human with limited knowledge of english.

## - License
This project is licensed under MIT, you can freely use, modify, and distribute on any projects both commercial and non-commercial project. Credit isnt mandatory but appreciated!