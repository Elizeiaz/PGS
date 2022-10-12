using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PGS.Comps.UI
{
    /// <summary>
    /// Окно картинка компонента
    /// </summary>
    public partial class FormCompImage : Form
    {
        private CompInfo.CompImage _CompImage = null;

        private bool _EditMode = false;
        /// <summary>
        /// Режим редактирования
        /// </summary>
        public bool EditMode
        {
            get
            {
                return _EditMode;
            }
            set
            {
                _EditMode = value;
                BtnImageLoad.Enabled = value;
                BtnImagePaste.Enabled = value;
                BtnImageDelete.Enabled = value;

            }
        }
        /// <summary>
        /// Констркуктор
        /// </summary>
        /// <param name="CompImage"></param>
        public FormCompImage(CompInfo.CompImage CompImage)
        {
            InitializeComponent();
            _CompImage = CompImage;
            pictureBox1.Image = _CompImage.Image;
        }

        private void BtnImageLoad_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _CompImage.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.Image = _CompImage.Image;
                }
            }
        }

        private void BtnImageDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить изображение?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
             == DialogResult.Yes)
            {
                _CompImage.Image = null;
                pictureBox1.Image = null;
            }
        }

        private void BtnImagePaste_Click(object sender, EventArgs e)
        {
            try
            {
                Image image = Clipboard.GetImage();

                if (image == null)
                {
                    IDataObject dsta = Clipboard.GetDataObject();
                    System.IO.MemoryStream ms = (System.IO.MemoryStream)dsta.GetData("PNG");
                    if (ms != null)
                    {
                        image = Image.FromStream(ms);
                    }
                }
                if (image != null)
                {
                    _CompImage.Image = image;
                    pictureBox1.Image = image;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка вставки изображения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormCompImage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }
    }
}
