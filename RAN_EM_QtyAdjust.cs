// **************************************************
// Custom code for InventoryQtyAdjustForm
// Created: 2/15/2018 7:17:10 AM
// **************************************************
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

public class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **
	private EpiDataView edvview;
	private EpiDataView edvClient;	
	private DataView inventoryQtyAdjBrwView_DataView;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		this.edvview = ((EpiDataView)(this.oTrans.EpiDataViews["view"]));
		this.edvClient = ((EpiDataView)(this.oTrans.EpiDataViews["CallContextClientData"]));

		this.inventoryQtyAdjBrwView_DataView = this.inventoryQtyAdjBrwView_Row.dataView;
		this.inventoryQtyAdjBrwView_DataView.ListChanged += new ListChangedEventHandler(this.inventoryQtyAdjBrwView_DataView_ListChanged);
		this.InventoryQtyAdj_Column.ColumnChanged += new DataColumnChangeEventHandler(this.InventoryQtyAdj_AfterFieldChange);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal
		this.edvview = null;
		this.edvClient = null;

		this.inventoryQtyAdjBrwView_DataView.ListChanged -= new ListChangedEventHandler(this.inventoryQtyAdjBrwView_DataView_ListChanged);
		this.inventoryQtyAdjBrwView_DataView = null;
		this.InventoryQtyAdj_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.InventoryQtyAdj_AfterFieldChange);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}



	private void SetCost(string partNum)
	{
		try
		{
			// Declare and Initialize EpiDataView Variables
			// Declare and create an instance of the Adapter.

			PartCostSearchAdapter adapterPartCostSearch = new PartCostSearchAdapter(this.oTrans);
			adapterPartCostSearch.BOConnect();
			
			// Declare and Initialize Variables
			// TODO: You may need to replace the default initialization with valid values as required for the BL method call.
			System.Guid guidID = System.Guid.Empty;

			// Call Adapter method
			string site = edvClient.dataView[edvClient.Row]["CurrentPlant"].ToString();
			bool result = adapterPartCostSearch.GetByID(partNum, site);
			
			if(result) {
				this.UnitCost.Text = adapterPartCostSearch.PartCostSearchData.PartCost.Rows[0]["AvgMaterialCost"].ToString();
			}

			// Cleanup Adapter Reference
			adapterPartCostSearch.Dispose();

		} catch (System.Exception ex)
		{
			ExceptionBox.Show(ex);
		}
	}

	private void SetExtendedCost(string quantity) {
        try {
            double extCost = Convert.ToDouble(this.UnitCost.Text) * Convert.ToDouble(quantity);
            this.ExtendedCost.Text = extCost.ToString();
        } catch (System.Exception ex)
		{
			ExceptionBox.Show(ex);
		}
        
    }


	private void inventoryQtyAdjBrwView_DataView_ListChanged(object sender, ListChangedEventArgs args)
	{
		// ** Argument Properties and Uses **
		// inventoryQtyAdjBrwView_DataView[0]["FieldName"]
		// args.ListChangedType, args.NewIndex, args.OldIndex
		// ListChangedType.ItemAdded, ListChangedType.ItemChanged, ListChangedType.ItemDeleted, ListChangedType.ItemMoved, ListChangedType.Reset
		// Add Event Handler Code		
		if(edvview != null){
			string partNum = edvview.dataView[edvview.Row]["PartNum"].ToString();
            string quantity = edvview.dataView[edvview.Row]["AdjustQuantity"].ToString();
			SetCost(partNum);
            SetExtendedCost(quantity);
		}
		
	}

	private void InventoryQtyAdj_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code
		string quantity = edvview.dataView[edvview.Row]["AdjustQuantity"].ToString();
		switch (args.Column.ColumnName)
		{
			case "AdjustQuantity":	
				SetExtendedCost(quantity);
				break;
			case "UnitOfMeasure":
				SetExtendedCost(quantity);
				break;
		}
	}
}
