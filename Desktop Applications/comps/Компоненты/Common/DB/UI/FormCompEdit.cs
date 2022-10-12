using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using PGS.Comps;
using System.Text;
using Microsoft.Win32;

namespace PGS.UI.Comps
{
    /// <summary>
    /// Окно просмотра/редактирования свойств компонента
    /// </summary>
    public partial class FormCompEdit : Form
    {
        private bool _EditMode = false;

        /// <summary>
        /// режим редактирования
        /// </summary>
        public bool EditMode
        {
            get
            {
                return _EditMode;
            }
            set
            {
                if (_EditMode != value)
                {
                    _EditMode = value;
                    EnableEditing(value);
                }
            }
        }

        #region Отключение редактирования

        /// <summary>
        /// Отключение/включение редактирования
        /// </summary>
        private void EnableEditing(bool isEnable)
        {
            foreach (var control in Controls) EnableEditingInControl(control, isEnable);

            pictureBox1.Enabled = true;
            button_save.Enabled = false;
            button_save.Visible = isEnable;
            button_cancel.Text = isEnable ? "Отмена" : "Закрыть";
            button_cancel.Enabled = true;
            btn_TempUnits.Enabled = true;
        }

        /// <summary>
        /// Рекурсивное отключение редактирования
        /// </summary>
        private void EnableEditingInControl(object control, bool isEnable)
        {
            if (control.GetType() == typeof(TextBox) ||
                control.GetType() == typeof(Controls.NumEdit))
            {
                var tmp = ((TextBox)control).BackColor;
                ((TextBox)control).ReadOnly = !isEnable;
                ((TextBox)control).BackColor = tmp;
            }
            else if (control.GetType() == typeof(CheckBox))
            {
                ((CheckBox)control).AutoCheck = isEnable;
            }
            else if (control.GetType() == typeof(ComboBox))
            {
                var tmp = ((ComboBox)control).BackColor;
                ((ComboBox)control).Enabled = isEnable;
                ((ComboBox)control).BackColor = tmp;
            }
            else if (control.GetType() == typeof(Button))
            {
                ((Button)control).Enabled = isEnable;
            }
            else if (control.GetType() == typeof(PictureBox))
            {
                ((PictureBox)control).Enabled = isEnable;
            }
            else if (control.GetType() == typeof(ToolStrip))
            {
                foreach (ToolStripItem item in ((ToolStrip)control).Items)
                    item.Enabled = isEnable;
            }
            else if (control is Control)
            {
                foreach (var item in ((Control)control).Controls) EnableEditingInControl(item, isEnable);
            }
        }

        #endregion

        private CompInfo _compInfo = null;

        private CompInfo compInfo
        {
            get
            {
                return _compInfo;
            }
            set
            {
                if (_compInfo != value)
                {
                    _compInfo = value;
                    if (_compInfo != null) _compInfo.Changed += Changed;
                    FillFormFields();
                }
            }
        }


        private bool _TempInKelvin = true;

        /// <summary>
        /// Показывать температуру в К
        /// </summary>
        public bool TempInKelvin
        {
            get
            {
                return _TempInKelvin;
            }
            set
            {
                _TempInKelvin = value;
                var units = TempInKelvin ? "K" : "°C";
                btn_TempUnits.Text = units;
                label_tkip.Text = "Tкип., " + units;
                label_tkr.Text = "Tкр., " + units;
                UpdateTempUnits();
            }
        }

        private void Changed()
        {
            button_save.Enabled = !compInfo.Saved;
        }

        #region Заполнение полей

        private string DoubleToString(double value)
        {
            return double.IsNaN(value) ? "" : ((decimal)value).ToString();
        }

        private string PsatToString(double value)
        {
            if (double.IsNaN(value)) return "";

            return double.IsInfinity(value) ? "" : value.ToString();
        }


        private void FillFormFields()
        {
            // Заполнение comboBox и dgv

            UpdateDgvIncompatibility();

            // Основные
            textBox_name.Text = compInfo.Name;
            textBox_namealt.Text = compInfo.NameAlt;
            textBox_formula.Text = compInfo.Formula;
            checkBox_isIsomer.Checked = compInfo.Isomer;
            textBox_pname.Text = compInfo.PrintName;
            textBox_DisplayName.Text = compInfo.DisplayName;

            comboBox_group.SelectedValue = compInfo.GroupID ?? -1;

            textBox_kod1c.Text = compInfo.Kod_1C.ToString();

            checkBox_isToxicity.Checked = compInfo.Properties.IsToxic;
            checkBox_isFlammability.Checked = compInfo.Properties.IsInflammable;

            //Закладка основные свойства
            tb_molar.Value = compInfo.Properties.MolarMass;
            tb_dmolar.Value = compInfo.Properties.dMolarMass;

            tb_density.Value = compInfo.Properties.MassDensity;
            UpdateTempUnits();
            tb_pkr.Value = compInfo.Properties.Pkr / 1000000;
            tb_w.Value = compInfo.Properties.w;
            tb_z.Value = compInfo.Properties.Z;
            UpdateZSource();

            textBox_pss243k.Text = PsatToString(compInfo.Properties.Psat.Ps[0]);
            textBox_pss253k.Text = PsatToString(compInfo.Properties.Psat.Ps[1]);
            textBox_pss263k.Text = PsatToString(compInfo.Properties.Psat.Ps[2]);
            textBox_pss273k.Text = PsatToString(compInfo.Properties.Psat.Ps[3]);
            textBox_pss283k.Text = PsatToString(compInfo.Properties.Psat.Ps[4]);
            textBox_pss293k.Text = PsatToString(compInfo.Properties.Psat.Ps[5]);


            // Вкладка "Токсичность"

            if (compInfo.Properties.ToxicityInfo.isEmpty)
            {
                textBox_pdk.Text = "";
                comboBox_pdkUnits.SelectedIndex = -1;
            }
            else
            {
                textBox_pdk.Text = DoubleToString(compInfo.Properties.ToxicityInfo.PDK);
                comboBox_pdkUnits.SelectedValue = (int)compInfo.Properties.ToxicityInfo.Units;
            }

            // Вкладка "Воспламеняемость"
            if (compInfo.Properties.FlammabilityInfo != null)
            {
                textBox_nkpr.Text = DoubleToString(compInfo.Properties.FlammabilityInfo.NKPR_Air);
                textBox_vkpr.Text = DoubleToString(compInfo.Properties.FlammabilityInfo.VKPR_Air);
                textBox_maxAir.Text = DoubleToString(compInfo.Properties.FlammabilityInfo.MaxConc_Air);
            }

            // Вкладка КРасхода
            if (compInfo.Properties.KFlow != null)
                textBox_kFlow.Text = DoubleToString(compInfo.Properties.KFlow.K);

            if (compInfo.Image.Image != null) pictureBox1.Image = compInfo.Image.Image;

            // Вкладка ГОСТ
            if (compInfo.Properties.GOST_31369 != null && !compInfo.Properties.GOST_31369.isEmpty)
            {
                tbGostZ.Text = compInfo.Properties.GOST_31369.Z.ToString();
                tbGostDZ.Text = compInfo.Properties.GOST_31369.dZ.ToString();
                tbGostB.Text = compInfo.Properties.GOST_31369.b.ToString();
                tbGostHMin.Text = compInfo.Properties.GOST_31369.Hmin.ToString();
                tbGostHMax.Text = compInfo.Properties.GOST_31369.Hmax.ToString();
            }

            tabControl1.SelectedIndex = 0;

            var si = compInfo.Status;
            if (si != null)
            {
                pbInRaw.Image = si.InRaws ? Common.Resources.task_complete : Common.Resources.task_reject;
                pbInReference.Image = si.InReferences ? Common.Resources.task_complete : Common.Resources.task_reject;

                if (si.InRaws)
                {
                    toolTip1.SetToolTip(pbInRaw, "Есть сырье");
                    toolTip1.SetToolTip(lbInRaws, "Есть сырье");
                }
                else
                {
                    toolTip1.SetToolTip(pbInRaw, "Нет сырья");
                    toolTip1.SetToolTip(lbInRaws, "Нет сырья");
                }

                if (si.InReferences)
                {
                    var range = "";
                    if (!double.IsNaN(si.MinReferenceConc))
                        range += string.Format("от {0} ",
                            Rounding.ValueToString(si.MinReferenceConc, si.MinReferenceConc * 0.01));
                    if (!double.IsNaN(si.MaxReferenceConc))
                        range += string.Format("до {0}",
                            Rounding.ValueToString(si.MaxReferenceConc, si.MaxReferenceConc * 0.01));

                    toolTip1.SetToolTip(pbInReference, range);
                    toolTip1.SetToolTip(lbInReferences, range);
                }
                else
                {
                    toolTip1.SetToolTip(pbInReference, "Нет эталонов");
                    toolTip1.SetToolTip(lbInReferences, "Нет эталонов");
                }

                if (si.Updated != null)
                    gbDtatuses.Text = string.Format("Наличие от {0:dd.MM.yy}", si.Updated);
                else
                    gbDtatuses.Text = "Наличие";
            }
            else
            {
                pbInRaw.Image = Common.Resources.task_reject;
                pbInReference.Image = Common.Resources.task_reject;
                lbInReferences.Visible = false;
                toolTip1.SetToolTip(pbInReference, "Нет эталонов");
                toolTip1.SetToolTip(lbInReferences, "Нет эталонов");
                gbDtatuses.Text = "Наличие";
            }
        }

        private void UpdateTempUnits()
        {
            if (compInfo == null) return;
            var To = TempInKelvin ? 0 : 273.15;
            tb_tkip.Value = compInfo.Properties.Tkip - To;
            tb_tkr.Value = compInfo.Properties.Tkr - To;
        }

        private void UpdateZSource()
        {
            switch (compInfo.Properties.ZSource)
            {
                case CompProperties.ValueSource.Default:
                    label_ZFlag.Text = "*Не задано";
                    break;
                case CompProperties.ValueSource.Calc:
                    label_ZFlag.Text = "*Расчет";
                    break;
                case CompProperties.ValueSource.Set:
                    label_ZFlag.Text = "*Задано";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// обновление таблицы несовместимости
        /// </summary>
        private void UpdateDgvIncompatibility()
        {
            dgvIncompatibility.Rows.Clear();

            foreach (var item in compInfo.Properties.Incompatibility.Comps)
                dgvIncompatibility.Rows.Add(item.ID, item.DisplayName);
        }

        #endregion

        #region Изменение полей

        private Color ColorWarning = Color.FromArgb(255, 230, 230);

        /// <summary>
        /// Проверяет введенное значение. Подсвечивает если путое
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private string CheckText(object sender)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (string.IsNullOrWhiteSpace(tb.Text))
                {
                    tb.BackColor = ColorWarning;
                    return "";
                }
                else
                {
                    tb.BackColor = SystemColors.Window;
                    return tb.Text;
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Проверяет значениею Возвращае double.Nan;
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private double CheckValue(object sender)
        {
            var tb = sender as TextBox;

            double v;
            if (double.TryParse((sender as TextBox).Text, out v))
            {
                return v;
            }
            else
            {
                tb.BackColor = ColorWarning;
                return double.NaN;
            }
        }

        private double CheckValuePercent(object sender)
        {
            var tb = sender as TextBox;

            double v;
            if (double.TryParse((sender as TextBox).Text, out v))
            {
                if (v >= 0 && v <= 100)
                    tb.BackColor = SystemColors.Window;
                else
                    tb.BackColor = ColorWarning;
                return v;
            }
            else
            {
                tb.BackColor = ColorWarning;
                return double.NaN;
            }
        }

        // Базовая информация о компоненте
        private void textBox_name_TextChanged(object sender, EventArgs e)
        {
            compInfo.Name = CheckText(sender);
        }

        private void textBox_namealt_TextChanged(object sender, EventArgs e)
        {
            compInfo.NameAlt = textBox_namealt.Text;
        }

        private void textBox_formula_TextChanged(object sender, EventArgs e)
        {
            if (OnlyLatinAndDigitValidator(textBox_formula.Text))
            {
                textBox_formula.BackColor = SystemColors.Window;
                compInfo.Formula = textBox_formula.Text;

                UpdateIndexesLabel();
            }
            else
            {
                textBox_formula.BackColor = ColorWarning;
            }
        }

        private void UpdateIndexesLabel()
        {
            var indexes = compInfo.Indexes;

            if (indexes.Length > 1 && indexes[0] > 0)
            {
                label_CName.Visible = true;
                label_CIndex.Text = indexes[0].ToString();
            }
            else
            {
                label_CName.Visible = false;
                label_CIndex.Text = "";
            }

            if (indexes.Length > 2 && indexes[1] > 0)
            {
                label_HName.Visible = true;
                label_HIndex.Text = indexes[1].ToString();
            }
            else
            {
                label_HName.Visible = false;
                label_HIndex.Text = "";
            }

            if (indexes.Length > 3 && indexes[2] > 0)
            {
                label_OName.Visible = true;
                label_OIndex.Text = indexes[2].ToString();
            }
            else
            {
                label_OName.Visible = false;
                label_OIndex.Text = "";
            }
        }

        private void checkBox_isIsomer_CheckedChanged(object sender, EventArgs e)
        {
            compInfo.Isomer = checkBox_isIsomer.Checked;
        }

        private void textBox_pname_TextChanged(object sender, EventArgs e)
        {
            compInfo.PrintName = CheckText(sender);
        }

        private void tb_DisplayName_TextChanged(object sender, EventArgs e)
        {
            compInfo.DisplayName = CheckText(sender);
        }

        private void comboBox_group_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (compInfo == null) return;

            if ((int)comboBox_group.SelectedValue == -1)
                compInfo.GroupID = null;
            else
                compInfo.GroupID = (int)comboBox_group.SelectedValue;
        }

        private void checkBox_isFlammability_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_isFlammability.Checked)
            {
                if (!compInfo.Properties.FlammabilityInfo.isEmpty)
                {
                    if (MessageBox.Show("Значения НКПР/ВКПР/Макс. С\nБудут потеряны. Продолжить?",
                            "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        compInfo.Properties.FlammabilityInfo.NKPR_Air = double.NaN;
                        compInfo.Properties.FlammabilityInfo.VKPR_Air = double.NaN;
                        compInfo.Properties.FlammabilityInfo.MaxConc_Air = double.NaN;
                        textBox_nkpr.Text = "";
                        textBox_vkpr.Text = "";
                        textBox_maxAir.Text = "";
                    }
                    else
                    {
                        checkBox_isFlammability.Checked = true;
                        return;
                    }
                }
            }
            else
            {
                tabControl1.SelectedIndex = 3;
            }

            compInfo.Properties.IsInflammable = checkBox_isFlammability.Checked;
        }

        private void checkBox_isToxicity_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox_isToxicity.Checked)
            {
                if (compInfo.Properties.ToxicityInfo.isEmpty)
                {
                    if (MessageBox.Show(
                            "Значение ПДК " + compInfo.Properties.ToxicityInfo.PDK + " Будет потеряно.\nПродолжить?",
                            "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        compInfo.Properties.ToxicityInfo.PDK = double.NaN;
                        textBox_pdk.Text = "";
                        comboBox_pdkUnits.SelectedIndex = -1;
                    }
                    else
                    {
                        checkBox_isToxicity.Checked = true;
                        return;
                    }
                }
            }
            else
            {
                tabControl1.SelectedIndex = 2;
            }

            compInfo.Properties.IsToxic = checkBox_isToxicity.Checked;
        }

        #region Вкладка Основные

        private void tb_molar_ValueChanged(object sender, EventArgs arg2)
        {
            compInfo.Properties.MolarMass = tb_molar.Value;
        }

        private void tb_dmolar_ValueChanged(object sender, EventArgs arg2)
        {
            compInfo.Properties.dMolarMass = tb_dmolar.Value;
        }

        private void tb_density_ValueChanged(object sender, EventArgs arg2)
        {
            compInfo.Properties.MassDensity = tb_density.Value;
        }

        private void tb_tkip_ValueChanged(object sender, EventArgs arg2)
        {
            var To = TempInKelvin ? 0 : 273.15;
            compInfo.Properties.Tkip = tb_tkip.Value + To;
        }

        private void tb_tkr_ValueChanged(object arg1, EventArgs arg2)
        {
            var To = TempInKelvin ? 0 : 273.15;
            compInfo.Properties.Tkr = tb_tkr.Value + To;
        }

        private void tb_pkr_ValueChanged(object arg1, EventArgs arg2)
        {
            compInfo.Properties.Pkr = tb_pkr.Value;
        }

        private void tb_w_ValueChanged(object arg1, EventArgs arg2)
        {
            compInfo.Properties.w = tb_w.Value;
        }

        private void tb_z_ValueChanged(object arg1, EventArgs arg2)
        {
            compInfo.Properties.Z = tb_z.Value;
            compInfo.Properties.ZSource = CompProperties.ValueSource.Set;
            label_ZFlag.Text = "*Задано";
        }

        #endregion

        private void textBox_kod1c_TextChanged(object sender, EventArgs e)
        {
            int kod_1C;
            if (int.TryParse(textBox_kod1c.Text, out kod_1C))
            {
                textBox_kod1c.BackColor = SystemColors.Window;
                compInfo.Kod_1C = kod_1C;
            }
            else
            {
                textBox_kod1c.BackColor = ColorWarning;
            }
        }

        // Вкладка Рнп.
        private void textBox_pss_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            var i = (int)tb.Tag;
            double v;
            if (double.TryParse(tb.Text, out v))
                compInfo.Properties.Psat.Ps[i] = v;
            else
                compInfo.Properties.Psat.Ps[i] = double.NaN;
        }


        // Вкладка токсичность
        private void textBox_pdk_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.ToxicityInfo.PDK = CheckValue(sender);
            if (!compInfo.Properties.ToxicityInfo.isEmpty)
            {

            }
            else
            {
                checkBox_isToxicity.Checked = false;
                compInfo.Properties.IsToxic = false;
            }
        }

        private void comboBox_pdkUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (compInfo != null && comboBox_pdkUnits.SelectedValue != null)
                compInfo.Properties.ToxicityInfo.Units = (Concentration.Units)comboBox_pdkUnits.SelectedValue;
        }

        // Вкладка воспламеняемость
        private void textBox_nkpr_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.FlammabilityInfo.NKPR_Air = CheckValuePercent(sender);
            checkBox_isFlammability.Checked = !compInfo.Properties.FlammabilityInfo.isEmpty;
        }

        private void textBox_vkpr_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.FlammabilityInfo.VKPR_Air = CheckValuePercent(sender);
            ;
            checkBox_isFlammability.Checked = !compInfo.Properties.FlammabilityInfo.isEmpty;
        }

        private void textBox_maxAir_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.FlammabilityInfo.MaxConc_Air = CheckValuePercent(sender);
            ;
            checkBox_isFlammability.Checked = !compInfo.Properties.FlammabilityInfo.isEmpty;
        }

        // Вкладка Красхода
        private void textBox_kFlow_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.KFlow.K = CheckValue(sender);
        }

        // Вкладка ГОСТ
        private void tbGostZ_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.GOST_31369.Z = CheckValue(sender);
        }

        private void tbGostDZ_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.GOST_31369.dZ = CheckValue(sender);
        }

        private void tbGostB_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.GOST_31369.b = CheckValue(sender);
        }

        private void tbGostHMax_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.GOST_31369.Hmax = CheckValue(sender);
        }

        private void tbGostHMin_TextChanged(object sender, EventArgs e)
        {
            compInfo.Properties.GOST_31369.Hmin = CheckValue(sender);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var f = new PGS.Comps.UI.FormCompImage(compInfo.Image);
            f.EditMode = EditMode;
            f.ShowDialog();
        }

        #endregion

        private void Init()
        {
            InitializeComponent();
            EnableEditing(EditMode);

            var con = DB.Connect();
            try
            {
                LoadGroups(con);
                LoadUnits(con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Констркуктор
        /// </summary>
        public FormCompEdit()
        {
            Init();
            compInfo = new CompInfo();
        }

        /// <summary>
        /// Констркуктор
        /// </summary>
        /// <param name="compInfo"></param>
        public FormCompEdit(CompInfo compInfo)
        {
            Init();
            this.compInfo = compInfo;
        }

        /// <summary>
        /// Констркуктор
        /// </summary>
        /// <param name="id"></param>
        public FormCompEdit(int id)
        {
            Init();
            compInfo = CompInfo.Load(id);
        }


        private class Group
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public Group(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        /// <summary>
        /// Загрузка групп
        /// </summary>
        private void LoadGroups(OdbcConnection con)
        {
            var groupList = new List<Group>();
            groupList.Add(new Group(-1, ""));

            var com = con.CreateCommand();
            com.CommandText = @"SELECT id, name FROM comps.comp_groups";
            var reader = com.ExecuteReader();

            while (reader.Read())
                groupList.Add(new Group((int)reader["id"], (string)reader["name"]));

            comboBox_group.ValueMember = "id";
            comboBox_group.DisplayMember = "Name";
            comboBox_group.DataSource = groupList;

            comboBox_group.SelectedItem = groupList[0];
        }

        /// <summary>
        /// Загрузка е.и. для концентрации
        /// </summary>
        private void LoadUnits(OdbcConnection con)
        {
            var com = con.CreateCommand();
            com.CommandText = @"SELECT id, name FROM common.conc_units";
            var reader = com.ExecuteReader();
            comboBox_pdkUnits.DisplayMember = "name";
            comboBox_pdkUnits.ValueMember = "id";
            var items = new List<object>();

            while (reader.Read()) items.Add(new { id = (int)reader["id"], name = (string)reader["name"] });
            comboBox_pdkUnits.DataSource = items;
        }


        #region валидация

        private bool ValidBeforeSaving()
        {
            if (!ValidBaseInfo()) return false;

            if (!compInfo.Properties.isEmpty)
                if (!ValidProperties())
                    return false;

            if (compInfo.Properties.ToxicityInfo != null && !compInfo.Properties.ToxicityInfo.isEmpty)
                if (!ValidToxicity())
                    return false;

            if (compInfo.Properties.FlammabilityInfo != null && !compInfo.Properties.FlammabilityInfo.isEmpty)
                if (!ValidFlammability())
                    return false;

            return true;
        }

        private bool ValidBaseInfo()
        {
            if (string.IsNullOrWhiteSpace(compInfo.Name))
            {
                textBox_name.BackColor = ColorWarning;
                MessageBox.Show("Введите название компонента", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            ;

            if (string.IsNullOrWhiteSpace(compInfo.Formula))
            {
                textBox_formula.BackColor = ColorWarning;
                MessageBox.Show("Введите формулу", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            ;

            if (string.IsNullOrWhiteSpace(compInfo.PrintName))
            {
                textBox_pname.BackColor = ColorWarning;
                MessageBox.Show("Введите название для печати", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool OnlyLatinAndDigitValidator(string str)
        {
            const string regPattern = @"(?![a-zA-Z0-9-\.]+).";
            return !Regex.IsMatch(str, regPattern);
        }


        private bool ValidProperties()
        {
            if (compInfo.Properties.MolarMass == double.NaN || !tb_molar.ValueValid)
            {
                tabControl1.SelectedIndex = 0;
                MessageBox.Show("Введите молярную массу", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            ;

            if (compInfo.Properties.dMolarMass == double.NaN || !tb_dmolar.ValueValid)
            {
                tabControl1.SelectedIndex = 0;
                MessageBox.Show("Введите погрешность молярной массы", "Валидация", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }

            ;

            return true;
        }

        private bool ValidToxicity()
        {
            if (double.IsNaN(compInfo.Properties.ToxicityInfo.PDK))
            {
                tabControl1.SelectedIndex = 2;
                textBox_pdk.BackColor = Color.LightCoral;
                MessageBox.Show("Введите ПДК", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool ValidFlammability()
        {
            if (double.IsNaN(compInfo.Properties.FlammabilityInfo.NKPR_Air))
            {
                tabControl1.SelectedIndex = 3;
                textBox_nkpr.BackColor = Color.LightCoral;
                MessageBox.Show("Введите НКПР", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (compInfo.Properties.FlammabilityInfo.NKPR_Air > 100)
            {
                tabControl1.SelectedIndex = 3;
                textBox_nkpr.BackColor = Color.LightCoral;
                MessageBox.Show("Некорректное значение", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (double.IsNaN(compInfo.Properties.FlammabilityInfo.VKPR_Air))
            {
                tabControl1.SelectedIndex = 3;
                textBox_vkpr.BackColor = Color.LightCoral;
                MessageBox.Show("Введите ВКПР", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (compInfo.Properties.FlammabilityInfo.VKPR_Air > 100)
            {
                tabControl1.SelectedIndex = 3;
                textBox_vkpr.BackColor = Color.LightCoral;
                MessageBox.Show("Некорректное значение", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (double.IsNaN(compInfo.Properties.FlammabilityInfo.MaxConc_Air))
            {
                tabControl1.SelectedIndex = 3;
                textBox_maxAir.BackColor = Color.LightCoral;
                MessageBox.Show("Введите Максимум", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (compInfo.Properties.FlammabilityInfo.MaxConc_Air > 100)
            {
                tabControl1.SelectedIndex = 3;
                textBox_maxAir.BackColor = Color.LightCoral;
                MessageBox.Show("Некорректное значение", "Валидация", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }


            return true;
        }

        #endregion

        private void FormCompEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (EditMode && !compInfo.Saved)
            {
                if (MessageBox.Show("Данные не сохранены. Закрыть?", "", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning)
                    == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (compInfo.ID >= 0)
                        CompInfo.Load(compInfo.ID, DataSets.All);
                }
            }
        }

        // cancel
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        // Добавить компонент
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidBeforeSaving()) return;
            var con = DB.Connect();

            OdbcTransaction transaction = null;
            try
            {
                transaction = con.BeginTransaction();

                compInfo.Save(con, transaction);
                transaction.Commit();
                DialogResult = DialogResult.OK;
            }
            catch (Exception exception)
            {
                if (transaction != null) transaction.Rollback();
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                con.Disconnect();
            }
        }

        // Добавление компонента в dgv
        private void button3_Click(object sender, EventArgs e)
        {
            var fcs = new FormCompSelect();
            if (fcs.ShowDialog() == DialogResult.OK)
            {
                if (fcs.CompID != null)
                {
                    var new_id = (int)fcs.CompID;

                    if (new_id != compInfo.ID &&
                        compInfo.Properties.Incompatibility.Comps.Find((x) => { return x.ID == new_id; }) == null)
                        compInfo.Properties.Incompatibility.Comps.Add(CompInfo.Load((int)fcs.CompID));
                }

                UpdateDgvIncompatibility();
            }
        }

        // Удаление компонента из dgv
        private void button5_Click(object sender, EventArgs e)
        {
            if (dgvIncompatibility.SelectedRows.Count == 0)
                MessageBox.Show("Выберите компонент", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var index = dgvIncompatibility.SelectedRows[0].Index;

            if (MessageBox.Show(
                    "Удалить компонент: " + dgvIncompatibility[Column2.Index, index].Value + "?",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                var id = (int)dgvIncompatibility[Column1.Index, index].Value;
                compInfo.Properties.Incompatibility.Comps.Remove(
                    compInfo.Properties.Incompatibility.Comps.Find((x) => { return x.ID == id; }));
            }

            UpdateDgvIncompatibility();
        }


        private void button_CalcZ_Click(object sender, EventArgs e)
        {
            compInfo.Properties.ReCalcZ();

            if (compInfo.Properties.ZSource != CompProperties.ValueSource.Default)
            {
                tb_z.Value = compInfo.Properties.Z;
                UpdateZSource();
            }
            else
            {
                tb_molar.ColorValueEmpty = ColorWarning;
                tb_tkip.ColorValueEmpty = ColorWarning;
                tb_tkr.ColorValueEmpty = ColorWarning;
                tb_pkr.ColorValueEmpty = ColorWarning;
                tb_w.ColorValueEmpty = ColorWarning;
                tb_z.Value = double.NaN;
                label_ZFlag.Text = "*Недостаточно данных для расчёта";
            }
        }


        private void button_changeKod_1C_Click(object sender, EventArgs e)
        {
            if (textBox_kod1c.Enabled) return;

            if (MessageBox.Show("Редактировать Код 1С?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
                textBox_kod1c.Enabled = true;
        }


        private void button_reGroup_Click(object sender, EventArgs e)
        {
            var groupForm = new Groups.FormGroupEdit("comps.comp_groups");
            groupForm.ShowDialog();
            var con = DB.Connect();
            LoadGroups(con);
            con.Disconnect();
        }

        private void BtnImageLoad_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Файлы изображений (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    compInfo.Image.Image = Image.FromFile(openFileDialog.FileName);
                    pictureBox1.Image = compInfo.Image.Image;
                }
            }
        }

        private void BtnImageDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить изображение?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                compInfo.Image.Image = null;
                pictureBox1.Image = null;
            }
        }

        private void BtnImagePaste_Click(object sender, EventArgs e)
        {
            try
            {
                var image = Clipboard.GetImage();
                if (image == null)
                {
                    var dsta = Clipboard.GetDataObject();
                    var ms = (System.IO.MemoryStream)dsta.GetData("PNG");
                    if (ms != null) image = Image.FromStream(ms);
                }
                else
                {
                    compInfo.Image.Image = image;
                    pictureBox1.Image = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка вставки изображения", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormCompEdit_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void FormCompEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();
        }

        private void LoadSettings()
        {
            using (var regKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PGS\\UI\\FormCompEdit"))
            {
                if (regKey == null) return;

                /*   WindowState = (FormWindowState)Enum.Parse(
                       typeof(FormWindowState), (string)regKey.GetValue("WindowState", WindowState.ToString()));

                   Left = (int)regKey.GetValue("WindowLeft", Left);
                   Top = (int)regKey.GetValue("WindowTop", Top);
                   Width = (int)regKey.GetValue("WindowWidth", Width);
                   Height = (int)regKey.GetValue("WindowHeight", Height);
   */
                TempInKelvin = bool.Parse((string)regKey.GetValue("TempInKelvin", "false"));
            }
        }

        private void SaveSettings()
        {
            using (var regKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PGS\\UI\\FormCompEdit"))
            {
                if (regKey == null)
                    return;

                regKey.SetValue("WindowState", WindowState.ToString());
                if (WindowState != FormWindowState.Maximized)
                {
                    regKey.SetValue("WindowLeft", Left);
                    regKey.SetValue("WindowTop", Top);
                    regKey.SetValue("WindowWidth", Width);
                    regKey.SetValue("WindowHeight", Height);
                    regKey.SetValue("WindowHeight", Height);
                }

                regKey.SetValue("TempInKelvin", TempInKelvin);
            }
        }

        private void btn_TempUnits_Click_1(object sender, EventArgs e)
        {
            TempInKelvin = !TempInKelvin;
        }

        private void gbDtatuses_Enter(object sender, EventArgs e)
        {
        }
    }
}