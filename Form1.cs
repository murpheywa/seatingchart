using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeatingChart
{
    public partial class Form1 : Form
    {
        private frmSearingChart _seatingChart = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _seatingChart = new frmSearingChart();
            _seatingChart.Show( this );
        }
    }
}
