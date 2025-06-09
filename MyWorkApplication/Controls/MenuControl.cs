using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace MyWorkApplication.Classes
{
    internal class MenuControl
    {
        private static readonly int m_iImageColumnWidth = 24;
        private static readonly int m_iExtraWidth = 15;

        private Bitmap m_pMemBitmap; // = new Bitmap(panel1.Width, panel1.Height, PixelFormat.Format32bppArgb);
        private Graphics m_pMemGraphics;
        private readonly List<MenuCommand> m_pMenuCommands = new List<MenuCommand>();

        private MenuCommand m_pTracMenuItem;

        public int Width => m_pMemBitmap.Width;
        public int Height => m_pMemBitmap.Height;

        public bool Done => m_pTracMenuItem != null && m_pTracMenuItem.Done;

        public int HitIndex => m_pMenuCommands.IndexOf(m_pTracMenuItem);

        public bool ChangeChecked(int iIndex, Graphics g)
        {
            m_pMenuCommands[iIndex].Checked = !m_pMenuCommands[iIndex].Checked;
            Draw(g);
            return m_pMenuCommands[iIndex].Checked;
        }

        public void Add(string csText, bool bChecked)
        {
            m_pMenuCommands.Add(new MenuCommand(csText, bChecked));
        }

        public void Prepare(Graphics g)
        {
            m_pMenuCommands.Add(new MenuCommand("-"));
            var pDone = new MenuCommand("Done", false, true);
            m_pMenuCommands.Add(pDone);

            var iHeight = 4; //(2 + 2 top + bottom);
            float fWidth = 0;
            foreach (var pMenuCommand in m_pMenuCommands)
            {
                iHeight += pMenuCommand.Height;
                var pSizeF = g.MeasureString(pMenuCommand.Text, SystemInformation.MenuFont);
                fWidth = Math.Max(fWidth, pSizeF.Width);
            }

            var iWidth = (int)fWidth + m_iImageColumnWidth + m_iExtraWidth;

            m_pMemBitmap = new Bitmap(iWidth, iHeight);
            m_pMemGraphics = Graphics.FromImage(m_pMemBitmap);

        }

        private MenuCommand HitTest(int X, int Y)
        {
            if (X < 0 || X > Width || Y < 0 || Y > Height) return null;

            var iHeight = 2;
            foreach (var pMenuCommand in m_pMenuCommands)
            {
                if (Y > iHeight && Y < iHeight + pMenuCommand.Height)
                    return pMenuCommand.Separator ? null : pMenuCommand;
                iHeight += pMenuCommand.Height;
            }

            return null;
        }

        public bool HitTestMouseMove(int X, int Y)
        {
            var pMenuCommand = HitTest(X, Y);
            if (pMenuCommand != m_pTracMenuItem)
            {
                m_pTracMenuItem = pMenuCommand;
                return true;
            }

            return false;
        }

        public bool HitTestMouseDown(int X, int Y)
        {
            var pMenuCommand = HitTest(X, Y);
            return pMenuCommand != null;
        }

        public void Draw(Graphics g)
        {
            var area = new Rectangle(0, 0, m_pMemBitmap.Width, m_pMemBitmap.Height);

            m_pMemGraphics.Clear(SystemColors.Control);

            // Draw the background area
            DrawBackground(m_pMemGraphics, area);

            // Draw the actual menu items
            DrawAllCommands(m_pMemGraphics);

            g.DrawImage(m_pMemBitmap, area, area, GraphicsUnit.Pixel);
        }

        private void DrawBackground(Graphics g, Rectangle rectWin)
        {
            var main = new Rectangle(0, 0, rectWin.Width, rectWin.Height);

            var xStart = 1;
            var yStart = 2;
            var yHeight = main.Height - yStart - 1;

            // Paint the main area background
            using (Brush backBrush = new SolidBrush(Color.FromArgb(249, 248, 247)))
            {
                g.FillRectangle(backBrush, main);
            }

            // Draw single line border around the main area
            using (var mainBorder = new Pen(Color.FromArgb(102, 102, 102)))
            {
                g.DrawRectangle(mainBorder, main);
            }

            var imageRect = new Rectangle(xStart, yStart, m_iImageColumnWidth, yHeight);

            // Draw the first image column
            using (Brush openBrush = new LinearGradientBrush(imageRect, Color.FromArgb(248, 247, 246),
                Color.FromArgb(215, 211, 204), 0f))
            {
                g.FillRectangle(openBrush, imageRect);
            }

            // Draw shadow around borders
            var rightLeft = main.Right + 1;
            var rightTop = main.Top + 4;
            var rightBottom = main.Bottom + 1;
            var leftLeft = main.Left + 4;
            var xExcludeStart = main.Left;
            var xExcludeEnd = main.Left;
        }

        private void DrawAllCommands(Graphics g)
        {
            var iTop = 2;
            foreach (var pMenuCommand in m_pMenuCommands)
                DrawSingleCommand(g, ref iTop, pMenuCommand, pMenuCommand == m_pTracMenuItem);
        }

        private void DrawSingleCommand(Graphics g, ref int iTop, MenuCommand pMenuCommand, bool hotCommand)
        {
            var iHeight = pMenuCommand.Height;
            var drawRect = new Rectangle(1, iTop, Width, iHeight);
            iTop += iHeight;

            // Remember some often used values
            var textGapLeft = 4;
            var imageLeft = 4;

            // Calculate some common values
            var imageColWidth = 24;

            // Is this item a separator?
            if (pMenuCommand.Separator)
            {
                // Draw the image column background
                var imageCol = new Rectangle(drawRect.Left, drawRect.Top, imageColWidth, drawRect.Height);

                // Draw the image column
                using (Brush openBrush = new LinearGradientBrush(imageCol, Color.FromArgb(248, 247, 246),
                    Color.FromArgb(215, 211, 204), 0f))
                {
                    g.FillRectangle(openBrush, imageCol);
                }

                // Draw a separator
                using (var separatorPen = new Pen(Color.FromArgb(166, 166, 166)))
                {
                    // Draw the separator as a single line
                    g.DrawLine(separatorPen,
                        drawRect.Left + imageColWidth + textGapLeft, drawRect.Top + 2,
                        drawRect.Right - 7,
                        drawRect.Top + 2);
                }
            }
            else
            {
                var leftPos = drawRect.Left + imageColWidth + textGapLeft;

                // Should the command be drawn selected?
                if (hotCommand)
                {
                    var selectArea = new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 9,
                        drawRect.Height - 1);

                    using (var selectBrush = new SolidBrush(Color.FromArgb(182, 189, 210)))
                    {
                        g.FillRectangle(selectBrush, selectArea);
                    }

                    using (var selectPen = new Pen(Color.FromArgb(10, 36, 106)))
                    {
                        g.DrawRectangle(selectPen, selectArea);
                    }
                }
                else
                {
                    var imageCol = new Rectangle(drawRect.Left, drawRect.Top, imageColWidth, drawRect.Height);

                    // Paint the main background color
                    using (Brush backBrush = new SolidBrush(Color.FromArgb(249, 248, 247)))
                    {
                        g.FillRectangle(backBrush,
                            new Rectangle(drawRect.Left + 1, drawRect.Top, drawRect.Width - 9, drawRect.Height));
                    }

                    using (Brush openBrush = new LinearGradientBrush(imageCol, Color.FromArgb(248, 247, 246),
                        Color.FromArgb(215, 211, 204), 0f))
                    {
                        g.FillRectangle(openBrush, imageCol);
                    }
                }

                // Calculate text drawing rectangle
                var strRect = new Rectangle(
                    leftPos,
                    drawRect.Top,
                    Width - imageColWidth - textGapLeft - 5,
                    drawRect.Height);

                // Left align the text drawing on a single line centered vertically
                // and process the & character to be shown as an underscore on next character
                var format = new StringFormat();
                format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
                format.Alignment = StringAlignment.Near;
                format.LineAlignment = StringAlignment.Center;
                format.HotkeyPrefix = HotkeyPrefix.Show;

                var textBrush = new SolidBrush(SystemColors.MenuText);
                g.DrawString(pMenuCommand.Text, SystemInformation.MenuFont, textBrush, strRect, format);

                // The image offset from top of cell is half the space left after
                // subtracting the height of the image from the cell height
                var imageTop = drawRect.Top + (drawRect.Height - 16) / 2;

                // Should a check mark be drawn?
                if (pMenuCommand.Checked)
                {
                    var boxPen = new Pen(Color.FromArgb(10, 36, 106));
                    Brush boxBrush;

                    if (hotCommand)
                        boxBrush = new SolidBrush(Color.FromArgb(133, 146, 181));
                    else
                        boxBrush = new SolidBrush(Color.FromArgb(212, 213, 216));

                    var boxRect = new Rectangle(imageLeft - 1, imageTop - 1, 16 + 2, 16 + 2);

                    // Fill the checkbox area very slightly
                    g.FillRectangle(boxBrush, boxRect);

                    // Draw the box around the checkmark area
                    g.DrawRectangle(boxPen, boxRect);

                    boxPen.Dispose();
                    boxBrush.Dispose();

                    var pPen = new Pen(Color.Black, 1);
                    g.DrawLine(pPen, new Point(imageLeft + 5, imageTop + 8),
                        new Point(imageLeft + 5 + 2, imageTop + 8 + 2));
                    g.DrawLine(pPen, new Point(imageLeft + 5, imageTop + 9),
                        new Point(imageLeft + 5 + 2, imageTop + 9 + 2));
                    g.DrawLine(pPen, new Point(imageLeft + 5 + 2, imageTop + 8 + 2),
                        new Point(imageLeft + 5 + 2 + 4, imageTop + 8 + 2 - 4));
                    g.DrawLine(pPen, new Point(imageLeft + 5 + 2, imageTop + 9 + 2),
                        new Point(imageLeft + 5 + 2 + 4, imageTop + 9 + 2 - 4));
                }
            }
        }

        private class MenuCommand
        {
            //private int m_iIndex;

            //public MenuCommand(string csText, int iIndex, bool bChecked)
            public MenuCommand(string csText, bool bChecked)
                : this(csText, bChecked, false)
            {
            }

            public MenuCommand(string csText)
                : this(csText, false, false)
            {
            }

            public MenuCommand(string csText, bool bChecked, bool bDone)
            {
                Text = csText;
                Checked = bChecked;
                Done = bDone;
            }

            public int Height => Separator ? 5 : 21;
            public bool Separator => Text == "-";

            public string Text { get; }

            //public int Index { get { return m_iIndex; } }
            public bool Done { get; }

            public bool Checked { get; set; }
        }
    }
}