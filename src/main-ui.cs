using raygui.math;
using Raylib_cs;
using System.Numerics;

namespace raygui
{
    // Retained Mode Gui (RMUI)
    public abstract class Element
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Color Color { get; set; } = Color.White;
        public float Rotation { get; set; } = 0f;

        public Axis RelativePosAxis { get; set; } = Axis.None;
        public Axis RelativeSizeAxis { get; set; } = Axis.None;
        public Axis RelativeOriginAxis { get; set; } = Axis.None;

        public List<Element> Children = [];

        protected abstract void UpdateElement(float dt);
        public abstract void DrawElement(float? relX = null, float? relY = null);
    }
    
    public class Button : Element
    {
        public Action? OnHover { get; set; }
        public Action<Button>? OnClick { get; set; }
        public Action<Button>? OnHoverEnter { get; set; }
        public Action<Button>? OnHoverLeave { get; set; }

        public string Text { get; set; } = string.Empty;
        public float FontSize { get; set; } = 14f;
        public bool ScaleText { get; set; } = false;
        
        private bool _isHover = false;
        private void CheckHover(ref Rectangle frameRect)
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            bool isLeftClicked = Raylib.IsMouseButtonPressed(MouseButton.Left);

            // Check Hover
            if (Raylib.CheckCollisionPointRec(mousePos, frameRect))
            {
                if (isLeftClicked) OnClick?.Invoke(this);
                if (!_isHover)
                {
                    _isHover = true;
                    OnHoverEnter?.Invoke(this);
                }
                OnHover?.Invoke();
            } else
            {
                ImGui.IsHoveringInterface = false;
                if (_isHover)
                {
                    _isHover = false;
                    OnHoverLeave?.Invoke(this);
                }
            }
        }

        protected override void UpdateElement(float dt){}
        public override void DrawElement(float? relX = null, float? relY = null)
        {
            Vector2 finalPosition = AxisCalculation.Calculate(Position, RelativePosAxis, relX?? Raylib.GetScreenWidth(), relY?? Raylib.GetScreenHeight());
            Vector2 finalOrigin = AxisCalculation.Calculate(Origin, RelativeOriginAxis, Size.X, Size.Y);
            Vector2 finalSize = AxisCalculation.Calculate(Size, RelativeSizeAxis, relX?? Raylib.GetScreenWidth(), relY?? Raylib.GetScreenHeight());
            
            Rectangle frameRect = new(finalPosition - finalOrigin, finalSize);
            CheckHover(ref frameRect);

            Raylib.DrawRectangleRec(frameRect, Color);

            // Draw Button Text
            Font currentFont = Raylib.GetFontDefault();
            float fontSize = ScaleText? frameRect.Height * 0.8f : FontSize;
            Vector2 measuredText = Raylib.MeasureTextEx(currentFont, Text, fontSize, 1);

            if (measuredText.X > frameRect.Width && ScaleText)
            {
                fontSize *= (frameRect.Width / measuredText.X) * 0.8f;
                measuredText = Raylib.MeasureTextEx(currentFont, Text, fontSize, 1);
            }
            int textX = (int)(frameRect.X + (frameRect.Width - measuredText.X) / 2);
            int textY = (int)(frameRect.Y + (frameRect.Height - fontSize) / 2);
            Raylib.DrawTextPro(currentFont, Text, new(textX, textY), Vector2.Zero, Rotation, fontSize, 1, Color.Black);

            for (int i=0; i < Children.Count; i++)
            {
                if (Children[i] is not null)
                {
                    Element element = Children[i];
                    Vector2 tempPos = element.Position;
                    element.Position = finalPosition - finalOrigin + element.Position;
                    element.DrawElement(finalSize.X, finalSize.Y);
                    element.Position = tempPos;
                }
            }
        }
    }
    public class Frame : Element
    {
        protected override void UpdateElement(float dt){}
        public override void DrawElement(float? relX = null, float? relY = null)
        {
            Vector2 finalPosition = AxisCalculation.Calculate(Position, RelativePosAxis, relX?? Raylib.GetScreenWidth(), relY?? Raylib.GetScreenHeight());
            Vector2 finalOrigin = AxisCalculation.Calculate(Origin, RelativeOriginAxis, Size.X, Size.Y);
            Vector2 finalSize = AxisCalculation.Calculate(Size, RelativeSizeAxis, relX?? Raylib.GetScreenWidth(), relY?? Raylib.GetScreenHeight());
            
            Rectangle frameRect = new(finalPosition - finalOrigin, finalSize);

            Raylib.DrawRectanglePro(frameRect, Vector2.Zero, Rotation, Color);

            for (int i=0; i < Children.Count; i++)
            {
                if (Children[i] is not null)
                {
                    Element element = Children[i];
                    Vector2 tempPos = element.Position;
                    element.Position = finalPosition - finalOrigin + element.Position;
                    element.DrawElement(finalSize.X, finalSize.Y);
                    element.Position = tempPos;
                }
            }
        }
    }
    public class ScrollingFrame : Element
    {
        protected override void UpdateElement(float dt){}
        public override void DrawElement(float? relX = null, float? relY = null)
        {
            // TODO: sumthing lol
        }
    }

    // Immediate Mode Gui (IMUI)
    public class ImGui
    {
        public static bool IsHoveringInterface { get; internal set; } = false;
        public static bool Button(Rectangle bounds, string text)
        {
            Vector2 mousePos = Raylib.GetMousePosition();
            bool hovered = Raylib.CheckCollisionPointRec(mousePos, bounds);
            bool clicked = false;

            // Visual feedback based on state
            Color btnColor = new(40, 40, 40);

            if (hovered)
            {
                if (Raylib.IsMouseButtonDown(MouseButton.Left))
                {
                    btnColor = new(125, 125, 125); // Active / Pressed visual
                }
                else
                {
                    btnColor = new(80, 80, 80); // Hovered visual
                }

                // Click activates on release or press (press is simplest for UI buttons)
                if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    clicked = true;
                }

                IsHoveringInterface = true;
            } else if (IsHoveringInterface && !hovered) IsHoveringInterface = false;

            // 1. Draw background
            Raylib.DrawRectangleRec(bounds, btnColor);

            // 2. Draw text centered
            int fontSize = 16;
            int textWidth = Raylib.MeasureText(text, fontSize);
            int textX = (int)(bounds.X + (bounds.Width - textWidth) / 2);
            int textY = (int)(bounds.Y + (bounds.Height - fontSize) / 2);
            Raylib.DrawText(text, textX, textY, fontSize, Color.White);

            // 3. Return outcome immediately
            return clicked;
        }
    }
}
namespace raygui.math
{
    public enum Axis
    {
        None,
        Horizontal,
        Vertical,
        Both,
    }
    public static class AxisCalculation
    {
        public static Vector2 Calculate(Vector2 v2d, Axis axis, float relX, float relY)
        {
            return axis switch
            {
                Axis.None => v2d,
                Axis.Horizontal => new(){X=v2d.X*relX, Y=v2d.Y},
                Axis.Vertical => new(){X=v2d.X, Y=v2d.Y*relY},
                Axis.Both => new(){X=v2d.X*relX, Y=v2d.Y*relY},
                _ => v2d,
            };
        }
    }

    public enum ListLayout
    {
        Horizontal,
        Vertical,
    }
}