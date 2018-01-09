#region Comments
// **************************************************
// Custom code for SalesOrderForm
// **************************************************
// Version 1.0.0 ECS HLalumiere 2017-08-21
// SalesOrderEntry.SalesOrderEntry
// RANGER-17-002
// **************************************************
// Version 1.2.1 ECS FVB 2018-01-09
// SalesOrderEntry.SalesOrderEntry
// RANGER-17-002
// Change the display condition on the RO of the release tab.
// **************************************************
#endregion

#region Usings
extern alias Erp_Contracts_BO_AlternatePart;
extern alias Erp_Contracts_BO_SalesOrder;
extern alias Erp_Contracts_BO_Quote;
extern alias Erp_Contracts_BO_Part;
extern alias Erp_Contracts_BO_Customer;
extern alias Erp_Contracts_BO_RMAProc;
extern alias Erp_Contracts_BO_OrderDtlSearch;
extern alias Erp_Contracts_BO_OrderHist;
extern alias Erp_Contracts_BO_QuoteDtlSearch;
extern alias Erp_Contracts_BO_SerialNumberSearch;
extern alias Erp_Contracts_BO_ShipTo;
extern alias Erp_Contracts_BO_MiscChrg;
extern alias Ice_Contracts_BO_UD02;
extern alias Erp_Contracts_BO_CustGrup;
extern alias Erp_Contracts_BO_ProdCal;
extern alias Erp_Contracts_BO_ReservePri;
extern alias Erp_Contracts_BO_Plant;
extern alias Ice_Contracts_BO_DynamicQuery;
extern alias Ice_Adapters_UD100;
extern alias Ice_Contracts_BO_UD100;

using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Ice.Lib.Customization;
using Ice.Lib.ExtendedProps;
using Ice.Lib.Framework;
using Ice.Lib.Searches;
using Ice.UI.FormFunctions;

// Add Reference to Ice.Core.Session

using Ice.Lib.Adapters;
using ECS.ToolsKit;
using ECS.ToolsKit.Extensions;
using Ice.Lib;
using System.Collections;
using System.Collections.Generic;
using Infragistics.Win.UltraWinGrid;
using Infragistics.Win;

using Ice.Adapters;
using Ice.Adapters.Controls;
using Ice.BO;
using Ice.Proxy.BO;
using Ice.Contracts;
using Erp.Adapters;
using Erp.BO;
using Erp.Proxy.BO;
using Erp.Contracts;
#endregion

namespace ECS.SalesOrderEntry
{
using ECS.ToolsKit;

	public class SalesOrderEntry : CustoBase<EpiTransaction>
	{
		
#region Variables Definitions
#endregion

#region Initialization / Clean-up
		public SalesOrderEntry(Script script) : base(script)
		{
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override void InitClass()
		{
		}
#endregion

#region Codes
#endregion

#region Events
#endregion
	}

}

namespace ECS.ToolsKit
{
    public abstract class CustoBase<T> : IDisposable where T : EpiTransaction
    {
        private object script;
        private Type tScript;
        private CustomScriptManager _csm;
        private T _oTrans;
        private Infragistics.Win.UltraWinToolbars.UltraToolbarsManager _baseToolbarsManager;

        private List<IEpiAdapter> adapters;
        private List<object> bos;

        protected CustoBase(object script)
        {
            this.script = script;
            tScript = this.script.GetType();
            _csm = this.GetField<CustomScriptManager>("csm");
            _oTrans = this.GetField<T>("oTrans");
            _baseToolbarsManager = this.GetField<Infragistics.Win.UltraWinToolbars.UltraToolbarsManager>("baseToolbarsManager");
        }

        public virtual void Dispose()
        {
            this.UnloadAdapters();
            this.UnloadBOs();
            this.script = null;
            this.tScript = null;
            this._csm = null;
            this._oTrans = null;
            this._baseToolbarsManager = null;
        }

		public virtual void InitClass() { }
		
		protected TAdp GetFormAdapter<TAdp>(string adpname) where TAdp : EpiBaseAdapter
		{
			return (TAdp)this.csm.TransAdaptersHT[adpname];
		}

		protected EpiBaseAdapter GetFormAdapter(string adpname)
		{
			return (EpiBaseAdapter)this.csm.TransAdaptersHT[adpname];
		}
		
        protected TField GetField<TField>(string fieldname)
        {
            var field = this.tScript
                            .GetField(fieldname, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field == null) throw new Exception(string.Format("The field {0} ({1}) could not be found. Be sure the name is exact and the field exist.", fieldname, typeof(T)));
            return (TField)field.GetValue(this.script);
        }

        protected TControl GetNativeControlReference<TControl>(string epiGuid) where TControl : System.Windows.Forms.Control
        {
            return (TControl)csm.GetNativeControlReference(epiGuid);
        }

        protected CustomScriptManager csm
        {
            get { return this._csm; }
        }

        protected T oTrans
        {
            get { return this._oTrans; }
        }

        protected Ice.Core.Session Session
        {
            get { return (Ice.Core.Session)this.oTrans.Session; }
        }

        protected object Script
        {
            get { return this.script; }
        }

        protected Infragistics.Win.UltraWinToolbars.UltraToolbarsManager baseToolbarsManager
        {
            get { return this._baseToolbarsManager; }
        }

        protected bool InCustomizationMode
        {
            get
            {
                Infragistics.Win.UltraWinToolbars.ToolBase tb = baseToolbarsManager.Tools["CustomizeTool"];

                if (tb == null) return false; // le menu n'existe pas, on n'est pas en mode custo.

                return tb.VisibleResolved; // Si le menu n'est pas visible, on ne devrait pas être en mode custo.
            }
        }

        protected void HideToolBars()
        {
            foreach (Infragistics.Win.UltraWinToolbars.UltraToolbar t in baseToolbarsManager.Toolbars)
            {
                t.Visible = false;
            }
        }

        protected void HideNativeControl(string epiGuid, bool preservetabstop = false)
        {
            System.Windows.Forms.Control ctrl = this.GetNativeControlReference<System.Windows.Forms.Control>(epiGuid);
            ctrl.Visible = false;
            ctrl.Location = new System.Drawing.Point(ctrl.Location.X, Math.Abs(ctrl.Location.Y) * -1); // Keep the X position but put Y in the negative to keep the original value and hide the control;
			if(preservetabstop == false)
			{
				ctrl.TabStop = false;
			}
        }

		protected void SetStatusText(string status, bool busy)
		{
		var t = this.oTrans.GetType();
		var m = t.GetMethod("setStatusText", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);

			m.Invoke(this.oTrans, new object[] {status, busy});
		}

#region Adapter / BO Repository functions
        protected T GetAdapter<T>() where T : class
        {
            // ne pas oublier de créer la variable
            if (adapters == null)
            {
                adapters = new List<IEpiAdapter>();
            }

            //// on vérifie si on a déjà une instance de T
            // OfType<T>() ne fonctionne pas puisque la fonction
            // se trouve dans System.Core.dll.
            //T result = adapters.OfType<T>().FirstOrDefault();
            T result = null;
            if (adapters.Count > 0)
            {
                foreach (IEpiAdapter i in adapters) // on prend le premier objet de type T.
                {
                    result = i as T;
                    if (result != null) break;
                }
            }

            // si result = null, l'adapter voulu n'a pas encore été instancié
            if (result == null)
            {
                result = this.oTrans.GetNewAdapter<T>();
                adapters.Add(result as IEpiAdapter);
            }

            return result;
        }

        protected void UnloadAdapters()
        {
            if (adapters == null) return;

            // ToArray() permet de faire une copie de la collection. 
            // Une exception survient si on fait un Remove dans la 
            // collection sur laquelle on fait un foreach. 
            foreach (IEpiAdapter o in adapters.ToArray())
            {
                IDisposable d = o as IDisposable;
                if (d != null) d.Dispose();

                adapters.Remove(o);
            }

            adapters = null;
        }

        protected T GetBO<T, TContract>()
            where T : Epicor.ServiceModel.Channels.ImplBase<TContract>
            where TContract : class
        {
            // ne pas oublier de créer la variable
            if (bos == null)
            {
                bos = new List<object>();
            }

            //// on vérifie si on a déjà une instance de T
            // OfType<T>() ne fonctionne pas puisque la fonction
            // se trouve dans System.Core.dll.
            //T result = adapters.OfType<T>().FirstOrDefault();
            T result = null;
            if (bos.Count > 0)
            {
                foreach (object i in bos) // on prend le premier objet de type T.
                {
                    result = i as T;
                    if (result != null) break;
                }
            }

            // si result = null, l'adapter voulu n'a pas encore été instancié
            if (result == null)
            {
                result = this.oTrans.GetNewBO<T, TContract>();
                bos.Add(result);
            }

            return result;
        }

        protected void UnloadBOs()
        {
            if (bos == null) return;

            // ToArray() permet de faire une copie de la collection. 
            // Une exception survient si on fait un Remove dans la 
            // collection sur laquelle on fait un foreach. 
            foreach (object o in bos.ToArray())
            {
                IDisposable d = o as IDisposable;
                if (d != null) d.Dispose();

                bos.Remove(o);
            }

            bos = null;
        }

#endregion
    }
}

namespace ECS.ToolsKit.Extensions
{
	public static class IEpiTransactionExt
	{
        public static T GetNewAdapter<T>(this IEpiTransaction trans) where T : class
        {
            T result = (T)Activator.CreateInstance(typeof(T), trans);
            var adapter = result as IEpiAdapter; // permet d'exposer les propriétés de IEpiAdapter

            adapter.BOConnect();

            return result;
        }

        public static T GetNewBO<T, TContract>(this IEpiTransaction trans)
            where T : Epicor.ServiceModel.Channels.ImplBase<TContract>
            where TContract : class
		{
			return (trans.Session as Ice.Core.Session).GetNewBO<T, TContract>();
		}
	}

	public static class SessionExt
	{
        public static T GetNewBO<T, TContract>(this Ice.Core.Session session)
            where T : Epicor.ServiceModel.Channels.ImplBase<TContract>
            where TContract : class
        {
            //return (T)Activator.CreateInstance(typeof(T), ((Ice.Core.Session)oTrans.Session).ConnectionPool);
            return WCFServiceSupport.CreateImpl<T>(session, Epicor.ServiceModel.Channels.ImplBase<TContract>.UriPath);
        }
	}

	public static class ListExt
	{
		public static List<T> OrderBy<T, TKey>(this List<T> lst, Func<T, TKey> keyselector) where TKey : IComparable
		{
			Comparison<T> comp = delegate(T x, T y)
				{
					return keyselector(x).CompareTo(keyselector(y));
				};
			lst.Sort(comp);
			return lst;
		}

		public static List<T> OrderByDescending<T, TKey>(this List<T> lst, Func<T, TKey> keyselector) where TKey : IComparable
		{
			Comparison<T> comp = delegate(T x, T y)
				{
					return keyselector(y).CompareTo(keyselector(x));
				};
			lst.Sort(comp);
			return lst;
		}
	}

	public static class IEnumerableExt
	{
		public static List<T> Where<T>(this IEnumerable coll, Func<T, bool> func)
		{
		List<T> retval = new List<T>();

			foreach(T item in coll)
			{
				if(func(item) == true) retval.Add(item);
			}
			return retval;
		}

		public static List<T> Where<T>(this IEnumerable<T> coll, Func<T, bool> func)
		{
		List<T> retval = new List<T>();

			foreach(T item in coll)
			{
				if(func(item) == true) retval.Add(item);
			}
			return retval;
		}

		public static T FirstOrDefault<T>(this IEnumerable coll, Func<T, bool> func = null)
		{
			foreach(T item in coll)
			{
				if(func == null) return item;
				if(func(item) == true) return item;
			}
			return default(T);
		}

		public static T FirstOrDefault<T>(this IEnumerable<T> coll, Func<T, bool> func = null)
		{
			foreach(T item in coll)
			{
				if(func == null) return item;
				if(func(item) == true) return item;
			}
			return default(T);
		}
	}

	public static class StringExt
	{
		public static string Left(this string s, int length)
		{
			length = Math.Min(length, s.Length);
			return s.Substring(0, length);
		}

		public static string Right(this string s, int length)
		{
			length = Math.Min(length, s.Length);
		int start = s.Length - length;

			return s.Substring(start, length);
		}

		public static bool IsNumeric(this string s)
		{
		double output;
			return double.TryParse(s.Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out output);
		}
	}

    public static class EpiDataViewExt
    { // Version 1
        public static int ReCalculateIndex(this EpiDataView view)
        {
            if (view.dataView.Count <= 0) return -1;

            if (view.Row == -1 && view.dataView.Count > 0) return 0;

            if (view.Row >= view.dataView.Count) return (view.dataView.Count - 1);

            return view.Row;
        }
		
		public static T CurrentRow<T>(this EpiDataView view) where T:class
		{
			return view.CurrentDataRow as T;
		}
    }

	public static class DataRowExtensions
	{ // version 2
		public static DataRow InitDataRow(this DataRow row)
		{
		    foreach (DataColumn c in row.Table.Columns)
		    {
				if(row.IsNull(c) == true)
		        {
					if(c.DataType == typeof(DateTime))
					{
						continue;
					}
					else if(c.DataType.IsValueType)
					{
						row[c] = Activator.CreateInstance(c.DataType);
					}
					else if(c.DataType == typeof(String))
					{
						row[c] = string.Empty;
					}
				}
		    }
			return row;
		}
		
		public static bool CellChanged(this DataRow row, string col)
	    {
	        var originalVersion = row[col, DataRowVersion.Original];
	        var currentVersion = row[col, DataRowVersion.Current];
	        if (originalVersion == DBNull.Value && currentVersion == DBNull.Value)  
	        {
	            return false;
	        }
	        else if (originalVersion != DBNull.Value && currentVersion != DBNull.Value)  
	        {  
	            return !originalVersion.Equals(currentVersion);
	        }
	        return true;
	    }
	
		    public static IList<DataColumn> GetChangedColumns(this DataRow row)
	    {
			IList<DataColumn> retval = new List<DataColumn>();
			foreach(DataColumn c in row.Table.Columns)
			{
				if(row.CellChanged(c.ColumnName) == true)
				{
					retval.Add(c);
				}
			}
	        return retval;
	    }

	    public static IList<string> GetChangedColumnNames(this DataRow row)
	    {
			IList<DataColumn> tmp = row.GetChangedColumns();
			IList<string> retval = new List<string>();
			foreach(DataColumn c in tmp)
			{
				retval.Add(c.ColumnName);
			}
	        return retval;
	    }

		#region Extracted from class System.Data.DataRowExtensions, Assembly System.Data.DataSetExtensions.dll, Version=4.0.0.0
			private static class UnboxT<T>
			{
				internal static readonly Converter<object, T> Unbox = DataRowExtensions.UnboxT<T>.Create(typeof(T));
				private static Converter<object, T> Create(Type type)
				{
					if (!type.IsValueType)
					{
						return new Converter<object, T>(DataRowExtensions.UnboxT<T>.ReferenceField);
					}
					if (type.IsGenericType && !type.IsGenericTypeDefinition && typeof(Nullable<>) == type.GetGenericTypeDefinition())
					{
						return (Converter<object, T>)Delegate.CreateDelegate(typeof(Converter<object, T>), typeof(DataRowExtensions.UnboxT<T>).GetMethod("NullableField", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic).MakeGenericMethod(new Type[]
						{
							type.GetGenericArguments()[0]
						}));
					}
					return new Converter<object, T>(DataRowExtensions.UnboxT<T>.ValueField);
				}
				private static T ReferenceField(object value)
				{
					if (DBNull.Value != value)
					{
						return (T)((object)value);
					}
					return default(T);
				}
				private static T ValueField(object value)
				{
					if (DBNull.Value == value)
					{
						throw new InvalidCastException(typeof(T).ToString());
					}
					return (T)((object)value);
				}
				private static TElem? NullableField<TElem>(object value) where TElem : struct
				{
					if (DBNull.Value == value)
					{
						return null;
					}
					return new TElem?((TElem)((object)value));
				}
			}
			/// <summary>Provides strongly-typed access to each of the column values in the specified row. The <see cref="M:System.Data.DataRowExtensions.Field``1(System.Data.DataRow,System.String)" /> method also supports nullable types. </summary>
			/// <returns>The value, of type <paramref name="T" />, of the <see cref="T:System.Data.DataColumn" /> specified by <paramref name="columnName" />.</returns>
			/// <param name="row">The input <see cref="T:System.Data.DataRow" />, which acts as the this instance for the extension method.</param>
			/// <param name="columnName">The name of the column to return the value of.</param>
			/// <typeparam name="T">A generic parameter that specifies the return type of the column.</typeparam>
			/// <exception cref="T:System.InvalidCastException">The value type of the underlying column could not be cast to the type specified by the generic parameter, <paramref name="T" />.</exception>
			/// <exception cref="T:System.IndexOutOfRangeException">The column specified by <paramref name="columnName" /> does not occur in the <see cref="T:System.Data.DataTable" /> that the <see cref="T:System.Data.DataRow" /> is a part of.</exception>
			/// <exception cref="T:System.NullReferenceException">A null value was assigned to a non-nullable type.</exception>
			public static T Field<T>(this DataRow row, string columnName)
			{
				//DataSetUtil.CheckArgumentNull<DataRow>(row, "row");
				return DataRowExtensions.UnboxT<T>.Unbox(row[columnName]);
			}
		#endregion
	}

	public static class DataTableExt
    {
        public static void SetColumnsReadOnly(this DataTable dt, bool val)
        {
            foreach (DataColumn c in dt.Columns)
            {
                c.SetReadOnly(val);
            }
        }

        public static void SetColumnReadOnly(this DataTable dt, string colname, bool val)
        {
            DataColumn col = dt.Columns[colname];
            if (col == null) throw new Exception(string.Format("Could not find the column '{0}'.", colname));
            col.SetReadOnly(val);
        }

        public static T Compute<T>(this DataTable dt, string expression, string filter, T dfltval)
        {
            object retval = dt.Compute(expression, filter);

            if (retval == DBNull.Value)
            {
                return dfltval;
            }

            return (T)retval;
        }

        public static DataTable GroupBy(this DataTable dt, string[] fields, string filter = "", string sort = "")
        {
            DataView dvgrp = new DataView(dt)
            {
                RowFilter = filter,
                Sort = sort
            };
            return dvgrp.ToTable(true, fields);
        }

        public static void SetFormat<T>(this DataTable dt, string format)
        {
            foreach (DataColumn c in dt.Columns)
            {
                if (c.DataType == typeof(T))
                {
                    c.SetFormat(format);
                }
            }
        }

		public static DataRow FirstOrNull(this DataTable dt, string w, string o)
		{
			foreach(DataRow r in dt.Select(w, o))
			{
				return r;
			}
			return null;
		}

        public static T First<T>(this DataTable dt, string c, string w, string o, T d)
        {
		var row = dt.FirstOrNull(w, o);

			if(row == null) return d;
			
            return row.Field<T>(c);
        }
        
    }

    public static class DataColumnExt
    {
        public static void SetReadOnly(this DataColumn c, bool val)
        {
            c.ExtendedProperties["ReadOnly"] = val;
        }

        public static void SetFormat(this DataColumn c, string format)
        {
            c.ExtendedProperties["Format"] = format;
        }
		
		public static void SetLike(this DataColumn c, string like)
        {
            c.ExtendedProperties["Like"] = like;
        }
    }

    public static class EpiBaseFormExt
    {
        public static void SetVersionNumber(this EpiBaseForm frm)
        {
			if(frm.Text.Contains("Version") == true) return;

            string custName = frm.CustomizationName;
            string version = "?.?";

            int pos = custName.LastIndexOf("_");
            if (pos >= 0)
            {
                version = custName.Substring(pos + 1, custName.Length - pos - 1);
            }

            frm.Text = String.Format("{0} - Version {1}", frm.Text, version);
        }

		public static void PinTreeViewPanel(this EpiBaseForm form, bool pinned)
		{
			(form
				.Controls["windowDockingArea1"]
				.Controls["dockableWindow3"] as Infragistics.Win.UltraWinDock.DockableWindow)
				.Pane
				.Pinned = pinned;
		}

		public static void HideTreeViewPanel(this EpiBaseForm form)
		{
			form.Controls["windowDockingArea1"].Visible = false;
		}

		public static void RenameTreeViewPanel(this EpiBaseForm form, string text)
		{
			(form
				.Controls["windowDockingArea1"]
				.Controls["dockableWindow3"] as Infragistics.Win.UltraWinDock.DockableWindow)
				.Pane
				.Text = text;
		}
		
		public static void CenterToScreen(this Form frm)
		{
		var t = frm.GetType();
		var m = t.GetMethod("CenterToScreen", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance);

			m.Invoke(frm, new object[] {});
		}

    }

    public static class EpiUltraGridExt
    {
        public static void SetAutoEditMode(this EpiUltraGrid grid)
        {
            grid.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText;
            grid.AfterCellActivate += (sender, e) =>
            {
                var eug = sender as EpiUltraGrid;

                if (eug == null) return;
                if (((eug.CurrentState & Infragistics.Win.UltraWinGrid.UltraGridState.InEdit) == 0) // Check if we are already in edit mode
                    && ((eug.CurrentState & Infragistics.Win.UltraWinGrid.UltraGridState.Cell) == Infragistics.Win.UltraWinGrid.UltraGridState.Cell) // Check if its a cell that is selected
                    && (eug.ActiveCell.CanEnterEditMode == true)) // Check if the cell can be edited
                {
                    eug.PerformAction(Infragistics.Win.UltraWinGrid.UltraGridAction.EnterEditMode, false, false);
					if((eug.CurrentState & Infragistics.Win.UltraWinGrid.UltraGridState.InEdit) == Infragistics.Win.UltraWinGrid.UltraGridState.InEdit)
					{ // Prevent exception to be raised if the Cell didn't enter EditMode for a reason.
						eug.ActiveCell.SelectAll();
					}
                }
            };
        }
		
		public static void AllowUpdate(this EpiUltraGrid grid, bool val)
		{
			grid.DisplayLayout.Override.AllowUpdate = (val == true) ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False;
		}

		public static void AllowAddNew(this EpiUltraGrid grid, bool val)
		{
			grid.DisplayLayout.Override.AllowAddNew = (val == true) ? Infragistics.Win.UltraWinGrid.AllowAddNew.Yes : Infragistics.Win.UltraWinGrid.AllowAddNew.No;
		}

		public static void AllowDelete(this EpiUltraGrid grid, bool val)
		{
			grid.DisplayLayout.Override.AllowDelete = (val == true) ? Infragistics.Win.DefaultableBoolean.True : Infragistics.Win.DefaultableBoolean.False;
		}

    }

}

namespace ECS.ToolsKit // ECSDebug
{
    public class ECSDebug
    {
        private static ECSDebugForm _frmDebug;
        private static bool _showDiagnostic = false;

        public static bool ShowDiagnostic { get { return _showDiagnostic; } set { _showDiagnostic = value; } }

        private static ECSDebugForm FrmDebug
        {
            get
            {
                if (_frmDebug != null) return _frmDebug;
                _frmDebug = new ECSDebugForm();
                _frmDebug.FormClosed += _frmDebug_FormClosed;
                _frmDebug.Show();
                return _frmDebug;
            }
        }

        static void _frmDebug_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            _frmDebug.FormClosed -= _frmDebug_FormClosed;
            _frmDebug = null;
        }

        public static void WriteLine(object value)
        {
            if (ShowDiagnostic == false) return;
            FrmDebug.WriteLine(value);
        }

        public static void DisplayData(System.Data.DataSet ds)
        {
            if (ShowDiagnostic == false) return;
            ECSDataSetViewer frm = new ECSDataSetViewer(ds);
            frm.ShowDialog();
        }

        public static void DisplayData(System.Data.DataTable dt)
        {
            if (ShowDiagnostic == false) return;
            ECSDataSetViewer frm = new ECSDataSetViewer(dt);
            frm.ShowDialog();
        }

        public static void DisplayData(System.Data.DataView dv)
        {
            if (ShowDiagnostic == false) return;
            ECSDataSetViewer frm = new ECSDataSetViewer(dv);
            frm.ShowDialog();
        }

        public static void DisplayData(object collection)
        {
            DisplayData(collection, collection.GetType().ToString());
        }

        public static void DisplayData(object collection, string name)
        {
            if (ShowDiagnostic == false) return;
            ECSDataSetViewer frm = new ECSDataSetViewer(collection, name);
            frm.ShowDialog();
        }

        public class ECSDebugForm : System.Windows.Forms.Form
        {
            private System.ComponentModel.IContainer components = null;
            private System.Windows.Forms.RichTextBox txtOutput;

            public ECSDebugForm()
            {
                InitializeComponent();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            private void InitializeComponent()
            {
                this.txtOutput = new System.Windows.Forms.RichTextBox();
                this.SuspendLayout();

                this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
                this.txtOutput.Location = new System.Drawing.Point(0, 0);
                this.txtOutput.Multiline = true;
                this.txtOutput.Name = "txtOutput";
                this.txtOutput.ReadOnly = true;
                this.txtOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Both; //System.Windows.Forms.ScrollBars.Both;
                this.txtOutput.Size = new System.Drawing.Size(614, 262);
                this.txtOutput.TabIndex = 0;

                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(614, 262);
                this.Controls.Add(this.txtOutput);
                this.Name = "ECSDebugForm";
                this.Text = "Diagnostic Trace";
                this.ResumeLayout(false);
                this.PerformLayout();
            }

            public void WriteLine(object value)
            {
                string tmp = string.Format("{0:HH:mm:ss:fff} {1}\n", DateTime.Now, value.ToString());
                this.txtOutput.AppendText(tmp);
            }
        }

        partial class ECSDataSetViewer
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.tabTables = new System.Windows.Forms.TabControl();
                this.SuspendLayout();
                // 
                // tabTables
                // 
                this.tabTables.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tabTables.Location = new System.Drawing.Point(0, 0);
                this.tabTables.Name = "tabTables";
                this.tabTables.SelectedIndex = 0;
                this.tabTables.Size = new System.Drawing.Size(459, 262);
                this.tabTables.TabIndex = 0;
                // 
                // ECSDataSetViewer1
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(459, 262);
                this.Controls.Add(this.tabTables);
                this.Name = "ECSDataSetViewer";
                this.Text = "ECS DataSet Viewer";
                this.ResumeLayout(false);

            }

            #endregion

            private System.Windows.Forms.TabControl tabTables;
        }

        public partial class ECSDataSetViewer : System.Windows.Forms.Form
        {
            public ECSDataSetViewer(System.Data.DataSet ds)
                : this()
            {
                this.SuspendLayout();
                this.tabTables.SuspendLayout();

                foreach (System.Data.DataTable dt in ds.Tables)
                {
                    this.AddTable(dt, dt.TableName);
                }

                this.tabTables.ResumeLayout(false);
                this.ResumeLayout(false);
            }

            public ECSDataSetViewer(System.Data.DataTable dt)
                : this()
            {
                this.SuspendLayout();
                this.tabTables.SuspendLayout();

                this.AddTable(dt, dt.TableName);

                this.tabTables.ResumeLayout(false);
                this.ResumeLayout(false);
            }

			public ECSDataSetViewer(System.Data.DataView dv)
                : this()
            {
                this.SuspendLayout();
                this.tabTables.SuspendLayout();

                this.AddTable(dv, dv.Table.TableName);

                this.tabTables.ResumeLayout(false);
                this.ResumeLayout(false);
            }

            public ECSDataSetViewer(object collection, string tabname)
                : this()
            {
                this.SuspendLayout();
                this.tabTables.SuspendLayout();

                this.AddTable(collection, tabname);

                this.tabTables.ResumeLayout(false);
                this.ResumeLayout(false);
            }

            private ECSDataSetViewer()
            {
                InitializeComponent();
            }

            private void AddTable(object collection, string tabname)
            {
                System.Windows.Forms.TabPage tp = new System.Windows.Forms.TabPage(tabname);
                System.Windows.Forms.DataGridView dgv = new System.Windows.Forms.DataGridView();
                tp.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(dgv)).BeginInit();

                dgv.DataSource = collection;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.ReadOnly = true;
                tp.Controls.Add(dgv);
                dgv.Dock = System.Windows.Forms.DockStyle.Fill;
                this.tabTables.Controls.Add(tp);

                tp.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(dgv)).EndInit();
            }
        }
    }
}

public class Script
{
	// ** Wizard Insert Location - Do Not Remove 'Begin/End Wizard Added Module Level Variables' Comments! **
	// Begin Wizard Added Module Level Variables **


	private EpiDataView edvOrderHed;
	private EpiDataView edvOrderDtl;
	
	private DataTable dtOrderRelPerVinID;

	private EpiBaseAdapter oTrans_ordAdapter;
	private UD03Adapter _ud03Adapter;
	private DataTable UD03_Column;
	private EpiDataView _edvUD03;
	private string _Key1UD03;
	private string _Key2UD03;
	private string _Key3UD03;
	private string _Key4UD03;
	private string _Key5UD03;
	private DataView OrderHed_DataView;
	// End Wizard Added Module Level Variables **


	// Add Custom Module Level Variables Here **
	private ECS.SalesOrderEntry.SalesOrderEntry custoSalesOrderEntry;


	private Erp.UI.Controls.Combos.OrderRelPlantCombo cboPlant;
	private Ice.Lib.Framework.EpiUltraGrid grdHedMiscCharge;
	private Erp.UI.Controls.Combos.ReservePriCombo cboReservePriorityCode;
	private Ice.Lib.Framework.EpiDateTimeEditor dtpOrderDate;
	private Ice.Lib.Framework.EpiCheckBox chkReadyToFullfill;
	private Ice.Lib.Framework.EpiCheckBox chkReadyToCalc;	
	private Erp.UI.App.SalesOrderEntry.SheetReleasePanel sheetReleasePanel1;


	private EpiDataView _edvOrderRel;
	private DataTable _dtOrderRel;
	private DataTable _dtOrderHed;

	private bool _isUpdating = false;
	private bool _hasUpdated = false;

	private MiscChrgDataSet.MiscChrgRow _rowPowerTailgate = null;
	private MiscChrgDataSet.MiscChrgRow _rowShippingHandling = null;
	private MiscChrgDataSet.MiscChrgRow _rowDiscount = null;

	private string _curBrand = "";
	private string _curModel = "";
	private string _curVehicle = "";
	private string _curYear = "";

	public void InitializeCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Variable Initialization' lines **
		// Begin Wizard Added Variable Initialization
		//SetupEventTracing();

		dtOrderRelPerVinID = new DataTable();

		grdPartAssociations.BeforeCellUpdate += new BeforeCellUpdateEventHandler(grdPartAssociations_BeforeCellUpdate);

		this.edvOrderHed = ((EpiDataView)oTrans.EpiDataViews["OrderHed"]);
		this.edvOrderHed.EpiViewNotification += new EpiViewNotification(this.edvOrderHed_EpiViewNotification);
		this.edvOrderDtl = ((EpiDataView)(this.oTrans.EpiDataViews["OrderDtl"]));
		this.oTrans_ordAdapter = ((EpiBaseAdapter)(this.csm.TransAdaptersHT["oTrans_ordAdapter"]));
		this.oTrans_ordAdapter.AfterAdapterMethod += new AfterAdapterMethod(this.oTrans_ordAdapter_AfterAdapterMethod);
		InitializeUD03Adapter();
		this._Key1UD03 = string.Empty;
		this._Key2UD03 = string.Empty;
		this._Key3UD03 = string.Empty;
		this._Key4UD03 = string.Empty;
		this._Key5UD03 = string.Empty;
		this._edvUD03.EpiViewNotification += new EpiViewNotification(_edvUD03_EpiViewNotification);
		this.baseToolbarsManager.ToolClick += new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.baseToolbarsManager_ToolClickForUD03);
		this.SalesOrderForm.BeforeToolClick += new Ice.Lib.Framework.BeforeToolClickEventHandler(this.SalesOrderForm_BeforeToolClickForUD03);
		this.SalesOrderForm.AfterToolClick += new Ice.Lib.Framework.AfterToolClickEventHandler(this.SalesOrderForm_AfterToolClickForUD03);
		this.OrderHed_Row.EpiRowChanged += new EpiRowChanged(this.OrderHed_AfterRowChangeForUD03);
		this.OrderHed_DataView = this.OrderHed_Row.dataView;
		this.OrderHed_DataView.ListChanged += new ListChangedEventHandler(this.OrderHed_DataView_ListChangedForUD03);
		this.edvOrderDtl.EpiViewNotification += new EpiViewNotification(this.edvOrderDtl_EpiViewNotification);
		// End Wizard Added Variable Initialization

		// Begin Wizard Added Custom Method Calls

		//grdVinPart.DisplayLayout.Override.AllowAddNew = AllowAddNew.FixedAddRowOnTop;

		this.btnCalculateShipping.Click += new System.EventHandler(this.btnCalculateShipping_Click);
		this.cmbPriorityCode.ValueChanged += new System.EventHandler(this.cmbPriorityCode_ValueChanged);
		this.tmeOrderTime.LostFocus += new System.EventHandler(this.tmeOrderTime_LostFocus);
		this.btnVehicle.Click += new System.EventHandler(this.btnVehicle_Click);
		this.nedLineQty.Validated += new System.EventHandler(this.nedLineQty_Validated);
		this.btnEndCustomer.Click += new System.EventHandler(this.btnEndCustomer_Click);
		// End Wizard Added Custom Method Calls

		using (var bo = oTrans.GetNewBO<MiscChrgImpl, MiscChrgSvcContract>())
		{
			bool morePages;
			var ds = bo.GetRows("PowerTailGateDelivery_c = 1 OR ShippingAndHandling_c = 1 OR Discount_c = 1", "", "", -1, -1, out morePages);
			_rowPowerTailgate = ds.MiscChrg.FirstOrDefault<MiscChrgDataSet.MiscChrgRow>(r => (bool)r["PowerTailGateDelivery_c"] == true);
			_rowShippingHandling = ds.MiscChrg.FirstOrDefault<MiscChrgDataSet.MiscChrgRow>(r => (bool)r["ShippingAndHandling_c"] == true);
			_rowDiscount = ds.MiscChrg.FirstOrDefault<MiscChrgDataSet.MiscChrgRow>(r => (bool)r["Discount_c"] == true);
		}

		_edvOrderRel = ((EpiDataView)oTrans.EpiDataViews["OrderRel"]);

		_dtOrderRel = _edvOrderRel.dataView.Table;
		_dtOrderHed = edvOrderHed.dataView.Table;

		_dtOrderRel.ColumnChanged += new DataColumnChangeEventHandler(dtOrderRel_ColumnChanged);
		_dtOrderHed.ColumnChanged += new DataColumnChangeEventHandler(dtOrderHed_ColumnChanged);

		try{ custoSalesOrderEntry = new ECS.SalesOrderEntry.SalesOrderEntry(this); } catch(Exception ex){ ExceptionBox.Show(ex); } 
	}

	public void DestroyCustomCode()
	{
		// ** Wizard Insert Location - Do not delete 'Begin/End Wizard Added Object Disposal' lines **
		// Begin Wizard Added Object Disposal

		grdPartAssociations.BeforeCellUpdate -= new BeforeCellUpdateEventHandler(grdPartAssociations_BeforeCellUpdate);
		this.edvOrderHed.EpiViewNotification -= new EpiViewNotification(this.edvOrderHed_EpiViewNotification);
		this.edvOrderHed = null;
		this.btnCalculateShipping.Click -= new System.EventHandler(this.btnCalculateShipping_Click);
		this.edvOrderDtl = null;
		this.cmbPriorityCode.ValueChanged -= new System.EventHandler(this.cmbPriorityCode_ValueChanged);
		this.tmeOrderTime.LostFocus -= new System.EventHandler(this.tmeOrderTime_LostFocus);
		this.oTrans_ordAdapter.AfterAdapterMethod -= new AfterAdapterMethod(this.oTrans_ordAdapter_AfterAdapterMethod);
		this.oTrans_ordAdapter = null;
		this._edvUD03.EpiViewNotification -= new EpiViewNotification(_edvUD03_EpiViewNotification);
		if ((this._ud03Adapter != null))
		{
			this._ud03Adapter.Dispose();
			this._ud03Adapter = null;
		}
		this._edvUD03 = null;
		this.UD03_Column = null;
		this._Key1UD03 = null;
		this._Key2UD03 = null;
		this._Key3UD03 = null;
		this._Key4UD03 = null;
		this._Key5UD03 = null;
		this.baseToolbarsManager.ToolClick -= new Infragistics.Win.UltraWinToolbars.ToolClickEventHandler(this.baseToolbarsManager_ToolClickForUD03);
		this.SalesOrderForm.BeforeToolClick -= new Ice.Lib.Framework.BeforeToolClickEventHandler(this.SalesOrderForm_BeforeToolClickForUD03);
		this.SalesOrderForm.AfterToolClick -= new Ice.Lib.Framework.AfterToolClickEventHandler(this.SalesOrderForm_AfterToolClickForUD03);
		this.OrderHed_Row.EpiRowChanged -= new EpiRowChanged(this.OrderHed_AfterRowChangeForUD03);
		this.OrderHed_DataView.ListChanged -= new ListChangedEventHandler(this.OrderHed_DataView_ListChangedForUD03);
		this.OrderHed_DataView = null;
		this.btnVehicle.Click -= new System.EventHandler(this.btnVehicle_Click);
		this.edvOrderDtl.EpiViewNotification -= new EpiViewNotification(this.edvOrderDtl_EpiViewNotification);
		this.nedLineQty.Validated -= new System.EventHandler(this.nedLineQty_Validated);
		this.btnEndCustomer.Click -= new System.EventHandler(this.btnEndCustomer_Click);
		// End Wizard Added Object Disposal

		// Begin Custom Code Disposal

		_dtOrderRel.ColumnChanged -= new DataColumnChangeEventHandler(dtOrderRel_ColumnChanged);
		_dtOrderHed.ColumnChanged -= new DataColumnChangeEventHandler(dtOrderHed_ColumnChanged);

		dtpOrderDate.ValueChanged -= new System.EventHandler(dtpOrderDate_LostFocus);

		// End Custom Code Disposal
		try{ custoSalesOrderEntry.Dispose(); } catch(Exception ex){ ExceptionBox.Show(ex); } 
	}

	private bool InCustomizationMode
    {
        get
        {
            Infragistics.Win.UltraWinToolbars.ToolBase tb = baseToolbarsManager.Tools["CustomizeTool"];

            if (tb == null) return false; // le menu n'existe pas, on n'est pas en mode custo.

            return tb.VisibleResolved; // Si le menu n'est pas visible, on ne devrait pas être en mode custo.
        }
    }

    private void HideToolBars()
    {
        foreach (Infragistics.Win.UltraWinToolbars.UltraToolbar t in baseToolbarsManager.Toolbars)
        {
            t.Visible = false;
        }
    }

	private void SalesOrderForm_Load(object sender, EventArgs args)
	{
		// Add Event Handler Code
		//ECSDebug.ShowDiagnostic = this.InCustomizationMode;
		this.oTrans.EpiBaseForm.SetVersionNumber();

		// if it's an UDXX form, otherwise must be in comment
		//this.oTrans.EpiBaseForm.RenameTreeViewPanel("put some text here");
		//this.oTrans.EpiBaseForm.HideTreeViewPanel(); // Must be done in XXXForm_Load
		//if(this.InCustomizationMode == false) this.HideToolBars();

		cboPlant = (Erp.UI.Controls.Combos.OrderRelPlantCombo)csm.GetNativeControlReference("2d6cc2de-1547-4f18-907e-e00b44258d55");
		grdHedMiscCharge = (Ice.Lib.Framework.EpiUltraGrid)csm.GetNativeControlReference("72e8dbae-9bf8-4a5d-80a1-076d1bbf3d6f");
		cboReservePriorityCode = (Erp.UI.Controls.Combos.ReservePriCombo)csm.GetNativeControlReference("eeca84b5-7fec-45b0-9ba0-95cc0a9355ec");
		
		dtpOrderDate = (Ice.Lib.Framework.EpiDateTimeEditor)csm.GetNativeControlReference("d6ced30f-11c8-4556-9930-8d608dfecf97");
		dtpOrderDate.LostFocus += new EventHandler(dtpOrderDate_LostFocus);

		chkReadyToCalc =  (Ice.Lib.Framework.EpiCheckBox)csm.GetNativeControlReference("1161ef95-83de-4176-9160-0f78a4cf1e5e");
		chkReadyToFullfill = (Ice.Lib.Framework.EpiCheckBox)csm.GetNativeControlReference("10475e92-4a50-4359-9aaf-4f484c12121d");
		sheetReleasePanel1 = (Erp.UI.App.SalesOrderEntry.SheetReleasePanel)csm.GetNativeControlReference("fafa6c8e-7b71-44ea-b7bd-31f8747bd7e0");

		cboReservePriorityCode.Enabled = false;
		nedLineQty.Visible = false;

		this.custoSalesOrderEntry.InitClass(); // Initialise the stuff that can only be initialized during or after the form Load.
	}


	private void dtOrderRel_ColumnChanged(object sender, DataColumnChangeEventArgs e)
	{
		if (e.Column.ColumnName == "Plant" && !_isUpdating)
		{
			var previousVal = e.Row["Plant", DataRowVersion.Original];

			if (e.Row["Plant"].ToString() != previousVal.ToString())
			{
				DialogResult dialogResult = MessageBox.Show("Are you sure you want to change the site from " + previousVal.ToString()  + " to " + e.Row["Plant"].ToString() + "?",
															"Site change confirmation",
															MessageBoxButtons.YesNo);
				if (dialogResult == DialogResult.No)
				{
					_isUpdating = true;
					e.Row["Plant"] = previousVal;
					_isUpdating = false;
				}
			}
		}
	}


	private void dtOrderHed_ColumnChanged(object sender, DataColumnChangeEventArgs e)
	{
		if (e.Column.ColumnName == "ReadyToCalc" && (bool)e.Row["IsFleet_c"] && (bool)e.Row["ReadyToCalc"])
		{
			var orderNum = (int)edvOrderHed.dataView[edvOrderHed.Row]["OrderNum"];	
			using (var bo = oTrans.GetNewBO<SalesOrderImpl, SalesOrderSvcContract>())
			{
				bool morePages = false;
	
				var ds = bo.GetRows("OrderNum = " + orderNum.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", -1, -1, out morePages);
	
				// If there are still quantities not associated with a VIN, prevent from closing.
				var unAssigned = ds.OrderRel.Where<SalesOrderDataSet.OrderRelRow>(r => ((Guid)r["VinID_c"]).ToString("D") == (new Guid()).ToString("D"));
				if (unAssigned.Sum(r => r.SellingReqQty) > 0)
				{
					MessageBox.Show("All quantities must be assigned to a VIN on a fleet order before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					e.Row["ReadyToCalc"] = false;
					return;
				}
			}			
		}
		if (e.Column.ColumnName == "ReadyToFulfill" && (bool)e.Row["ReadyToFulfill"])
		{
			btnCalculateShipping_Click(this, null);
		}

		if (e.Column.ColumnName == "PowerTailgate_c" && _rowPowerTailgate != null)
		{
			var isBaseCurrency = (oTrans.EpiBaseForm.CurrentCurrencyToggleCode == CurrencyToggleCode.BASE);
	
			if ((bool)e.Row["PowerTailgate_c"])
			{
				var edvOHOrderMsc = ((EpiDataView)oTrans.EpiDataViews["OHOrderMsc"]);
				var miscCharge = edvOHOrderMsc.dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(r => r.MiscCode == _rowPowerTailgate["MiscCode"].ToString());
				if (miscCharge == null)
				{
					oTrans.GetNewOHOrderMsc();
					miscCharge = edvOHOrderMsc.dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(c => c.RowMod == "A");
					miscCharge.MiscCode = _rowPowerTailgate["MiscCode"].ToString();

					if (isBaseCurrency)
					{
						miscCharge.DspMiscAmt = Convert.ToDecimal(_rowPowerTailgate["MiscAmt"]);	
					}
					else
					{
						miscCharge.DocDspMiscAmt = Convert.ToDecimal(_rowPowerTailgate["MiscAmt"]);
					}				

					miscCharge.CurrencySwitch = isBaseCurrency;
				}
			}
			else
			{
				var miscCharge = ((EpiDataView)oTrans.EpiDataViews["OHOrderMsc"]).dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(r => r.MiscCode == _rowPowerTailgate["MiscCode"].ToString());
				if (miscCharge != null)
				{
					miscCharge.Delete();
				}
			}

			oTrans.Update();
		}


		
	}

	private void edvOrderHed_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if (args.NotifyType == EpiTransaction.NotifyType.Initialize)
		{
			if (cboPlant != null) cboPlant.Enabled = false;
			if (btnCalculateShipping != null) btnCalculateShipping.Enabled = true;
			if (sheetFleet != null) sheetFleet.Enabled = false;

			if (   args.Row >= 0
                && sheetReleasePanel1 != null
                && cboPlant != null
                && sheetFleet != null
				&& chkOverrideShipSite != null)
			{
				_curBrand = "";
				_curModel = "";
				_curVehicle = "";
				_curYear = "";

				cboPlant.Enabled = chkOverrideShipSite.Checked;
				//btnCalculateShipping.Enabled = chkOverrideSpecialDiscounts.Checked;
				sheetFleet.Enabled = (bool)view.dataView[args.Row]["IsFleet_c"];
				sheetReleasePanel1.Enabled = !((bool)view.dataView[args.Row]["IsFleet_c"]  && (string)view.dataView[args.Row]["PriorityCode_c"] == "JTF4" ) ;
				//((DataView)grdHedMiscCharge.DataSource).Table.Columns["DocDspMiscAmt"].ReadOnly = !chkOverrideSpecialDiscounts.Checked;
			}
			else
			{
				ClearUD03Data();
			}

			if (_hasUpdated)
			{
				_hasUpdated = false;
				oTrans.Refresh();
			}
		}
	}

	private void btnCalculateShipping_Click(object sender, System.EventArgs args)
	{
		oTrans.Update();
		
		// First we need to get a reference to the Customer entity for the current order
		using (var cust = oTrans.GetNewBO<CustomerImpl, CustomerSvcContract>())
		{
			bool morePages;
			var custRow = cust.GetRows("CustNum = '" + edvOrderHed.CurrentDataRow["CustNum"].ToString() + "'", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", -1, -1, out morePages)
                              .Customer
                              .FirstOrDefault<CustomerDataSet.CustomerRow>();

			if (custRow != null)
			{
				var groupCode = custRow.GroupCode;

				using (var ud02 = oTrans.GetNewBO<UD02Impl, UD02SvcContract>())
				{
					// With the order amount, the PriorityCode, and the DiscountType (GroupCode), extract the matching row from the UD02 table (discount matrix).
					var whereClause = string.Format("AmountFrom_c <= {0} AND (AmountTo_c >= {0} OR AmountTo_c = 0) AND PriorityCode_c = '{1}' AND DiscountType_c = '{2}'",
		                                            edvOrderHed.CurrentDataRow["OrderAmt"].ToString(),
		                                            edvOrderHed.CurrentDataRow["PriorityCode_c"].ToString(),
		                                            groupCode);

					var ud02Row = ud02.GetRows(whereClause, "", -1, -1, out morePages)
                                      .UD02
                                      .FirstOrDefault<UD02DataSet.UD02Row>();

					if (ud02Row != null)
					{
						var isBaseCurrency = (oTrans.EpiBaseForm.CurrentCurrencyToggleCode == CurrencyToggleCode.BASE);

						// Calculate extra shipping charges
						var extraShippingAmt = Convert.ToDecimal(ud02Row["ExtraShippingAmount_c"]);
						var extraShippingPercent = Convert.ToDecimal(ud02Row["ExtraShippingPerc_c"]);

						var orderAmt = (isBaseCurrency) ? Convert.ToDecimal(edvOrderHed.CurrentDataRow["TotalCharges"]) - Convert.ToDecimal(edvOrderHed.CurrentDataRow["TotalDiscount"])
                                                        : Convert.ToDecimal(edvOrderHed.CurrentDataRow["DocTotalCharges"]) - Convert.ToDecimal(edvOrderHed.CurrentDataRow["DocTotalDiscount"]);

						var extraShippingCalcAmt = extraShippingAmt + (orderAmt * (extraShippingPercent / 100.0m));

						//MessageBox.Show(string.Format("extraShippingAmt = {0}\nextraShippingPercent = {1}\norderAmt = {2}\nextraShippingCalcAmt = {3}",
                        //                              extraShippingAmt, extraShippingPercent, orderAmt, extraShippingCalcAmt));

						// If we have extra shipping charges, add them on the order
						if (extraShippingCalcAmt > 0)
						{
							// First check if a shipping and handling charge already exists.
							var edvOHOrderMsc = ((EpiDataView)oTrans.EpiDataViews["OHOrderMsc"]);
							var miscCharge = edvOHOrderMsc.dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(r => r.MiscCode == _rowShippingHandling["MiscCode"].ToString());
							if (miscCharge == null)
							{
								// If not, add it.
								oTrans.GetNewOHOrderMsc();
								miscCharge = edvOHOrderMsc.dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(c => c.RowMod == "A");
								miscCharge.MiscCode = _rowShippingHandling["MiscCode"].ToString();								
								miscCharge.FreqCode = "F";								
							}

							if (miscCharge != null)
							{
								// Otherwise update the amount
								if (isBaseCurrency)
								{
									miscCharge.DspMiscAmt = extraShippingCalcAmt;
								}
								else
								{
									miscCharge.DocDspMiscAmt = extraShippingCalcAmt;
								}
									
								miscCharge.CurrencySwitch = isBaseCurrency;
							}
							// Save the transaction and refresh the views.
							oTrans.Update();
						}

						// If we have a discount percentage (above 0%), apply it as a negative charge.
						var discountPercent = Convert.ToDecimal(ud02Row["AddDiscPerc_c"]);
						if (discountPercent > 0)
						{
							// First check if a discount charge already exists.
							var edvOHOrderMsc = ((EpiDataView)oTrans.EpiDataViews["OHOrderMsc"]);
							var miscCharge = edvOHOrderMsc.dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(r => r.MiscCode == _rowDiscount["MiscCode"].ToString());
							if (miscCharge == null)
							{
								// If not, add it.
								oTrans.GetNewOHOrderMsc();
								miscCharge = edvOHOrderMsc.dataView.Table.Rows.FirstOrDefault<SalesOrderDataSet.OHOrderMscRow>(c => c.RowMod == "A");
								miscCharge.MiscCode = _rowDiscount["MiscCode"].ToString();								
								miscCharge.FreqCode = "F";
								miscCharge.Description = discountPercent.ToString() + "% Large Order Discount";							
							}

							if (miscCharge != null)
							{
								// Otherwise update the amount								
								var extraDiscountAmt = orderAmt * (discountPercent / 100m);
								if (isBaseCurrency)
								{
									miscCharge.DspMiscAmt = -extraDiscountAmt;
								}
								else
								{
									miscCharge.DocDspMiscAmt = -extraDiscountAmt;
								}
									
								miscCharge.CurrencySwitch = isBaseCurrency;
							}

							// Save the transaction and refresh the views.
							oTrans.Update();
						}
						
						// Fix for a problem with Epicor, which pops an error server side that I cannot figure out.
						// Analysed with Ghislain, spent a lot of time trying to get around it, and this seems to be the only
						// way that works. This uses reflection to call a Protected method on the form to clear the form, then reload the order.
						// Not perfect, but it works. oTrans.ClearDataSets() does NOT work, neither does oTrans.Refresh().
						var orderNum = (int)edvOrderHed.CurrentDataRow["OrderNum"];
						var frm = (Erp.UI.App.SalesOrderEntry.SalesOrderForm)SalesOrderForm;
						var t = frm.GetType();
						var m = t.GetMethod("OnClickClear", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
						m.Invoke(frm, new object[] {});

						oTrans.GetOrderData(orderNum);

					}
				}
			}
		}
	}


	private void cmbPriorityCode_ValueChanged(object sender, System.EventArgs args)
	{
		if (edvOrderHed.Row > -1)
		{
			//MessageBox.Show("cmbPriorityCode_ValueChanged");
			using (var rpClient = oTrans.GetNewBO<ReservePriImpl, ReservePriSvcContract>())
			{
				bool morePages = false;
				var reservePri = rpClient.GetRows("Company = '" + edvOrderHed.dataView[edvOrderHed.Row]["Company"].ToString() + "' AND PriorityCode = '" + cmbPriorityCode.Value.ToString() + "'", -1, -1, out morePages)
	                                     .ReservePri
	                                     .FirstOrDefault<ReservePriDataSet.ReservePriRow>();
	
				// Do not allow changing priority code to a non-Fleet one if VINs exist
				if (reservePri != null && !((bool)reservePri["IsFleet_c"]) && _edvUD03.dataView.Count > 0)
				{
					MessageBox.Show("You cannot change to a non-fleet priority code if there are existing VINs attached to this order. Please remove the VINs and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					oTrans.Refresh();
					return;
				}
			}
	
			cboReservePriorityCode.Value = cmbPriorityCode.Value;
	
			using (var rpClient = oTrans.GetNewBO<ReservePriImpl, ReservePriSvcContract>())
			{
				bool morePages = false;
				var reservePri = rpClient.GetRows("Company = '" + edvOrderHed.dataView[edvOrderHed.Row]["Company"].ToString() + "' AND PriorityCode = '" + cmbPriorityCode.Value.ToString() + "'", -1, -1, out morePages)
	                                     .ReservePri
	                                     .FirstOrDefault<ReservePriDataSet.ReservePriRow>();
				
				if (reservePri != null)
				{
					edvOrderHed.dataView[edvOrderHed.Row]["IsFleet_c"] = (bool)reservePri["IsFleet_c"];
	
					if ((bool)edvOrderHed.dataView[edvOrderHed.Row]["ReadyToCalc"])
					{
						using (var plantClient = oTrans.GetNewBO<PlantImpl, PlantSvcContract>())
						{
							var plant = plantClient.GetRows("Company = '" + edvOrderHed.dataView[edvOrderHed.Row]["Company"].ToString() + "' AND Plant = '" + edvOrderHed.dataView[edvOrderHed.Row]["Plant"].ToString() + "'","", "", -1, -1, out morePages)
												   .Plant
												   .FirstOrDefault<PlantDataSet.PlantRow>();
		
							// If the current order does not have a plant associated, set it to the current plant and use its calendar
							if (plant == null)
							{
								var currentPlantID = ((Ice.Core.Session)oTrans.Session).PlantID;
								edvOrderHed.dataView[edvOrderHed.Row]["Plant"] = currentPlantID;
								plant = plantClient.GetRows("Company = '" + edvOrderHed.dataView[edvOrderHed.Row]["Company"].ToString() + "' AND Plant = '" + currentPlantID + "'","", "", -1, -1, out morePages)
												   .Plant
												   .FirstOrDefault<PlantDataSet.PlantRow>();
							}
		
							// Get local time
							var tempDate = (tmeOrderTime.Value != null) ? tmeOrderTime.Value : null;
							var orderTime = (tempDate != null) ? ((DateTime)tempDate - ((DateTime)tempDate).Date).TotalSeconds : 0;
							var orderDateTime = ((DateTime)dtpOrderDate.Value).AddSeconds(orderTime);
							
							var limitDateTime = ((DateTime)dtpOrderDate.Value).AddSeconds((int)reservePri["AddExtraDayAfter_c"]);
							
							// If local time exceeds the time limit, add a day to the normal lead time.
							var extraDays = (int)reservePri["LeadTimeDays_c"];
							
							// If the AddExtraDay field is set
							if ((bool)reservePri["AddExtraDay_c"]) extraDays += (orderDateTime > limitDateTime) ? 1 : 0;
		
							var countDays = 0;
							var daysToAdd = 0;
							
							using (var prodCalClient = oTrans.GetNewBO<ProdCalImpl, ProdCalSvcContract>())
							{
								var endDate = (DateTime)edvOrderHed.dataView[edvOrderHed.Row]["OrderDate"];
		
								// While the count of added days is lower or equal to the number of extra days to add
								while (countDays < extraDays)
								{
									// Increment the number of absolute days to add.
									daysToAdd += 1;
		
									// Increment the end date by one day
									endDate = endDate.AddDays(1);
		
									// Get the number representing the day of the week. Sunday = 0, Saturday = 6
		                            // Add 1, and we will use this as an indexer for the database fields WorkWeek1-7
									var dayOfWeek = ((int)endDate.DayOfWeek) + 1;
									
									// Check if the end date if a valid prod day from the calendar.
									var whereClauseProdCal = string.Format("Company = '{0}' AND CalendarID = '{1}'",
		                                                                   edvOrderHed.dataView[edvOrderHed.Row]["Company"].ToString(),
		                                                                   plant.CalendarID);
		
									var whereClauseProdCalDay = string.Format("ModifiedDay = '{0}'",
		                                                                      endDate.ToString("yyyyMMdd"));
		
									var rows = prodCalClient.GetRows(whereClauseProdCal, whereClauseProdCalDay, "", "", "", -1, -1, out morePages);
									
									var isValidWeekDay = rows.ProdCal.FirstOrDefault<ProdCalDataSet.ProdCalRow>(c => (bool)c["WorkWeek" + dayOfWeek.ToString()]) != null;
									var isValidDay = rows.ProdCalDay.FirstOrDefault<ProdCalDataSet.ProdCalDayRow>(d => !d.WorkingDay) == null;
		
									// If it is a valid day, increment the count
									if (isValidWeekDay && isValidDay) countDays += 1;	
								}
							}
		
							var needByDate = ((DateTime)edvOrderHed.dataView[edvOrderHed.Row]["OrderDate"]).AddDays(daysToAdd);
							
							edvOrderHed.dataView[edvOrderHed.Row]["NeedByDate"] = needByDate;
							edvOrderHed.dataView[edvOrderHed.Row]["RequestDate"] = needByDate;
		
						}
					}
				}
			}
		}

		
	}


	private void tmeOrderTime_LostFocus(object sender, System.EventArgs args)
	{
		cmbPriorityCode_ValueChanged(sender, args);
	}

	private void dtpOrderDate_LostFocus(object sender, System.EventArgs args)
	{
		cmbPriorityCode_ValueChanged(sender, args);
	}

	
	private void oTrans_ordAdapter_AfterAdapterMethod(object sender, AfterAdapterMethodArgs args)
	{
		// ** Argument Properties and Uses **
		// ** args.MethodName **
		// ** Add Event Handler Code **

		// ** Use MessageBox to find adapter method name
		// EpiMessageBox.Show(args.MethodName)
		switch (args.MethodName)
		{
			case "Update":
				_hasUpdated = true;
				break;
		}

	}

	private void SetupEventTracing()
	{
		ECSDebug.ShowDiagnostic = true;

		ICollection adpKeys = this.csm.TransAdaptersHT.Keys;
		foreach(object o in adpKeys)
		{
			EpiBaseAdapter adp = (EpiBaseAdapter)(this.csm.TransAdaptersHT[o]);
			adp.BeforeAdapterMethod += (s,e) => { ECSDebug.WriteLine(string.Format("{0} {1} BeforeAdapterMethod {2}", o, adp.GetType(), e.MethodName)); };
			adp.AfterAdapterMethod += (s,e) => { ECSDebug.WriteLine(string.Format("{0} {1} AfterAdapterMethod {2}", o, adp.GetType(), e.MethodName)); };
		}

		ICollection edvKeys = this.oTrans.EpiDataViews.Keys;
		foreach(object o in edvKeys)
		{
			EpiDataView edv = (EpiDataView)(this.oTrans.EpiDataViews[o]);
			edv.EpiViewNotification += (s,e) => { ECSDebug.WriteLine(string.Format("{0} EpiViewNotification {1}", o, e.NotifyType)); };
			edv.dataView.Table.ColumnChanged += (s,e) => { ECSDebug.WriteLine(string.Format("{0} ColumnChanged {1} {2}", e.Row.Table.TableName, e.Column.ColumnName, e.ProposedValue)); };
		}
		
		this.oTrans.EpiViewChanged += (a) => { ECSDebug.WriteLine(string.Format("EpiViewChanged to {0}", a.CurrentView.ViewName)); };
		this.oTrans.EpiButtonClick += (a) => { ECSDebug.WriteLine(string.Format("EpiButtonClick {0}", a.Button.Name)); };
		
		this.oTrans.EpiBaseForm.BeforeToolClick += (s,e) => { ECSDebug.WriteLine(string.Format("BeforeToolClick {0}", e.Tool.Key)); };

		// Epicor 10.1 only
		this.oTrans.EpiBaseForm.AfterToolClick += (s,e) => { ECSDebug.WriteLine(string.Format("AfterToolClick {0}", e.Tool.Key)); };
	}


	private void InitializeUD03Adapter()
	{
		// Create an instance of the Adapter.
		this._ud03Adapter = new UD03Adapter(this.oTrans);
		this._ud03Adapter.BOConnect();

		// Add Adapter Table to List of Views
		// This allows you to bind controls to the custom UD Table
		this._edvUD03 = new EpiDataView();
		this._edvUD03.dataView = new DataView(this._ud03Adapter.UD03Data.UD03);
		this._edvUD03.AddEnabled = true;
		this._edvUD03.AddText = "New VIN";
		if ((this.oTrans.EpiDataViews.ContainsKey("UD03View") == false))
		{
			this.oTrans.Add("UD03View", this._edvUD03);
		}

		// Initialize DataTable variable
		this.UD03_Column = this._ud03Adapter.UD03Data.UD03;

		// Set the parent view / keys for UD child view
		string[] parentKeyFields = new string[1];
		string[] childKeyFields = new string[1];
		parentKeyFields[0] = "OrderNum";
		childKeyFields[0] = "Key2";
		this._edvUD03.SetParentView(edvOrderHed, parentKeyFields, childKeyFields);

		if ((this.oTrans.PrimaryAdapter != null))
		{
			this.oTrans.PrimaryAdapter.GetCurrentDataSet(Ice.Lib.Searches.DataSetMode.RowsDataSet).Tables.Add(this._edvUD03.dataView.Table.Clone());
		}

	}

	private void _edvUD03_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		//chkIsFleet.Enabled = (_edvUD03.dataView.Count == 0);
		//lblIsFleet.Enabled = chkIsFleet.Enabled;

		if (args.NotifyType == EpiTransaction.NotifyType.Initialize && args.Row > -1)
		{
			//MessageBox.Show("UD03_EpiViewNotification");

			_edvUD03.SetCurrentRowPropertyManually("Brand_c", SettingStyle.Disabled);
			_edvUD03.SetCurrentRowPropertyManually("Model_c", SettingStyle.Disabled);
			_edvUD03.SetCurrentRowPropertyManually("Vehicle_c", SettingStyle.Disabled);

			grdVinPart.SetCellEditor(grdVinPart.DisplayLayout.Bands[0].Columns["ReasonForLate_c"], cboLateReason);
			grdVinPart.SetCellEditor(grdVinPart.DisplayLayout.Bands[0].Columns["ReasonForChange_c"], cboChangeReason);

			LoadOrderRelPerVinID();
		}

	}

	private void GetUD03Data(string orderNum)
	{
		if (this._Key2UD03 != orderNum)
		{
			// Build where clause for search.
			string whereClause = "Key2 = \'" + orderNum + "\'";
			System.Collections.Hashtable whereClauses = new System.Collections.Hashtable(1);
			whereClauses.Add("UD03", whereClause);

			// Call the adapter search.
			SearchOptions searchOptions = SearchOptions.CreateRuntimeSearch(whereClauses, DataSetMode.RowsDataSet);
			this._ud03Adapter.InvokeSearch(searchOptions);

			if ((this._ud03Adapter.UD03Data.UD03.Rows.Count > 0))
			{
				this._edvUD03.Row = 0;
			} else
			{
				this._edvUD03.Row = -1;
			}

			// Notify that data was updated.
			this._edvUD03.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD03.Row, this._edvUD03.Column));

			// Set key fields to their new values.
			this._Key2UD03 = orderNum;
		}
	}

	private void GetNewUD03Record()
	{
		DataRow parentViewRow = edvOrderHed.CurrentDataRow;

		// Check for existence of Parent Row.
		if ((parentViewRow == null))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(_curBrand + _curModel + _curYear + _curVehicle))
		{
			var rowCount = _ud03Adapter.UD03Data.UD03.Rows.Count;
			
			if (rowCount > 0)
			{
				var lastRow = _ud03Adapter.UD03Data.UD03.Rows[rowCount - 1];
				_curBrand = lastRow["Brand_c"].ToString();
				_curModel = lastRow["Model_c"].ToString();
				_curYear = lastRow["Year_c"].ToString();
				_curVehicle = lastRow["Vehicle_c"].ToString();
			}			
		}

		if (string.IsNullOrWhiteSpace(_curBrand + _curModel + _curYear + _curVehicle))
		{
			SearchOnUD100AdapterShowDialog();
		}

		if (string.IsNullOrWhiteSpace(_curBrand + _curModel + _curYear + _curVehicle))
		{
			MessageBox.Show("The selected vehicle is not entered correctly into the database, the Brand, Model, Year, and Vehicle fields are empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		if (!string.IsNullOrWhiteSpace(_curBrand + _curModel + _curYear + _curVehicle) && this._ud03Adapter.GetaNewUD03())
		{
			// Get unique row count id for Key5
			int rowCount = this._ud03Adapter.UD03Data.UD03.Rows.Count;
			int lineNum = rowCount;
			bool goodIndex = false;
			while ((goodIndex == false))
			{
				// Check to see if index exists
				DataRow[] matchingRows = this._ud03Adapter.UD03Data.UD03.Select("Key5 = \'" + lineNum.ToString() + "\'");
				if ((matchingRows.Length > 0))
				{
					lineNum = (lineNum + 1);
				} else
				{
					goodIndex = true;
				}
			}

			// Set initial UD Key values
			var newRowGuid = Guid.NewGuid();

			DataRow editRow = this._ud03Adapter.UD03Data.UD03.Rows[(rowCount - 1)];
			editRow.BeginEdit();
			editRow["Key1"] = newRowGuid.ToString("D");
			editRow["Key2"] = parentViewRow["OrderNum"].ToString();
			editRow["Key3"] = string.Empty;
			editRow["Key4"] = string.Empty;
			editRow["Key5"] = lineNum.ToString();
			editRow["VinID_c"] = newRowGuid;
			editRow["OrderNum_c"] = parentViewRow["OrderNum"];
			editRow["Brand_c"] = _curBrand;
			editRow["Model_c"] = _curModel;
			editRow["Year_c"] = _curYear;
			editRow["Vehicle_c"] = _curVehicle;

			editRow.EndEdit();

			// Notify that data was updated.
			this._edvUD03.Notify(new EpiNotifyArgs(this.oTrans, (rowCount - 1), this._edvUD03.Column));
		}
	}

	private void SaveUD03Record()
	{
		// Save adapter data
		this._ud03Adapter.Update();
	}

	private void DeleteUD03Record()
	{
		// Check to see if deleted view is ancestor view
		bool isAncestorView = false;
		Ice.Lib.Framework.EpiDataView parView = this._edvUD03.ParentView;
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
			DataRow[] drsDeleted = this._ud03Adapter.UD03Data.UD03.Select("Key1 = \'" + this._Key1UD03 + "\' AND Key2 = \'" + this._Key2UD03 + "\' AND Key3 = \'" + this._Key3UD03 + "\' AND Key4 = \'" + this._Key4UD03 + "\'");
			for (int i = 0; (i < drsDeleted.Length); i = (i + 1))
			{
				this._ud03Adapter.Delete(drsDeleted[i]);
			}
		} else
		{
			if ((this.oTrans.LastView == this._edvUD03))
			{
				if ((this._edvUD03.Row >= 0))
				{
					DataRow drDeleted = ((DataRow)(this._ud03Adapter.UD03Data.UD03.Rows[this._edvUD03.Row]));
					if ((drDeleted != null))
					{
						if (this._ud03Adapter.Delete(drDeleted))
						{
							if ((_edvUD03.Row > 0))
							{
								_edvUD03.Row = (_edvUD03.Row - 1);
							}

							// Notify that data was updated.
							this._edvUD03.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD03.Row, this._edvUD03.Column));
						}
					}
				}
			}
		}
	}

	private void UndoUD03Changes()
	{
		this._ud03Adapter.UD03Data.RejectChanges();

		// Notify that data was updated.
		this._edvUD03.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD03.Row, this._edvUD03.Column));
	}

	private void ClearUD03Data()
	{
		this._Key1UD03 = string.Empty;
		this._Key2UD03 = string.Empty;
		this._Key3UD03 = string.Empty;
		this._Key4UD03 = string.Empty;
		this._Key5UD03 = string.Empty;

		this._ud03Adapter.UD03Data.Clear();

		dtOrderRelPerVinID.Clear();
		grdPartAssociations.Refresh();

		// Notify that data was updated.
		this._edvUD03.Notify(new EpiNotifyArgs(this.oTrans, this._edvUD03.Row, this._edvUD03.Column));
	}

	private void baseToolbarsManager_ToolClickForUD03(object sender, Infragistics.Win.UltraWinToolbars.ToolClickEventArgs args)
	{
		switch (args.Tool.Key)
		{
			case "EpiAddNewNew VIN":
				if (!((bool)edvOrderHed.dataView[edvOrderHed.Row]["IsFleet_c"]))
				{
					MessageBox.Show("You must be in a fleet order in order to add a VIN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					GetNewUD03Record();
				}				
				break;

			case "ClearTool":
				ClearUD03Data();
				break;

			case "UndoTool":
				UndoUD03Changes();
				break;
		}
	}

	private void SalesOrderForm_BeforeToolClickForUD03(object sender, Ice.Lib.Framework.BeforeToolClickEventArgs args)
	{
		switch (args.Tool.Key)
		{
			case "SaveTool":
				//MessageBox.Show("BeforeToolClick:SaveTool");
				SaveUD03Record();
				break;
		}
	}

	private void SalesOrderForm_AfterToolClickForUD03(object sender, Ice.Lib.Framework.AfterToolClickEventArgs args)
	{
		switch (args.Tool.Key)
		{
			case "DeleteTool":
				//MessageBox.Show("AfterToolClick:DeleteTool " + _edvUD03.Row.ToString());
				if (_edvUD03.Row > -1)
				{
					var vinId = (Guid)_edvUD03.dataView[_edvUD03.Row]["VinID_c"];
					var orderNum = (int)edvOrderHed.dataView[edvOrderHed.Row]["OrderNum"];	
			
					using (var bo = oTrans.GetNewBO<SalesOrderImpl, SalesOrderSvcContract>())
					{
						bool morePages = false;
			
						var ds = bo.GetRows("OrderNum = " + orderNum.ToString(), "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", -1, -1, out morePages);
			
						// Find existing release for this VIN, if it exists
						var existingRel = ds.OrderRel.FirstOrDefault<SalesOrderDataSet.OrderRelRow>(r => ((Guid)r["VinID_c"]).ToString("D") == vinId.ToString("D"));
						if (existingRel != null)
						{
							MessageBox.Show("You must remove existing releases associated with this VIN before removing it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
							return;
						}
					}

					DeleteUD03Record();
				}
				break;
		}
	}

	private void OrderHed_AfterRowChangeForUD03(EpiRowChangedArgs args)
	{
		// ** add AfterRowChange event handler
		string ordernum = args.CurrentView.dataView[args.CurrentRow]["OrderNum"].ToString();
		//MessageBox.Show("AfterRowChange" + ordernum);
		GetUD03Data(ordernum);
	}

	private void OrderHed_DataView_ListChangedForUD03(object sender, ListChangedEventArgs args)
	{
		// ** add ListChanged event handler
		string ordernum = OrderHed_DataView[0]["OrderNum"].ToString();
		//MessageBox.Show("ListChanged" + ordernum);
		GetUD03Data(ordernum);
	}

	private void btnVehicle_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		//GetNewUD03Record();
		SearchOnUD100AdapterShowDialog();
	
		if (_edvUD03.Row > -1)
		{
			var row = _ud03Adapter.UD03Data.UD03.Rows[_edvUD03.Row];
			row["Brand_c"] = _curBrand;
			row["Model_c"] = _curModel;
			row["Year_c"] = _curYear;
			row["Vehicle_c"] = _curVehicle;
		}
	}

	private void SearchOnUD100AdapterShowDialog()
	{
		// Wizard Generated Search Method
		// You will need to call this method from another method in custom code
		// For example, [Form]_Load or [Button]_Click
		using (var bo = new UD100Adapter(oTrans))
		{
			bo.BOConnect();

			var morePages = false;
			SearchOptions so = new SearchOptions(SearchMode.ShowDialog);
			so.SelectMode = SelectMode.SingleSelect;
			if (bo.InvokeSearch(so) == DialogResult.OK)
			{
				System.Data.DataRow selectedRow = bo.UD100List.Tables[0].Rows[0];
		
				_curBrand = selectedRow["Key1"].ToString();
				_curModel = selectedRow["Key2"].ToString();
				_curYear = selectedRow["Key3"].ToString();
				_curVehicle = _curBrand + " " + _curModel + " " + _curYear;
			}
		}
		
	}

	
	private void grdPartAssociations_BeforeCellUpdate(object sender, Infragistics.Win.UltraWinGrid.BeforeCellUpdateEventArgs e)
	{
		var oldQty = Convert.ToDecimal(e.Cell.Value);
		var newQty = Convert.ToDecimal(e.NewValue);
		var orderNum = (int)edvOrderHed.dataView[edvOrderHed.Row]["OrderNum"];
		var orderLine = (int)grdPartAssociations.ActiveRow.GetCellValue(grdPartAssociations.DisplayLayout.Bands[0].Columns[0]);
		
		var vinId = (Guid)_edvUD03.dataView[_edvUD03.Row]["VinID_c"];

		var delta = newQty - oldQty;

		using (var bo = oTrans.GetNewBO<SalesOrderImpl, SalesOrderSvcContract>())
		{
			bool morePages = false;

			var ds = bo.GetRows("OrderNum = " + orderNum.ToString(), "", "", "", "", "", "OrderLine = " + orderLine.ToString(), "", "", "", "", "", "", "", "", "", "", -1, -1, out morePages);
		
			if (delta > 0)
			{
				// Delta is positive, add to releases

				// Find existing releases with no VIN and deduct from them until delta reached
				var availRels = ds.OrderRel.Where<SalesOrderDataSet.OrderRelRow>(r => ((Guid)r["VinID_c"]).ToString("D") == "00000000-0000-0000-0000-000000000000");
				var availQty = availRels.Sum(r => r.SellingReqQty);

				if (availRels.Count > 0)
				{
					if (delta > availQty)
					{
						// ####### Exception: If the positive delta is larger than the total quantity for all the available releases, abort
						MessageBox.Show("The difference entered exceeds the total quantity available for release.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						// Find existing release for this VIN, if it exists
						var existingRel = ds.OrderRel.FirstOrDefault<SalesOrderDataSet.OrderRelRow>(r => ((Guid)r["VinID_c"]).ToString("D") == vinId.ToString("D"));
						if (existingRel != null)
						{
							// If it exists, remove the existing release and create a new one with the total quantity needed.
							delta += existingRel.SellingReqQty;
							existingRel.Delete();
							bo.Update(ds);
						}

						// Create a new release for this order line and VIN for the quantity of the delta.
						bo.GetNewOrderRel(ds, orderNum, orderLine);
						var orderRel = ds.OrderRel.FirstOrDefault<SalesOrderDataSet.OrderRelRow>(r => r.RowMod == "A");
						
						try
						{
							orderRel.NeedByDate = ((DateTime)_edvUD03.dataView[_edvUD03.Row]["EstVehArrival_c"]).AddDays(-14);
						}
						catch { }

						orderRel.SellingReqQty = delta;
						orderRel.OurReqQty = delta;
						orderRel["VinID_c"] = vinId;
					}
				}
				else
				{
					// ####### Exception: There are no releases to deduct from!
					MessageBox.Show("The difference entered exceeds the total quantity available for release.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			else 
			{
				// Delta is negative, deduct from releases
				delta = Math.Abs(delta);

				// Find existing releases for this VIN and deduct from them until delta reached
				var vinRels = ds.OrderRel.Where<SalesOrderDataSet.OrderRelRow>(r => ((Guid)r["VinID_c"]).ToString("D") == vinId.ToString("D"));
							
				if (vinRels.Count > 0)
				{
					if (delta > vinRels.Sum(r => r.SellingReqQty))
					{
						// ####### Exception: If the negative delta is larger than the total quantity for all the releases for this VIN, abort
						MessageBox.Show("The difference entered exceeds the total quantity currently released for this VIN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						var relIndex = vinRels.Count - 1;
						var toDeduct = delta;
						while (toDeduct > 0)
						{
							// Deduct from this release ...
							vinRels[relIndex].SellingReqQty -= 1;
							vinRels[relIndex].RowMod = "U";
							toDeduct -= 1;
	
							if (vinRels[relIndex].SellingReqQty == 0)
							{
								// ... until empty or delta reached, then delete and move on to the next.
								bo.Update(ds);
								var toDelete = ds.OrderRel.FirstOrDefault<SalesOrderDataSet.OrderRelRow>(r => ((Guid)r["VinID_c"]).ToString("D") == vinId.ToString("D"));
								if (toDelete != null) vinRels[relIndex].Delete();
								relIndex -= 1;
							}
						}
					}
				}
				else
				{
					// ####### Exception: There are no releases to deduct from!
					MessageBox.Show("The difference entered exceeds the total quantity available for release.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

			}

			bo.Update(ds);
			
		}

		e.Cancel = true;		
		
		oTrans.Refresh();
		
	}
	
	
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


	private void edvOrderDtl_EpiViewNotification(EpiDataView view, EpiNotifyArgs args)
	{
		// ** Argument Properties and Uses **
		// view.dataView[args.Row]["FieldName"]
		// args.Row, args.Column, args.Sender, args.NotifyType
		// NotifyType.Initialize, NotifyType.AddRow, NotifyType.DeleteRow, NotifyType.InitLastView, NotifyType.InitAndResetTreeNodes
		if ((args.NotifyType == EpiTransaction.NotifyType.Initialize))
		{
			if ((args.Row > -1))
			{
				using (var bo = oTrans.GetNewBO<SalesOrderImpl, SalesOrderSvcContract>())
				{
					bool morePages = false;
					var orderNum = (int)edvOrderHed.dataView[edvOrderHed.Row]["OrderNum"];
					var orderLine = (int)view.dataView[args.Row]["OrderLine"];

					var ds = bo.GetRows("OrderNum = " + orderNum.ToString(), "", "", "", "", "", "OrderLine = " + orderLine.ToString(), "", "", "", "", "", "", "", "", "", "", -1, -1, out morePages);
					nedLineQty.Visible = (ds.OrderRel.Count > 1);
					nedLineQty.Value = (decimal)view.dataView[args.Row]["SellingQuantity"];
				}
			}
		}
	}

	private void nedLineQty_Validated(object sender, System.EventArgs args)
	{
		using (var bo = oTrans.GetNewBO<SalesOrderImpl, SalesOrderSvcContract>())
		{
			bool morePages = false;
			var orderNum = (int)edvOrderHed.dataView[edvOrderHed.Row]["OrderNum"];
			var orderLine = (int)edvOrderDtl.dataView[edvOrderDtl.Row]["OrderLine"];

			var ds = bo.GetRows("OrderNum = " + orderNum.ToString(), "", "", "", "", "", "OrderLine = " + orderLine.ToString(), "", "", "", "", "", "", "", "", "", "", -1, -1, out morePages);

			// Calculate delta between existing quantity and compare to quantity attributed to first release.
			var oldLineQty = (decimal)edvOrderDtl.dataView[edvOrderDtl.Row]["OrderQty"];
			var newLineQty = (decimal)nedLineQty.Value;			
			var firstRel = ds.OrderRel.FirstOrDefault<SalesOrderDataSet.OrderRelRow>(r => r.OrderRelNum == 1);
			var firstRelQty = (decimal)firstRel.SellingReqQty;
			var delta = newLineQty - oldLineQty;

			// If the delta is positive, we need to add to the first release's quantity.
			if (delta > 0)
			{
				firstRel.SellingReqQty += delta;
				firstRel.RowMod = "U";
				bo.Update(ds);
				oTrans.Refresh();
			}
			else
			{
				// Otherwise we need to check if there is enough quantity on the first release to deduct the delta from.
				delta = Math.Abs(delta);
				if (delta <= firstRelQty)
				{
					// If the delta is lower than the quantity available on the first release, lower the quantity on the first release.
					firstRel.SellingReqQty -= delta;
					firstRel.RowMod = "U";
					bo.Update(ds);
					oTrans.Refresh();
				}
				else
				{
					// If the delta is higher than the quantity available on the first release, throw an error.
					MessageBox.Show("You can only reduce the quantity of the detail line by the quantity attributed to the first release for the line.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					nedLineQty.Value = (decimal)edvOrderDtl.dataView[edvOrderDtl.Row]["OrderQty"];
				}
			}

		
		}


	}

	private void btnEndCustomer_Click(object sender, System.EventArgs args)
	{
		// ** Place Event Handling Code Here **
		SearchOnCustomerAdapterShowDialog();
	}

	private void SearchOnCustomerAdapterShowDialog()
	{
		// Wizard Generated Search Method
		// You will need to call this method from another method in custom code
		// For example, [Form]_Load or [Button]_Click

		bool recSelected;
		string whereClause = string.Empty;
		System.Data.DataSet dsCustomerAdapter = Ice.UI.FormFunctions.SearchFunctions.listLookup(this.oTrans, "CustomerAdapter", out recSelected, true, whereClause);
		if (recSelected)
		{
			System.Data.DataRow adapterRow = dsCustomerAdapter.Tables[0].Rows[0];

			// Map Search Fields to Application Fields
			EpiDataView edvOrderHed = ((EpiDataView)(this.oTrans.EpiDataViews["OrderHed"]));
			System.Data.DataRow edvOrderHedRow = edvOrderHed.CurrentDataRow;
			if ((edvOrderHedRow != null))
			{
				edvOrderHedRow.BeginEdit();
				edvOrderHedRow["EndCustomer_c"] = adapterRow["CustID"];
				edvOrderHedRow.EndEdit();
			}
		}
	}
}
