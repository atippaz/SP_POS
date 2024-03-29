﻿using SP_POS.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SP_POS.Pages.ReportPage
{
    public partial class Report : UserControl
    {
        Sql sq = new Sql();
        DataTable OrderAll = new DataTable();
        DataTable orderTotal = new DataTable();

        public Report()
        {
            InitializeComponent();
            getAllOrder();
            getTotalOrder();
        }

        private void getTotalOrder(string date = "")
        {
            var data = new DataTable();
            if (date == "")
            {
                data = sq.Select("SELECT [OrderID],[OrderDate],[CreateDate],[ProdID],[ProdQty],[ProdPrice],[ProdCost],[realPrice] FROM [dbo].[ReportTotalOrder]");
            }
            else
            {
                data = sq.Select($"SELECT [OrderID],[OrderDate],[CreateDate],[ProdID],[ProdQty],[ProdPrice],[ProdCost],[realPrice] FROM [dbo].[ReportTotalOrder] where CreateDate ='{date}' ");
            }
            dgvAllorder.DataSource = data;
            orderTotal = data;
            calTotalorder();
        }

        void getAllOrder(string date = "")
        {
            var data = new DataTable();
            if (date == "")
            {
                 data = sq.Select("SELECT  [OrderID],[CreateDate],[OrderDetailID],[ProdID],[ProdQty],[ProdPrice] FROM .[dbo].[ReportAllOrder]");
            }
            else
            {
                data = sq.Select($"SELECT  [OrderID],[CreateDate],[OrderDetailID],[ProdID],[ProdQty],[ProdPrice] FROM .[dbo].[ReportAllOrder] where CreateDate ='{date}'");
            }
            dgvTotalOrder.DataSource = data;
            OrderAll = data;
            calAllorder();
        }
        void calAllorder()
        {
            int total = 0;
            foreach (DataRow item in OrderAll.Rows)
            {
                if(item["ProdQty"] != DBNull.Value && item["ProdPrice"]!=DBNull.Value)
                {
                    total += (Convert.ToInt32(item["ProdQty"].ToString()) * Convert.ToInt32(item["ProdPrice"].ToString()));
                }
            }
            Alllorderlbl.Text = total.ToString();
        }
        void calTotalorder()
        {
            int total = 0;
            foreach (DataRow item in orderTotal.Rows)
            {
                if (item["ProdQty"] != DBNull.Value && item["realPrice"] != DBNull.Value)
                {
                    total += (Convert.ToInt32(item["ProdQty"].ToString()) * Convert.ToInt32(item["realPrice"].ToString()));
                }
            }
            TotalOrderlbl.Text = total.ToString();
        }

        private void Allorder_Click(object sender, EventArgs e)
        {
            Excel ex = new Excel();
            ex.Print(OrderAll);
        }

        private void TotalOrder_Click(object sender, EventArgs e)
        {
            Excel ex = new Excel();
            ex.Print(orderTotal);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getAllOrder(dateTimePicker1.Value.ToString("yyyy-MM-dd"));
            getTotalOrder(dateTimePicker1.Value.ToString("yyyy-MM-dd"));
        }
    }
}
