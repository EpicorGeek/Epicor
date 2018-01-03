// **************************************************
// Custom code for UD05Form
// Created: 12/11/2017 1:24:35 PM
// **************************************************
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Ice.BO;
using Ice.UI;
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
	private EpiDataView edvUD05;
	
	private EpiBaseAdapter oTrans_adapter;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		this.edvUD05 = ((EpiDataView)(this.oTrans.EpiDataViews["UD05"]));

		this.oTrans_adapter = ((EpiBaseAdapter)(this.csm.TransAdaptersHT["oTrans_adapter"]));
		this.oTrans_adapter.AfterAdapterMethod += new AfterAdapterMethod(this.oTrans_adapter_AfterAdapterMethod);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal
		this.edvUD05 = null;
	
		this.oTrans_adapter.AfterAdapterMethod -= new AfterAdapterMethod(this.oTrans_adapter_AfterAdapterMethod);
		this.oTrans_adapter = null;
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void oTrans_adapter_AfterAdapterMethod(object sender, AfterAdapterMethodArgs args)
	{
		// ** Argument Properties and Uses **
		// ** args.MethodName **
		// ** Add Event Handler Code **

		// ** Use MessageBox to find adapter method name
		// EpiMessageBox.Show(args.MethodName)
		switch (args.MethodName)
		{
			case "Update":
				DataRow currentRow = this.edvUD05.CurrentDataRow;
				currentRow["Area_c"] = currentRow["Key1"];
				break;
		}

	}
}
