
/********** Default Values **********/

// On EpiViewNotification AddRow
private void custDataView_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if ((args.NotifyType == EpiTransaction.NotifyType.AddRow))
		{
			DataRow currentCustomer = this.custDataView.CurrentDataRow;
			if(this.custDataView.CurrentDataRow != null){
				this.btnNextCustID.Enabled = true;
				if(this.custDataView.CurrentDataRow.RowState.ToString() == "Added")
				{
					this.custDataView.CurrentDataRow["CheckDuplicatePO"] = true;
				}
			}
			else{
				this.btnNextCustID.Enabled = false;
			}
			
		}
	}
    
    
// On EpiViewNotification Initialize and RowState == Added

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