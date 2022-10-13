using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using PGS.DMS.Models;
using PGS.DMS.Properties;

namespace PGS.DMS.UI
{
    /// <summary>
    /// Панель навигации между директориями.
    /// </summary>
    public partial class UserControlPathNavigator : UserControl
    {
        /* Класс представляет из себя панель с кнопками для удобного перехода между группами.
         * 
         * Существует в пределах элемента ToolStrip и автоматически забирает всё свободное пространство.
         * Функция нажатия на кнопку привязывается при помощи делегата EventHandler MouseClickAction
         */

        // Цвет при наведении на кнопку
        private readonly Color HOVER_COLOR = Color.Bisque;
        // Дополнительная ширина кнопки под картинку
        private readonly int BUTTON_DISTANCE = 20;

        private readonly ToolStrip _parentToolStripToolStrip;

        public EventHandler MouseClickAction;

        public UserControlPathNavigator(ToolStrip parentToolStrip)
        {
            InitializeComponent();
            _parentToolStripToolStrip = parentToolStrip;
        }

        public void Update(DocumentGroup documentGroup)
        {
            var list = new List<DocumentGroup>();
            var tempDg = documentGroup;

            while (tempDg != null)
            {
                list.Add(tempDg);
                tempDg = tempDg.Parent;
            }

            list.Reverse();
            CreateButtons(list);
        }

        private void CreateButtons(List<DocumentGroup> list)
        {
            panel.Controls.Clear();
            var positionWidth = 0;

            foreach (var dg in list)
            {
                var btn = new Button();
                btn.Text = dg.Name;
                btn.Tag = dg.Id;

                btn.AutoSize = false;

                var textWidth = TextRenderer.MeasureText(dg.Name, Font).Width;
                textWidth += BUTTON_DISTANCE;

                btn.Size = new Size(textWidth, panel.Height);
                btn.Location = new Point(positionWidth, 0);
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = HOVER_COLOR;
                btn.Click += MouseClickAction;

                var image = ResizeImage(Resources.pathArrow, new Size(12, 12));
                btn.Image = image;
                btn.ImageAlign = ContentAlignment.MiddleRight;
                btn.TextAlign = ContentAlignment.MiddleLeft;

                panel.Controls.Add(btn);

                positionWidth += btn.Width;
            }
        }

        // Растягиваем на всё свободное пространство
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (_parentToolStripToolStrip.Orientation == Orientation.Vertical) return DefaultSize;

            var width = _parentToolStripToolStrip.DisplayRectangle.Width;

            if (_parentToolStripToolStrip.OverflowButton.Visible)
                width = width - _parentToolStripToolStrip.OverflowButton.Width -
                        _parentToolStripToolStrip.OverflowButton.Margin.Horizontal;

            foreach (ToolStripItem item in _parentToolStripToolStrip.Items)
            {
                if (item is ToolStripControlHost)
                    if (((ToolStripControlHost)item).Control is UserControlPathNavigator)
                        continue;
                if (item.IsOnOverflow) continue;

                width = width - item.Width - item.Margin.Horizontal;
            }

            if (width < DefaultSize.Width) width = DefaultSize.Width;

            var size = base.GetPreferredSize(proposedSize);
            size.Width = width;

            // Чтобы не конфликтовал с ToolStripControlHost.
            MinimumSize = new Size(width, Height);

            return size;
        }

        /// <summary>
        /// Изменение размера Bitmap, если изменить не удалось => скрываем картинку (Размер x:0 y:0)
        /// </summary>
        public static Bitmap ResizeImage(Bitmap imgToResize, Size size)
        {
            try
            {
                var b = new Bitmap(size.Width, size.Height);
                using (var g = Graphics.FromImage((Image)b))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                }

                return b;
            }
            catch
            {
                return new Bitmap(0, 0);
            }
        }
    }
}