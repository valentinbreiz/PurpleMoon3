using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace PurpleMoon3
{
    public class TerminalManager : Service
    {
        public int Width, Height;
        public int CursorX, CursorY;
        public Color BackColor, ForeColor;
        public Font Font;

        public TerminalManager() : base("terminal", ServiceType.Utility)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
            Font = Fonts.Serif8x16;
            Width = Kernel.SVGA.Width / (Font.Width + Font.SpacingX);
            Height = Kernel.SVGA.Height / (Font.Height + Font.SpacingY);
            BackColor = Color.Black;
            ForeColor = Color.White;
            Clear();

            Kernel.ServiceMgr.Register(this);
            Kernel.ServiceMgr.Start(this);
        }

        public override void Start()
        {
            base.Start();
        }

        public override void Stop()
        {
            base.Stop();
        }

        public void Update(Color col)
        {
            int w = Font.Width + Font.SpacingX;
            int h = Font.Height + Font.SpacingY;
            Kernel.SVGA.DrawRect(1 + CursorX * w, 1 + CursorY * h, w - 2, h - 2, 1, col);
            Kernel.SVGA.SVGA.Update(0, 0, (uint)Kernel.SVGA.Width, (uint)Kernel.SVGA.Height);
        }

        public void NewLine()
        {
            Update(BackColor);
            CursorX = 0;
            CursorY++;
            if (CursorY >= Height)
            {
                Scroll();
            }
            Update(ForeColor);
        }

        public void Delete()
        {
            int w = Font.Width + Font.SpacingX;
            int h = Font.Height + Font.SpacingY;
            if (CursorX > 0)
            {
                Update(BackColor);
                CursorX--;
                Kernel.SVGA.DrawChar(CursorX * w, CursorY * h, ' ', ForeColor, BackColor, Font);
                Update(ForeColor);
            }
            else if (CursorY > 0)
            {
                Update(BackColor);
                CursorY--;
                CursorX = Width - 1;
                Kernel.SVGA.DrawChar(CursorX * w, CursorY * h, ' ', ForeColor, BackColor, Font);
                Update(ForeColor);
            }
        }

        public void Scroll()
        {
            Update(BackColor);
            int h = Font.Height + Font.SpacingY;
            Kernel.SVGA.Copy(0, h, 0, 0, Width * (Font.Width + Font.SpacingX), Kernel.SVGA.Height - h);
            for (int i = 0; i < Width; i++) { Kernel.SVGA.DrawChar(i * (Font.Width + Font.SpacingX), Kernel.SVGA.Height - h, ' ', ForeColor, BackColor, Font); }
            CursorX = 0;
            CursorY = Height - 1;
            Update(ForeColor);
        }

        public void Clear() { Clear(BackColor); }

        public void Clear(Color bg)
        {
            BackColor = bg;
            Kernel.SVGA.Clear(bg);
            CursorX = 0;
            CursorY = 0;
            Update(ForeColor);
        }

        public void WriteChar(char c) { WriteChar(c, ForeColor, BackColor); }

        public void WriteChar(char c, Color fg) { WriteChar(c, fg, BackColor); }

        public void WriteChar(char c, Color fg, Color bg)
        {
            if (c == '\n') { NewLine(); }
            else
            {
                Update(BackColor);
                Kernel.SVGA.DrawChar(CursorX * (Font.Width + Font.SpacingX), CursorY * (Font.Height + Font.SpacingY), c, fg, bg, Font);
                CursorX++;
                if (CursorX >= Width) { NewLine(); return; }
                Update(ForeColor);
            }
        }

        public void Write(string text) { Write(text, ForeColor, BackColor); }

        public void Write(string text, Color fg) { Write(text, fg, BackColor); }

        public void Write(string text, Color fg, Color bg)
        {
            for (int i = 0; i < text.Length; i++)
            {
                WriteChar(text[i], fg, bg);
            }
        }

        public void WriteLine(string text) { Write(text); NewLine(); }
        public void WriteLine(string text, Color fg) { Write(text, fg); NewLine(); }
        public void WriteLine(string text, Color fg, Color bg) { Write(text, fg, bg); NewLine(); }

        public ConsoleKeyInfo ReadKey() { return Console.ReadKey(true); }
        public string ReadLine() { return Console.ReadLine(); }
    }
}
