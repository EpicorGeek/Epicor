// **************************************************
// Custom code for PartForm
// Created: 8/21/2017 7:27:50 AM
// **************************************************

extern alias Erp_Contracts_BO_PartRevSearch;
extern alias Erp_Adapters_PO;
extern alias Erp_Adapters_PartRevSearch;


extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_PartPlantSearch;
extern alias Erp_Contracts_BO_PO;
extern alias Erp_Contracts_BO_PartOnHandWhse;
extern alias Erp_Contracts_BO_Vendor;
extern alias Erp_Contracts_BO_VendorPPSearch;
extern alias Ice_Adapters_UD100;
extern alias Ice_Contracts_BO_UD100;
extern alias Ice_Contracts_BO_QueryConversion;
extern alias Ice_Contracts_BO_DynamicQuery;
// Data  Migration
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

	private EpiDataView edvPartRev;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **
	private PartRevSearchAdapter partRevAdapter;
	private EpiDataView partRevView;

	private static EpiTextBox height;
	private static EpiTextBox width;
	private static EpiTextBox depth;
    
    private static EpiTextBox weight;
    private static EpiTextBox primaryMaterial;
    private static EpiTextBox color;
    private static EpiTextBox weightcapacity;
    
    private static EpiTextBox installTime;
    private static EpiTextBox requireInstallKit;
    
    private static EpiTextBox usedWith;
    private static EpiTextBox maxLadderLength;
    private static EpiTextBox partitionOffset;
    private static EpiTextBox partitionDoorOpening;
    private static EpiTextBox numShelves;
    private static EpiTextBox openShelfSpace;
    
    private static EpiTextBox numSmallBins;
    private static EpiTextBox numMediumBins;
    private static EpiTextBox numLargeBins;
    private static EpiTextBox numBottles;
    private static EpiTextBox numDividers;
	
	public void InitializeCustomCode()
	{
		// Begin Wizard Added Variable Initialization

		this.edvPartRev = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));
		this.edvPartRev.EpiViewNotification += new EpiViewNotification(this.edvPartRev_EpiViewNotification);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls
		this.txtCopyPart.ReadOnly = true;
		this.txtCopyRev.ReadOnly = true;
		this.buttonCopy.ReadOnly = true;
		this.txtCurrentRevision.TextChanged += new System.EventHandler(this.txtCurrentRevision_TextChanged);
		this.buttonCopy.Click += new System.EventHandler(this.buttonCopy_Click);
		this.btnRetrieve.Click += new System.EventHandler(this.btnRetrieve_Click);
		this.ugdVehicle.ClickCell += new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.ugdVehicle_ClickCell);
		this.btnGetPartRev.Click += new System.EventHandler(this.btnGetPartRev_Click);
		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal
		this.txtCurrentRevision.TextChanged -= new System.EventHandler(this.txtCurrentRevision_TextChanged);
		this.btnRetrieve.Click -= new System.EventHandler(this.btnRetrieve_Click);
		this.edvPartRev.EpiViewNotification -= new EpiViewNotification(this.edvPartRev_EpiViewNotification);
		this.edvPartRev = null;
		this.btnGetPartRev.Click -= new System.EventHandler(this.btnGetPartRev_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal
        this.buttonCopy.Click -= new System.EventHandler(this.buttonCopy_Click);
		this.ugdVehicle.ClickCell -= new Infragistics.Win.UltraWinGrid.ClickCellEventHandler(this.ugdVehicle_ClickCell);
		// End Custom Code Disposal
	}

	private void buttonCopy_Click(object sender, System.EventArgs args)
	{ 
		//txtCopyPart = ((EpiTextBox)csm.GetNativeControlReference("d74c3905-a49e-42f4-a48d-e2a6a8b79379")); 
		//txtCopyRev = ((EpiTextBox)csm.GetNativeControlReference("31cbeabb-4107-4626-ae5d-66897b75a8d8")); 	
		this.partRevView = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));

		this.partRevAdapter = new PartRevSearchAdapter(this.oTrans);
		this.partRevAdapter.BOConnect();

	    string whereClause = "PartNum = '" + txtCopyPart.Text + "' AND RevisionNum = '" + txtCopyRev.Text + "'"; 
	    System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
	    whereClauses.Add("PartRev", whereClause);

	    SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
	    this.partRevAdapter.InvokeSearch(searchOptions);
	    
	    DataRow dataRow = this.partRevAdapter.PartRevSearchData.PartRev.Rows[0];
	    
		if(this.partRevAdapter.PartRevSearchData.PartRev.Count == 0) {
			MessageBox.Show("Filter has no rows!");
		}
		else {
			partRevView.dataView[this.partRevView.Row]["Height_c"] = dataRow["Height_c"];
			partRevView.dataView[this.partRevView.Row]["Width_c"] = dataRow["Width_c"];
			partRevView.dataView[this.partRevView.Row]["Depth_c"] = dataRow["Depth_c"];
			
			partRevView.dataView[this.partRevView.Row]["Weight_c"] = dataRow["Weight_c"];
			partRevView.dataView[this.partRevView.Row]["PrimaryMaterial_c"] = dataRow["PrimaryMaterial_c"];
			partRevView.dataView[this.partRevView.Row]["Color_c"] = dataRow["Color_c"];
			partRevView.dataView[this.partRevView.Row]["WeightCap_c"] = dataRow["WeightCap_c"];
			
			partRevView.dataView[this.partRevView.Row]["InstallTime_c"] = dataRow["InstallTime_c"];
			partRevView.dataView[this.partRevView.Row]["ReqInstallKit_c"] = dataRow["ReqInstallKit_c"];
			
			partRevView.dataView[this.partRevView.Row]["UsedWith_c"] = dataRow["UsedWith_c"];
			partRevView.dataView[this.partRevView.Row]["MaxLadderLength_c"] = dataRow["MaxLadderLength_c"];
			partRevView.dataView[this.partRevView.Row]["PartitionOffset_c"] = dataRow["PartitionOffset_c"];
			partRevView.dataView[this.partRevView.Row]["PartitionDoorOpening_c"] = dataRow["PartitionDoorOpening_c"];
			partRevView.dataView[this.partRevView.Row]["NumShelves_c"] = dataRow["NumShelves_c"];
			partRevView.dataView[this.partRevView.Row]["OpenShelfSpace_c"] = dataRow["OpenShelfSpace_c"];
			
			partRevView.dataView[this.partRevView.Row]["NumSmallBins_c"] = dataRow["NumSmallBins_c"];
			partRevView.dataView[this.partRevView.Row]["NumMediumBins_c"] = dataRow["NumMediumBins_c"];
			partRevView.dataView[this.partRevView.Row]["NumLargeBins_c"] = dataRow["NumLargeBins_c"];
			partRevView.dataView[this.partRevView.Row]["NumBottles_c"] = dataRow["NumBottles_c"];
			partRevView.dataView[this.partRevView.Row]["NumDividers_c"] = dataRow["NumDividers_c"];
			partRevView.dataView[this.partRevView.Row]["QtyBoxes_c"] = dataRow["QtyBoxes_c"];

			MessageBox.Show("Copy Successful!");
		}
	}

	private void edvPartRev_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		this.partRevView = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));
		
		if (this.partRevView.CurrentDataRow != null && (args.NotifyType == EpiTransaction.NotifyType.Initialize))
		{
			this.buttonCopy.ReadOnly = false;
			this.txtCopyPart.Text = "";
			this.txtCopyRev.Text = "";
		}
		else {
			this.buttonCopy.ReadOnly = true;
		}
	}

	private void txtCurrentRevision_TextChanged(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		this.ugdVehicle.DataSource = null;
	}

	private void btnRetrieve_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		EpiDataView edvPartRev = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));
		System.Data.DataRow edvPartRevRow = edvPartRev.CurrentDataRow;

		if ((edvPartRevRow != null))
		{
			FillVehicleGrid(edvPartRevRow["PartNum"].ToString(), edvPartRevRow["RevisionNum"].ToString());
		} //if ((edvPartRevRow != null))
	}

	private void ugdVehicle_ClickCell(object sender, Infragistics.Win.UltraWinGrid.ClickCellEventArgs args)
	{
		// ** Place Event Handling Code Here **
		switch (args.Cell.Column.Header.Caption)
        {
			case "Check":
					if ( ugdVehicle.ActiveRow != null && ugdVehicle.ActiveRow.IsDataRow)
					{
						UltraGridRow activeRow = this.ugdVehicle.ActiveRow;
						if (!ugdVehicle.ActiveRow.IsDataRow)
							return;

						if (!Convert.ToBoolean(activeRow.Cells["Calculated_Check"].Value))
						{
							CreateChild(activeRow.Cells["UD100_Key1"].Value.ToString(), activeRow.Cells["UD100_Key2"].Value.ToString(), activeRow.Cells["UD100_Key3"].Value.ToString(), activeRow.Cells["UD100_Key4"].Value.ToString(), activeRow.Cells["UD100_Key5"].Value.ToString());
						}else
						{
							DeleteChild(activeRow.Cells["UD100_Key1"].Value.ToString(), activeRow.Cells["UD100_Key2"].Value.ToString(), activeRow.Cells["UD100_Key3"].Value.ToString(), activeRow.Cells["UD100_Key4"].Value.ToString(), activeRow.Cells["UD100_Key5"].Value.ToString());
						}
					}
				break;
		} //switch (args.Cell.Column.Header.Caption)
	}

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
			
			ugdVehicle.DataSource = ad.QueryResults.Tables["Results"];
			
			ad.Dispose();

//			foreach(Infragistics.Win.UltraWinGrid.UltraGridColumn curColumn in ugdVehicle.DisplayLayout.Bands[0].Columns)
//			{
//				curColumn.Hidden = true;
//			}
//
//			//Unhide the "columns"	
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key1"].Hidden = false;
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key2"].Hidden = false;	
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key3"].Hidden = false;
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key4"].Hidden = false;
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key5"].Hidden = false;
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Number01"].Hidden = false;
//			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Number02"].Hidden = false;
//			ugdVehicle.DisplayLayout.Bands[0].Columns["Calculated_Check"].Hidden = false;

//			ugdVehicle.Rows.Band.Columns["Calculated_Check"].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.CheckBox;
//			
//			ugdVehicle.DisplayLayout.Override.SelectedAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;
//	        ugdVehicle.DisplayLayout.Override.ActiveAppearancesEnabled = Infragistics.Win.DefaultableBoolean.False;

			//Hide Columns
			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Company"].Hidden = true;
			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100A_ChildKey1"].Hidden = true;
			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100A_ChildKey2"].Hidden = true;
	
			//Set Format
			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Number01"].Format = "###,###,###,##0.0";
			ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Number02"].Format = "###,###,###,##0.0";
	
			//Disable Columns
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key1"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key2"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key3"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key4"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key5"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Number01"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;
			this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Number02"].CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled;

			//Disable BackColor Columns
			//this.ugdVehicle.DisplayLayout.Bands[0].Columns["UD100_Key1"].Appearance.BackColorDisabled = System.Drawing.Color.White;
//			args.Row.Cells["UD100_Key1"].Appearance.ForeColorDisabled = System.Drawing.Color.Black;
//			args.Row.Cells["UD100_Key2"].Appearance.BackColorDisabled = System.Drawing.Color.White;
//			args.Row.Cells["UD100_Key3"].Appearance.BackColorDisabled = System.Drawing.Color.White;
//			args.Row.Cells["UD100_Key4"].Appearance.BackColorDisabled = System.Drawing.Color.White;
//			args.Row.Cells["UD100_Key5"].Appearance.BackColorDisabled = System.Drawing.Color.White;
//			args.Row.Cells["UD100_Number01"].Appearance.BackColorDisabled = System.Drawing.Color.White;
//			args.Row.Cells["UD100_Number02"].Appearance.BackColorDisabled = System.Drawing.Color.White;

			foreach (UltraGridRow row in ugdVehicle.Rows)
			{
				row.Appearance.BackColorDisabled = System.Drawing.Color.White;
				row.Appearance.ForeColorDisabled = System.Drawing.Color.Black;
			}

		}
		catch (Exception ex)
		{
			//MessageBox.Show(ex.ToString());

		}
	}
	
	public void CreateChild(string vBrand, string vVehicle, string vYear, string vLength, string vHeight)
	{
		try
		{
			EpiDataView edvPartRev = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));
			System.Data.DataRow edvPartRevRow = edvPartRev.CurrentDataRow;

			if ((edvPartRevRow != null))
			{
				string vPartNum = edvPartRevRow["PartNum"].ToString();
				string vRevisionNum  = edvPartRevRow["RevisionNum"].ToString();

				//MessageBox.Show("vPartNum: " + vPartNum + "  vRevisionNum: " + vRevisionNum);

				UD100Adapter ud100AdapterN  = new UD100Adapter(PartForm);
				ud100AdapterN.BOConnect();

				//ud100AdapterN.GetNewUD100A(vBrand, vVehicle, vYear, vLength, vHeight, vPartNum, vRevisionNum, "");
				ud100AdapterN.GetByID(vBrand, vVehicle, vYear, vLength, vHeight);

				ud100AdapterN.GetaNewUD100a(vBrand, vVehicle, vYear, vLength, vHeight);
				int newRow = ud100AdapterN.UD100Data.UD100A.Rows.Count - 1;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["ChildKey1"] = vPartNum;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["ChildKey2"] = vRevisionNum;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["ChildKey3"] = "";
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["ChildKey4"] = "";	
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["ChildKey5"] = "";
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["Brand_c"] = vBrand;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["Model_c"] = vVehicle;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["Year_c"] = vYear;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["Length_c"] = vLength;	
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["Height_c"] = vHeight;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["PartNum_c"] = vPartNum;
				ud100AdapterN.UD100Data.UD100A.Rows[newRow]["RevisionNum_c"] = vRevisionNum;
				
				ud100AdapterN.Update();
				
				ud100AdapterN.Dispose();

				FillVehicleGrid(vPartNum, vRevisionNum);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());

		}
	}

	public void DeleteChild(string vBrand, string vVehicle, string vYear, string vLength, string vHeight)
	{
		try
		{
			EpiDataView edvPartRev = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));
			System.Data.DataRow edvPartRevRow = edvPartRev.CurrentDataRow;

			if ((edvPartRevRow != null))
			{
				string vPartNum = edvPartRevRow["PartNum"].ToString();
				string vRevisionNum  = edvPartRevRow["RevisionNum"].ToString();

				//MessageBox.Show("vPartNum: " + vPartNum + "  vRevisionNum: " + vRevisionNum);

				UD100Adapter ud100AdapterN  = new UD100Adapter(PartForm);
				ud100AdapterN.BOConnect();

				ud100AdapterN.GetByID(vBrand, vVehicle, vYear, vLength, vHeight);

				DataRow[] drsDeleted = ud100AdapterN.UD100Data.UD100A.Select("ChildKey1 = \'" + vPartNum + "\' and ChildKey2 = \'" + vRevisionNum + "\'");
				for (int i = 0; (i < drsDeleted.Length); i = (i + 1))
				{
					ud100AdapterN.Delete(drsDeleted[i]);
				}

				ud100AdapterN.Update();
				
				ud100AdapterN.Dispose();

				FillVehicleGrid(vPartNum, vRevisionNum);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());

		}
	}


	private void SearchOnPartRevSearchAdapterShowDialog()
	{
		// Wizard Generated Search Method
		// You will need to call this method from another method in custom code
		// For example, [Form]_Load or [Button]_Click

		bool recSelected;
		string whereClause = string.Empty;
		System.Data.DataSet dsPartRevSearchAdapter = Ice.UI.FormFunctions.SearchFunctions.listLookup(this.oTrans, "PartRevSearchAdapter", out recSelected, true, whereClause);
		txtCopyPart = ((EpiTextBox)csm.GetNativeControlReference("d74c3905-a49e-42f4-a48d-e2a6a8b79379")); 
		txtCopyRev = ((EpiTextBox)csm.GetNativeControlReference("31cbeabb-4107-4626-ae5d-66897b75a8d8")); 		
		
		if (recSelected)
		{
			System.Data.DataRow adapterRow = dsPartRevSearchAdapter.Tables[0].Rows[0];

			// Map Search Fields to Application Fields
			EpiDataView edvPart = ((EpiDataView)(this.oTrans.EpiDataViews["Part"]));
			txtCopyPart.Text = adapterRow["PartNum"].ToString();
			txtCopyRev.Text = adapterRow["RevisionNum"].ToString();
			
		}
	}

	private void btnGetPartRev_Click(object sender, System.EventArgs args)
	{
		SearchOnPartRevSearchAdapterShowDialog();
	}
}
