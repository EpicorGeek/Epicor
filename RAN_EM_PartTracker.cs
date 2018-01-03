// **************************************************
// Custom code for PartTrackerForm
// Created: 10/5/2017 12:05:40 PM
// **************************************************

extern alias Erp_Contracts_BO_NonConf;
extern alias Erp_Contracts_BO_SerialNo;
extern alias Ice_Contracts_BO_DynamicQuery;
extern alias Ice_Contracts_BO_QueryConversion;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Erp.Adapters;
using Erp.UI;
using Ice.Lib;
using Ice.Adapters;
using Ice.Lib.Customization;
using Ice.Lib.ExtendedProps;
using Ice.Lib.Framework;
using Ice.Lib.Searches;
using Ice.UI.FormFunctions;

using Ice.BO;
using Ice.UI;
using Erp.BO;
using Infragistics.Win;
using System.Collections;
using System.Collections.Generic;
using Infragistics.Win.UltraWinGrid;
using System.Drawing;
using Infragistics.Shared;

public class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **

	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		this.gridVehicle.ReadOnly = true;
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.btnRetrieve.Click -= new System.EventHandler(this.btnRetrieve_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void btnRetrieve_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		EpiDataView edvPartRev = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));
		DataRow edvPartRevRow = edvPartRev.CurrentDataRow;

		if ((edvPartRevRow != null))
		{
			FillVehicleGrid(edvPartRevRow["PartNum"].ToString(), edvPartRevRow["RevisionNum"].ToString());
		}
	}

	/** Same function as in Part Entry Form**/
	private void FillVehicleGrid(String vPartNum, String vRevisionNum)
	{
		try
		{
			DynamicQueryAdapter ad = new DynamicQueryAdapter(oTrans);
			ad.BOConnect();		
			QueryExecutionDataSet parameters = new QueryExecutionDataSet();				
			parameters.Tables["ExecutionParameter"].Rows.Add("vPartNum",vPartNum,"nvarchar",false,Guid.NewGuid(),"A");
			parameters.Tables["ExecutionParameter"].Rows.Add("vRevision",vRevisionNum,"nvarchar",false,Guid.NewGuid(),"A");
		
			ad.ExecuteByID("RAN_DEL_VehicleTable",parameters); 
			
			gridVehicle.DataSource = ad.QueryResults.Tables["Results"];
			
			ad.Dispose();

			//Hide Columns
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Company"].Hidden = true;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100A_ChildKey1"].Hidden = true;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100A_ChildKey2"].Hidden = true;
	
			//Set Format
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Number01"].Format = "###,###,###,##0.0";
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Number02"].Format = "###,###,###,##0.0";
	
			//Disable Columns
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Key1"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Key2"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Key3"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Key4"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Key5"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Number01"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["UD100_Number02"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.gridVehicle.DisplayLayout.Bands[0].Columns["Calculated_Check"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

			foreach (UltraGridRow row in gridVehicle.Rows)
			{
				row.Appearance.BackColorDisabled = System.Drawing.Color.White;
				row.Appearance.ForeColorDisabled = System.Drawing.Color.Black;
			}

		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}
	
}
