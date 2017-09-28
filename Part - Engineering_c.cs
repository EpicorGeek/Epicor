// **************************************************
// Custom code for PartForm
// Created: 8/21/2017 7:27:50 AM
// **************************************************

extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_PartPlantSearch;
extern alias Erp_Contracts_BO_PO;
extern alias Erp_Contracts_BO_PartOnHandWhse;
extern alias Erp_Contracts_BO_Vendor;
extern alias Erp_Contracts_BO_VendorPPSearch;

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

	private UD101Adapter _ud101Adapter;
	private EpiDataView _edvPartRev;
	private DataTable UD101_Column;
	private EpiDataView _edvUD101;
	private string _Key1UD101;
	private string _Key2UD101;
	private string _Key3UD101;
	private string _Key4UD101;
	private string _Key5UD101;
	private DataView PartRev_DataView;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		gridVehicles.ReadOnly = true;
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		//this.gridVehicle.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False;
		//gridVehicle.ReadOnly = true;
		InitializeUD101Adapter();
		this._Key1UD101 = string.Empty;
		this._Key2UD101 = string.Empty;
		this._Key3UD101 = string.Empty;
		this._Key4UD101 = string.Empty;
		this._Key5UD101 = string.Empty;
		this.baseToolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.baseToolbarsManager_ToolClickForUD101);
		this.PartForm.BeforeToolClick += new Ice.Lib.Framework.BeforeToolClickEventHandler(this.PartForm_BeforeToolClickForUD101);
		this.PartForm.AfterToolClick += new Ice.Lib.Framework.AfterToolClickEventHandler(this.PartForm_AfterToolClickForUD101);
		this.PartRev_Row.EpiRowChanged += new EpiRowChanged(this.PartRev_AfterRowChangeForUD101);
		this.PartRev_DataView = this.PartRev_Row.dataView;
		this.PartRev_DataView.ListChanged += new ListChangedEventHandler(this.PartRev_DataView_ListChangedForUD101);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		if ((this._ud101Adapter != null))
		{
			this._ud101Adapter.Dispose();
			this._ud101Adapter = null;
		}
		this._edvUD101 = null;
		this._edvPartRev = null;
		this.UD101_Column = null;
		this._Key1UD101 = null;
		this._Key2UD101 = null;
		this._Key3UD101 = null;
		this._Key4UD101 = null;
		this._Key5UD101 = null;
		this.baseToolbarsManager.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.baseToolbarsManager_ToolClickForUD101);
		this.PartForm.BeforeToolClick -= new Ice.Lib.Framework.BeforeToolClickEventHandler(this.PartForm_BeforeToolClickForUD101);
		this.PartForm.AfterToolClick -= new Ice.Lib.Framework.AfterToolClickEventHandler(this.PartForm_AfterToolClickForUD101);
		this.PartRev_Row.EpiRowChanged -= new EpiRowChanged(this.PartRev_AfterRowChangeForUD101);
		this.PartRev_DataView.ListChanged -= new ListChangedEventHandler(this.PartRev_DataView_ListChangedForUD101);
		this.PartRev_DataView = null;
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void InitializeUD101Adapter()
	{
		// Create an instance of the Adapter.
		this._ud101Adapter = new UD101Adapter(this.oTrans);
		this._ud101Adapter.BOConnect();

		// Add Adapter Table to List of Views
		// This allows you to bind controls to the custom UD Table
		this._edvUD101 = new EpiDataView();
		this._edvUD101.dataView = new DataView(this._ud101Adapter.UD101Data.UD101);
		this._edvUD101.AddEnabled = true;
		this._edvUD101.AddText = "New Vehicle";
		if ((this.oTrans.EpiDataViews.ContainsKey("UD101View") == false))
		{
			this.oTrans.Add("UD101View", this._edvUD101);
		}

		// Initialize DataTable variable
		this.UD101_Column = this._ud101Adapter.UD101Data.UD101;

		// Initialize EpiDataView field.
		this._edvPartRev = ((EpiDataView)(this.oTrans.EpiDataViews["PartRev"]));

		// Set the parent view / keys for UD child view
		string[] parentKeyFields = new string[2];
		string[] childKeyFields = new string[2];
		parentKeyFields[0] = "PartNum";
		childKeyFields[0] = "Key1";
		parentKeyFields[1] = "RevisionNum";
		childKeyFields[1] = "Key2";
		this._edvUD101.SetParentView(this._edvPartRev, parentKeyFields, childKeyFields);

		if ((this.oTrans.PrimaryAdapter != null))
		{
			// this.oTrans.PrimaryAdapter.GetCurrentDataSet(Ice.Lib.Searches.DataSetMode.RowsDataSet).Tables.Add(this._edvUD101.dataView.Table.Clone())
		}

	}

	private void GetUD101Data(string key1, string key2, string key3, string key4, string key5)
	{
		if ((this._Key1UD101 != key1) || (this._Key2UD101 != key2) || (this._Key3UD101 != key3) || (this._Key4UD101 != key4) || (this._Key5UD101 != key5))
		{
			// Build where clause for search.
            // Evan
			string whereClause = "Key1 = \'" + key1 + "\' And Key2 = \'" + key2 + "\'";    /* + "\' And Key3 = \'" + key3 + "\' And Key4 = \'" + key4 + "\'";*/
			System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
			whereClauses.Add("UD101", whereClause);

			// Call the adapter search.
			SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
			this._ud101Adapter.InvokeSearch(searchOptions);

			if ((this._ud101Adapter.UD101Data.UD101.Rows.Count > 0))
			{
				this._edvUD101.Row = 0;
			} else
			{
				this._edvUD101.Row = -1;
			}

			// Notify that data was updated.
			this._edvUD101.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD101.Row, this._edvUD101.Column));

			// Set key fields to their new values.
			this._Key1UD101 = key1;
			this._Key2UD101 = key2;
			this._Key3UD101 = key3;
			this._Key4UD101 = key4;
			this._Key5UD101 = key5;
		}
	}

	private void GetNewUD101Record()
	{
		DataRow parentViewRow = this._edvPartRev.CurrentDataRow;
		// Check for existence of Parent Row.
		if ((parentViewRow == null))
		{
			return;
		}
		if (this._ud101Adapter.GetaNewUD101())
		{
			string partnum = parentViewRow["PartNum"].ToString();
			string revisionnum = parentViewRow["RevisionNum"].ToString();

			// Get unique row count id for Key5
			int rowCount = this._ud101Adapter.UD101Data.UD101.Rows.Count;
			int lineNum = rowCount;
			bool goodIndex = false;
			while ((goodIndex == false))
			{
				// Check to see if index exists
				DataRow[] matchingRows = this._ud101Adapter.UD101Data.UD101.Select("Key5 = \'" + lineNum.ToString() + "\'");
				if ((matchingRows.Length > 0))
				{
					lineNum = (lineNum + 1);
				} else
				{
					goodIndex = true;
				}
			}

			// Set initial UD Key values
			DataRow editRow = this._ud101Adapter.UD101Data.UD101.Rows[(rowCount - 1)];
			editRow.BeginEdit();
			editRow["Key1"] = partnum;
			editRow["Key2"] = revisionnum;
			editRow["Key3"] = string.Empty;
			editRow["Key4"] = string.Empty;
			editRow["Key5"] = string.Empty;
			//editRow["Key5"] = lineNum.ToString();
			editRow.EndEdit();

			// Notify that data was updated.
			this._edvUD101.Notify(new EpiNotifyArgs(this.oTrans, (rowCount - 1), this._edvUD101.Column));
		}
	}

	private void SaveUD101Record()
	{
		// Save adapter data
		this._ud101Adapter.Update();
	}

	private void DeleteUD101Record()
	{
		// Check to see if deleted view is ancestor view
		bool isAncestorView = false;
		Ice.Lib.Framework.EpiDataView parView = this._edvUD101.ParentView;
		while ((parView != null))
		{
			if ((this.oTrans.LastView == parView))
			{
				isAncestorView = true;
				break;
			} else
			{
				parView = parView.ParentView;
			}
		}

		// If Ancestor View then delete all child rows
		if (isAncestorView)
		{
			DataRow[] drsDeleted = this._ud101Adapter.UD101Data.UD101.Select("Key1 = \'" + this._Key1UD101 + "\' AND Key2 = \'" + this._Key2UD101 + "\' AND Key3 = \'" + this._Key3UD101 + "\' AND Key4 = \'" + this._Key4UD101 + "\'");
			for (int i = 0; (i < drsDeleted.Length); i = (i + 1))
			{
				this._ud101Adapter.Delete(drsDeleted[i]);
			}
		} else
		{
			if ((this.oTrans.LastView == this._edvUD101))
			{
				if ((this._edvUD101.Row >= 0))
				{
					DataRow drDeleted = ((DataRow)(this._ud101Adapter.UD101Data.UD101.Rows[this._edvUD101.Row]));
					if ((drDeleted != null))
					{
						if (this._ud101Adapter.Delete(drDeleted))
						{
							if ((_edvUD101.Row > 0))
							{
								_edvUD101.Row = (_edvUD101.Row - 1);
							}

							// Notify that data was updated.
							this._edvUD101.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD101.Row, this._edvUD101.Column));
						}
					}
				}
			}
		}
	}

	private void UndoUD101Changes()
	{
		this._ud101Adapter.UD101Data.RejectChanges();

		// Notify that data was updated.
		this._edvUD101.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD101.Row, this._edvUD101.Column));
	}

	private void ClearUD101Data()
	{
		this._Key1UD101 = string.Empty;
		this._Key2UD101 = string.Empty;
		this._Key3UD101 = string.Empty;
		this._Key4UD101 = string.Empty;
		this._Key5UD101 = string.Empty;

		this._ud101Adapter.UD101Data.Clear();

		// Notify that data was updated.
		this._edvUD101.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD101.Row, this._edvUD101.Column));
	}

	private void baseToolbarsManager_ToolClickForUD101(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs args)
	{
		// EpiMessageBox.Show(args.Tool.Key);
		switch (args.Tool.Key)
		{
			case "EpiAddNewNew Vehicle":
				GetNewUD101Record();
				break;

			case "ClearTool":
				ClearUD101Data();
				break;

			case "UndoTool":
				UndoUD101Changes();
				break;
		}
	}

	private void PartForm_BeforeToolClickForUD101(object sender, Ice.Lib.Framework.BeforeToolClickEventArgs args)
	{
		// EpiMessageBox.Show(args.Tool.Key);
		switch (args.Tool.Key)
		{
			case "SaveTool":
				SaveUD101Record();
				break;
		}
	}

	private void PartForm_AfterToolClickForUD101(object sender, Ice.Lib.Framework.AfterToolClickEventArgs args)
	{
		// EpiMessageBox.Show(args.Tool.Key);
		switch (args.Tool.Key)
		{
			case "DeleteTool":
            // Evan
				/*if ((args.Cancelled == false))
				{
					EpiMessageBox.Show("Delete Launch");
					DeleteUD101Record();
				}*/
				DeleteUD101Record();
				break;
		}
	}

	private void PartRev_AfterRowChangeForUD101(EpiRowChangedArgs args)
	{
		// ** add AfterRowChange event handler
		string partnum = args.CurrentView.dataView[args.CurrentRow]["PartNum"].ToString();
		string revisionnum = args.CurrentView.dataView[args.CurrentRow]["RevisionNum"].ToString();
		GetUD101Data(partnum, revisionnum, string.Empty, string.Empty, string.Empty);
	}

	private void PartRev_DataView_ListChangedForUD101(object sender, ListChangedEventArgs args)
	{
		// ** add ListChanged event handler
		string partnum = PartRev_DataView[0]["PartNum"].ToString();
		string revisionnum = PartRev_DataView[0]["RevisionNum"].ToString();
		GetUD101Data(partnum, revisionnum, string.Empty, string.Empty, string.Empty);
	}
}
