// **************************************************
// Custom code for CRMCallForm
// Created: 7/25/2017 10:30:36 AM
// **************************************************

extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_Vendor;

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

	private EpiBaseAdapter oTrans_adapter;
	private EpiDataView edvCRMCall;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **
	private static EpiCombo callType;
	private static EpiCombo inquiryType;
	private static EpiCombo points;
	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		this.oTrans_adapter = ((EpiBaseAdapter)(this.csm.TransAdaptersHT["oTrans_adapter"]));
		this.oTrans_adapter.BeforeAdapterMethod += new BeforeAdapterMethod(this.oTrans_adapter_BeforeAdapterMethod);
		this.edvCRMCall = ((EpiDataView)(this.oTrans.EpiDataViews["CRMCall"]));

		callType = ((EpiCombo)csm.GetNativeControlReference("aa40b07c-65e2-4cec-bc2a-1fc794aee8cb"));
		inquiryType = ((EpiCombo)csm.GetNativeControlReference("25e32f3c-cd04-4c18-8674-3b58b309078f"));
		points = ((EpiCombo)csm.GetNativeControlReference("f16c1292-c473-4962-b4ae-940542c38ea0"));
		this.CRMCall_Column.ColumnChanged += new DataColumnChangeEventHandler(this.CRMCall_AfterFieldChange);
		this.edvCRMCall.EpiViewNotification += new EpiViewNotification(this.edvCRMCall_EpiViewNotification);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.oTrans_adapter.BeforeAdapterMethod -= new BeforeAdapterMethod(this.oTrans_adapter_BeforeAdapterMethod);
		this.oTrans_adapter = null;
		this.edvCRMCall = null;

		callType = null;
		inquiryType = null;
		points = null;
		this.CRMCall_Column.ColumnChanged -= new DataColumnChangeEventHandler(this.CRMCall_AfterFieldChange);
		this.edvCRMCall.EpiViewNotification -= new EpiViewNotification(this.edvCRMCall_EpiViewNotification);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}

	private void oTrans_adapter_BeforeAdapterMethod(object sender, BeforeAdapterMethodArgs args)
	{
		// ** Argument Properties and Uses **
		// ** args.MethodName **
		// ** Add Event Handler Code **
	
		// ** Use MessageBox to find adapter method name
		// EpiMessageBox.Show(args.MethodName)
		switch (args.MethodName)
		{
			case "Update":
				if(edvCRMCall.CurrentDataRow != null){
					if(edvCRMCall.CurrentDataRow["CallDesc"].ToString().Contains("CS - New") && String.IsNullOrEmpty(edvCRMCall.CurrentDataRow["InquiryType_c"].ToString()) ) {
						throw new UIException("Vehicle Type is required for CS - New");
					}
				}
				break;
		}
	
	}

	private void CRMCall_AfterFieldChange(object sender, DataColumnChangeEventArgs args)
	{
		// ** Argument Properties and Uses **
		// args.Row["FieldName"]
		// args.Column, args.ProposedValue, args.Row
		// Add Event Handler Code
		switch (args.Column.ColumnName)
		{
			case "CallTypeCode":
				if(edvCRMCall.CurrentDataRow != null){
					/*if(!edvCRMCall.CurrentDataRow["CallTypeCode"].ToString().Contains("CATY001")) {
						edvCRMCall.CurrentDataRow["Points_c"] = "1";
					}*/
					edvCRMCall.CurrentDataRow["Points_c"] = "1";
				}
				break;
		}
	}

	private void edvCRMCall_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if(args.NotifyType == EpiTransaction.NotifyType.Initialize && edvCRMCall.CurrentDataRow != null)
		{
			if(edvCRMCall.CurrentDataRow.RowState.ToString() == "Added" && String.IsNullOrEmpty(edvCRMCall.CurrentDataRow["CallTypeCode"].ToString())) 
			{
				edvCRMCall.CurrentDataRow["CallTypeCode"] = "Call";
			}
		}	
	}
}
