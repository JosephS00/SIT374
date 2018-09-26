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
using System.Windows.Forms.DataVisualization.Charting;

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

        #region property
        //save the relationship between count of cell and property name
        Dictionary<String, int> PropertyNameToIndex;
        #endregion

        #region get file 
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }
        #endregion

        #region data import 
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
                //save this relation
                PropertyNameToIndex = new Dictionary<string, int>();
                //add head name to grid and propertyBox
                foreach (JProperty property in exampleToken)
                {
                    keys[count] = property.Name;
                    this.propertyBox.Items.Add(property.Name);
                    PropertyNameToIndex.Add(property.Name, count);
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
                //set row's data style by this
                //this.DataGrid.Rows[0].Cells[0].Style;

                //a example that changes the forecolor of the first row's second column 
                //this.DataGrid.Rows[0].Cells[1].Style.ForeColor = Color.Red;

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
                //add items into checkedListBoxs 
                //Clear all existing item in check box
                this.fieldBox.Items.Clear();
                this.chart1.Series.Clear();
                //add item to field box that would contain all field number
                for (int i = 1; i <= dataArray.Count; i++)
                {
                    this.fieldBox.Items.Add("field " + i.ToString());
                }

                //add item to property box that would contains all proerty name

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private Boolean Validate(JProperty jProperty)
        {
            //use the DataCompare class to check the input data
            switch (jProperty.Name)
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
                case "CO2 Emmissions":
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
        #endregion

        #region Generate Bar Chart
        private void GenerateBarChart_Click(object sender, EventArgs e)
        {
            //get checked items in checkedListBox1
            this.chart1.Series.Clear();
            for (int i = 0; i < this.fieldBox.Items.Count; i++)
            {
                //if field is checked, show them in chart
                if (this.fieldBox.GetItemChecked(i))
                {
                    String itemName = this.fieldBox.Items[i].ToString();

                    this.chart1.Series.Add(new Series(itemName));
                    AddPropertyToChart(itemName, i);
                }
            }
        }

        private void AddPropertyToChart(String field, int fieldNumber)
        {
            //add selected properties into Chart
            for (int i = 0; i < propertyBox.CheckedItems.Count; i++)
            {
                String currentProperty = propertyBox.CheckedItems[i].ToString();

                this.chart1.Series[field].Points.AddXY(currentProperty, this.DataGrid.Rows[fieldNumber].Cells[PropertyNameToIndex[currentProperty]].Value.ToString());
            }
        }
        #endregion

        #region Generate Line Chart
        private void GenerateLineChart_Click(object sender, EventArgs e)
        {
            this.chart1.Series.Clear();
            //for (int i = 0; i < this.fieldBox.Items.Count; i++)
            //{
            //    //if field is checked, show them in chart
            //    if (this.fieldBox.GetItemChecked(i))
            //    {
            //        String itemName = this.fieldBox.Items[i].ToString();

            //        Series lineChartItem = new Series(itemName);
            //        lineChartItem.ChartType = SeriesChartType.Line;

            //        this.chart1.Series.Add(new Series(itemName));

            //        AddPropertyToChart(itemName, i);
            //    }
            //}
            for (int i = 0; i < propertyBox.CheckedItems.Count; i++)
            {
                String currentProperty = propertyBox.CheckedItems[i].ToString();

                Series lineChartItem = new Series(currentProperty);
                lineChartItem.ChartType = SeriesChartType.Line;
                for (int j = 0; j < this.fieldBox.Items.Count; j++)
                {
                    //if field is checked, show them in chart
                    if (this.fieldBox.GetItemChecked(j))
                    {
                        String itemName = this.fieldBox.Items[j].ToString();

                        lineChartItem.Points.AddXY(itemName, this.DataGrid.Rows[j].Cells[PropertyNameToIndex[currentProperty]].Value.ToString());

                    }
                }
                this.chart1.Series.Add(lineChartItem);
            }
        }
        #endregion

    }
}
