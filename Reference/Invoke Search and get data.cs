
/********** BO Adapter  - Invoke Search **********/
EpiDataView edvCustomer = ((EpiDataView)(this.oTrans.EpiDataViews["Distributor"]));	
DataRow drCustomer = edvCustomer.CurrentDataRow;
string distNum = drCustomer["CustNum"].ToString();

CustCntAdapter adapterCustCnt = new CustCntAdapter(this.oTrans);
adapterCustCnt.BOConnect();

string whereClause = "CustNum =  " + distNum + " AND LeadContact_c = true";
SearchOptions opts = new SearchOptions(SearchMode.AutoSearch);
opts.NamedSearch.WhereClauses.Add("CustCnt", whereClause);

bool morePages = false;
DataSet dsLeadContact = adapterCustCnt.GetRows(opts, out morePages);
if(dsLeadContact.Tables["CustCnt"].Rows.Count>0)
{
MessageBox.Show("DataSet: " + dsLeadContact.Tables["CustCnt"].Rows.Count.ToString());		
}


    
/****************OPTIONS****************/
// Get Data Row
    DataRow dataRow = this.adapterName.TableNameData.TableName.Rows[row index];
    dataRow["Column Name"]
   
    or
   
// SELECT Data Row
    DataRow[] copyRows = ud100Adapter.UD100Data.UD100A.Select("ChildKey1 = \'" + txtCopyPart.Text.ToString() + "\' and ChildKey2 = \'" + txtCopyRev.Text.ToString() + "\'");
/********************************************/

                    
                    

                    
                    
                    
/********** BO Adapter  - GetByID Search **********/
// Set BO adapter
this.partRevAdapter = new PartRevSearchAdapter(this.oTrans);
this.partRevAdapter.BOConnect();

// Get Data
this.partRevAdapter.GetByID(txtCopyPart.Text, txtCopyRev.Text, "");

// Get Row
DataRow dataRow = this.partRevAdapter.PartRevSearchData.PartRev.Rows[0];

                    
                    

                    
                    
                
/********** Simple Search Function List Lookup **********/
    bool recSelected;
    string whereClause = string.Empty;
    System.Data.DataSet dsCustomerAdapter = Ice.UI.FormFunctions.SearchFunctions.listLookup(this.oTrans, "CustomerAdapter", out recSelected, true, whereClause);
    if (recSelected)
    {
        System.Data.DataRow adapterRow = dsCustomerAdapter.Tables[0].Rows[0];

        // Map Search Fields to Application Fields
        EpiDataView edvCustomer = ((EpiDataView)(this.oTrans.EpiDataViews["Customer"]));
        System.Data.DataRow edvCustomerRow = edvCustomer.CurrentDataRow;
        if ((edvCustomerRow != null))
        {
            edvCustomerRow.BeginEdit();
            edvCustomerRow["ParentCustNum_c"] = adapterRow["CustNum"];
            edvCustomerRow["ParentCustID_c"] = adapterRow["CustID"];
            edvCustomerRow.EndEdit();
            
        }
    }