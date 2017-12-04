private void LoadOrderRelPerVinID()
	{
		if (_edvUD03.dataView[_edvUD03.Row]["VinID_c"] != null)
		{
			var orderNum = (int)edvOrderHed.dataView[edvOrderHed.Row]["OrderNum"];
			var vinId = (Guid)_edvUD03.dataView[_edvUD03.Row]["VinID_c"];

			using (DynamicQueryAdapter baq = new DynamicQueryAdapter(oTrans))
			{
				baq.BOConnect();
	
				var ds = new QueryExecutionDataSet();
				ds.ExecutionParameter.AddExecutionParameterRow("OrderNum", orderNum.ToString(), "int", false, Guid.Empty, "A");
				ds.ExecutionParameter.AddExecutionParameterRow("VinID", vinId.ToString("D"), "uniqueidentifier", false, Guid.Empty, "A");
	
				baq.ExecuteByID("RAN_GetOrderRelPerVinID", ds);
	
				dtOrderRelPerVinID = baq.QueryResults.Tables["Results"];
	
				grdPartAssociations.DataSource = dtOrderRelPerVinID;
				grdPartAssociations.DataBind();
	
				try
				{
					for (int i = 0; i < 4; i++)
					{
						grdPartAssociations.DisplayLayout.Bands[0].Columns[i].CellActivation = Activation.NoEdit;
					}
	
					for (int i = 2; i < 5; i++)
					{
						grdPartAssociations.DisplayLayout.Bands[0].Columns[i].Style = Infragistics.Win.UltraWinGrid.ColumnStyle.Integer;
						grdPartAssociations.DisplayLayout.Bands[0].Columns[i].PromptChar = ' ';
					}
				}
				catch { }
			}
		}
	}