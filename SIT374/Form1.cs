using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataApi;

namespace SIT374
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region constant
        const String invalidePrompt = "invalide data";
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                JArray dataArray = GetDataFromDB.ReadCsvFile(textBox1.Text);
                //extract all data property name.
                JToken exampleToken = dataArray[0];
                String[] keys = new String[dataArray[0].Count()];
                List<String[]> values = new List<String[]>();

                int count = 0;
                foreach (JProperty property in exampleToken)
                {
                    keys[count] = property.Name;
                    count++;
                }
                // Set how many kinds of data should be show
                DataGrid.ColumnCount = dataArray[0].Count();
                DataGrid.ColumnHeadersVisible = true;

                // Set the column header style.
                DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

                columnHeaderStyle.BackColor = Color.Beige;
                columnHeaderStyle.Font = new Font("Verdana", 10, FontStyle.Bold);
                DataGrid.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

                //set the row style
                DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
                //DataGrid.RowsDefaultCellStyle;

                // Set the column header names.
                for (int i = 0; i < DataGrid.ColumnCount; i++)
                {
                    DataGrid.Columns[i].Name = keys[i];
                }

                // Populate the rows.
                foreach (JToken jToken in dataArray)
                {
                    String[] aValues = new String[dataArray[0].Count()];
                    int j = 0;
                    foreach (JProperty property in jToken)
                    {
                        if (Validate(property))
                        {
                            aValues[j] = property.Value.ToString();
                        }
                        else
                        {
                            aValues[j] = invalidePrompt;
                        }
                        j++;
                    }
                    values.Add(aValues);
                    DataGrid.Rows.Add(aValues);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
        }

        private Boolean Validate(JProperty jProperty)
        {
            //use the DataCompare class to check the input data
            switch(jProperty.Name)
            {
                case "Accelaration Pedal":
                    return DataCompare.AccelarationPedal(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "RPM":
                    return DataCompare.RPM(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Power":
                    return DataCompare.Power(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Torque":
                    return DataCompare.Torque(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Cylinders":
                    return DataCompare.Cylinders(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Valves per Cylinder":
                    return DataCompare.ValvesperCylinder(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Cylinder Capacity":
                    return DataCompare.CylinderCapacity(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Top Speed":
                    return DataCompare.TopSpeed(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Acceleration":
                    return DataCompare.Acceleration(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Fuel Consumption":
                    return DataCompare.FuelConsumption(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "CO2 Emissions":
                    return DataCompare.CO2Emissions(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                case "Weight":
                    return DataCompare.Weight(Convert.ToDouble(jProperty.Value.ToString()));
                    break;
                default:
                    return false;
                    break;
            }
        }

        private void DataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
