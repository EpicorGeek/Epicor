// **************************************************
// Custom code for MainController
// Created: 8/23/2017 9:17:18 AM
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

	private EpiDataView edvV_PartRev_Vehicle_1View;
    
    private static EpiTextBox part;
    private static EpiTextBox revision;
    private static EpiTextBox vehicleid;
    private static EpiTextBox brand;
    private static EpiTextBox vehicle;
    private static EpiTextBox year;
    private static EpiTextBox model;
	// End Wizard Added Module Level Variables **

	// Add Custom Module Level Variables Here **

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization

		this.edvV_PartRev_Vehicle_1View = ((EpiDataView)(this.oTrans.EpiDataViews["V_PartRev_Vehicle_1View"]));
		this.edvV_PartRev_Vehicle_1View.EpiViewNotification += new EpiViewNotification(this.edvV_PartRev_Vehicle_1View_EpiViewNotification);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		// End Wizard Added Custom Method Calls
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		this.edvV_PartRev_Vehicle_1View.EpiViewNotification -= new EpiViewNotification(this.edvV_PartRev_Vehicle_1View_EpiViewNotification);
		this.edvV_PartRev_Vehicle_1View = null;
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		// End Custom Code Disposal
	}


	private void edvV_PartRev_Vehicle_1View_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if ((args.Sender.ToString() == "Ice.Lib.Framework.BAQDataView") && (args.NotifyType == EpiTransaction.NotifyType.Initialize))
		{
            part = ((EpiTextBox)csm.GetNativeControlReference("ed6ec35a-0be1-4dcd-89eb-5308ac6794f1")); 
            revision = ((EpiTextBox)csm.GetNativeControlReference("c1aa85d6-8d70-4496-955b-7daf20859ce2")); 
            vehicleid = ((EpiTextBox)csm.GetNativeControlReference("7d32ffc7-c6fc-4973-9fd9-c6c252231c65")); 
            brand = ((EpiTextBox)csm.GetNativeControlReference("4da113e1-62c5-460c-b2ba-a2c30f926bcc")); 
            vehicle = ((EpiTextBox)csm.GetNativeControlReference("598f1ee6-03ca-4fb8-b233-2d570e3190ef")); 
            year = ((EpiTextBox)csm.GetNativeControlReference("dfcbd85f-77fe-40cb-8c39-0eda0915d335")); 
            model = ((EpiTextBox)csm.GetNativeControlReference("5409ba3e-abd9-456d-9b5a-bba146bc22cc")); 
			if ((args.Row < 0) && (part.Text != "" || revision.Text != "" || vehicleid.Text != "" || brand.Text != "" || vehicle.Text != "" || year.Text != "" || model.Text != ""))
			{
				MessageBox.Show("No Results!");
			}
		}
	}
}
