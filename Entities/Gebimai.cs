// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from gebimai_com on 2012-04-29 08:52:51Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
using System;
using System.ComponentModel;
using System.Data;
#if MONO_STRICT
	using System.Data.Linq;
#else   // MONO_STRICT
	using DbLinq.Data.Linq;
	using DbLinq.Vendor;
#endif  // MONO_STRICT
	using System.Data.Linq.Mapping;
using System.Diagnostics;


public partial class gebimaicom : DataContext
{
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		#endregion
	
	
	public gebimaicom(string connectionString) : 
			base(connectionString)
	{
		this.OnCreated();
	}
	
	public gebimaicom(string connection, MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		this.OnCreated();
	}
	
	public gebimaicom(IDbConnection connection, MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		this.OnCreated();
	}
	
	public Table<address> addresses
	{
		get
		{
			return this.GetTable<address>();
		}
	}
	
	public Table<admin> admins
	{
		get
		{
			return this.GetTable<admin>();
		}
	}
	
	public Table<barcode> barcodes
	{
		get
		{
			return this.GetTable<barcode>();
		}
	}
	
	public Table<category> categories
	{
		get
		{
			return this.GetTable<category>();
		}
	}
	
	public Table<dev> dev
	{
		get
		{
			return this.GetTable<dev>();
		}
	}
	
	public Table<order> orders
	{
		get
		{
			return this.GetTable<order>();
		}
	}
	
	public Table<product> products
	{
		get
		{
			return this.GetTable<product>();
		}
	}
	
	public Table<rebot> rebot
	{
		get
		{
			return this.GetTable<rebot>();
		}
	}
	
	public Table<secret> secrets
	{
		get
		{
			return this.GetTable<secret>();
		}
	}
	
	public Table<sender> senders
	{
		get
		{
			return this.GetTable<sender>();
		}
	}
	
	public Table<stock> stocks
	{
		get
		{
			return this.GetTable<stock>();
		}
	}
	
	public Table<timespan> timespans
	{
		get
		{
			return this.GetTable<timespan>();
		}
	}
	
	public Table<user> users
	{
		get
		{
			return this.GetTable<user>();
		}
	}
	
	public Table<wbdialog> wbdialogs
	{
		get
		{
			return this.GetTable<wbdialog>();
		}
	}
	
	public Table<wbstate> wbstates
	{
		get
		{
			return this.GetTable<wbstate>();
		}
	}
}

#region Start MONO_STRICT
#if MONO_STRICT

public partial class gebimaicom
{
	
	public gebimaicom(IDbConnection connection) : 
			base(connection)
	{
		this.OnCreated();
	}
}
#region End MONO_STRICT
	#endregion
#else     // MONO_STRICT

public partial class gebimaicom
{
	
	public gebimaicom(IDbConnection connection) : 
			base(connection, new DbLinq.MySql.MySqlVendor())
	{
		this.OnCreated();
	}
	
	public gebimaicom(IDbConnection connection, IVendor sqlDialect) : 
			base(connection, sqlDialect)
	{
		this.OnCreated();
	}
	
	public gebimaicom(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) : 
			base(connection, mappingSource, sqlDialect)
	{
		this.OnCreated();
	}
}
#region End Not MONO_STRICT
	#endregion
#endif     // MONO_STRICT
#endregion

[Table(Name="gebimai_com.address")]
public partial class address : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _address1;
	
	private string _area;
	
	private int _id;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void Onaddress1Changed();
		
		partial void Onaddress1Changing(string value);
		
		partial void OnareaChanged();
		
		partial void OnareaChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		#endregion
	
	
	public address()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_address1", Name="address", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string address1
	{
		get
		{
			return this._address1;
		}
		set
		{
			if (((_address1 == value) 
						== false))
			{
				this.Onaddress1Changing(value);
				this.SendPropertyChanging();
				this._address1 = value;
				this.SendPropertyChanged("address1");
				this.Onaddress1Changed();
			}
		}
	}
	
	[Column(Storage="_area", Name="area", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string area
	{
		get
		{
			return this._area;
		}
		set
		{
			if (((_area == value) 
						== false))
			{
				this.OnareaChanging(value);
				this.SendPropertyChanging();
				this._area = value;
				this.SendPropertyChanged("area");
				this.OnareaChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.admin")]
public partial class admin : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _alias;
	
	private string _area;
	
	private int _id;
	
	private int _userid;
	
	private EntityRef<user> _user = new EntityRef<user>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnaliasChanged();
		
		partial void OnaliasChanging(string value);
		
		partial void OnareaChanged();
		
		partial void OnareaChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnuseridChanged();
		
		partial void OnuseridChanging(int value);
		#endregion
	
	
	public admin()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_alias", Name="alias", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string alias
	{
		get
		{
			return this._alias;
		}
		set
		{
			if (((_alias == value) 
						== false))
			{
				this.OnaliasChanging(value);
				this.SendPropertyChanging();
				this._alias = value;
				this.SendPropertyChanged("alias");
				this.OnaliasChanged();
			}
		}
	}
	
	[Column(Storage="_area", Name="area", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string area
	{
		get
		{
			return this._area;
		}
		set
		{
			if (((_area == value) 
						== false))
			{
				this.OnareaChanging(value);
				this.SendPropertyChanging();
				this._area = value;
				this.SendPropertyChanged("area");
				this.OnareaChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_userid", Name="user_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int userid
	{
		get
		{
			return this._userid;
		}
		set
		{
			if ((_userid != value))
			{
				this.OnuseridChanging(value);
				this.SendPropertyChanging();
				this._userid = value;
				this.SendPropertyChanged("userid");
				this.OnuseridChanged();
			}
		}
	}
	
	#region Parents
	[Association(Storage="_user", OtherKey="id", ThisKey="userid", Name="admin_ibfk_1", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public user user
	{
		get
		{
			return this._user.Entity;
		}
		set
		{
			if (((this._user.Entity == value) 
						== false))
			{
				if ((this._user.Entity != null))
				{
					user previoususer = this._user.Entity;
					this._user.Entity = null;
					previoususer.admins.Remove(this);
				}
				this._user.Entity = value;
				if ((value != null))
				{
					value.admins.Add(this);
					_userid = value.id;
				}
				else
				{
					_userid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.barcode")]
public partial class barcode : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _bc;
	
	private string _imgUrl;
	
	private string _note;
	
	private int _num;
	
	private int _price;
	
	private int _productid;
	
	private string _title;
	
	private string _url;
	
	private EntitySet<stock> _stocks;
	
	private EntityRef<product> _product = new EntityRef<product>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnbcChanged();
		
		partial void OnbcChanging(string value);
		
		partial void OnimgUrlChanged();
		
		partial void OnimgUrlChanging(string value);
		
		partial void OnnoteChanged();
		
		partial void OnnoteChanging(string value);
		
		partial void OnnumChanged();
		
		partial void OnnumChanging(int value);
		
		partial void OnpriceChanged();
		
		partial void OnpriceChanging(int value);
		
		partial void OnproductidChanged();
		
		partial void OnproductidChanging(int value);
		
		partial void OntitleChanged();
		
		partial void OntitleChanging(string value);
		
		partial void OnurlChanged();
		
		partial void OnurlChanging(string value);
		#endregion
	
	
	public barcode()
	{
		_stocks = new EntitySet<stock>(new Action<stock>(this.stocks_Attach), new Action<stock>(this.stocks_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_bc", Name="bc", DbType="varchar(15)", IsPrimaryKey=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string bc
	{
		get
		{
			return this._bc;
		}
		set
		{
			if (((_bc == value) 
						== false))
			{
				this.OnbcChanging(value);
				this.SendPropertyChanging();
				this._bc = value;
				this.SendPropertyChanged("bc");
				this.OnbcChanged();
			}
		}
	}
	
	[Column(Storage="_imgUrl", Name="imgUrl", DbType="varchar(128)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string imgUrl
	{
		get
		{
			return this._imgUrl;
		}
		set
		{
			if (((_imgUrl == value) 
						== false))
			{
				this.OnimgUrlChanging(value);
				this.SendPropertyChanging();
				this._imgUrl = value;
				this.SendPropertyChanged("imgUrl");
				this.OnimgUrlChanged();
			}
		}
	}
	
	[Column(Storage="_note", Name="note", DbType="varchar(200)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string note
	{
		get
		{
			return this._note;
		}
		set
		{
			if (((_note == value) 
						== false))
			{
				this.OnnoteChanging(value);
				this.SendPropertyChanging();
				this._note = value;
				this.SendPropertyChanged("note");
				this.OnnoteChanged();
			}
		}
	}
	
	[Column(Storage="_num", Name="num", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int num
	{
		get
		{
			return this._num;
		}
		set
		{
			if ((_num != value))
			{
				this.OnnumChanging(value);
				this.SendPropertyChanging();
				this._num = value;
				this.SendPropertyChanged("num");
				this.OnnumChanged();
			}
		}
	}
	
	[Column(Storage="_price", Name="price", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int price
	{
		get
		{
			return this._price;
		}
		set
		{
			if ((_price != value))
			{
				this.OnpriceChanging(value);
				this.SendPropertyChanging();
				this._price = value;
				this.SendPropertyChanged("price");
				this.OnpriceChanged();
			}
		}
	}
	
	[Column(Storage="_productid", Name="product_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int productid
	{
		get
		{
			return this._productid;
		}
		set
		{
			if ((_productid != value))
			{
				this.OnproductidChanging(value);
				this.SendPropertyChanging();
				this._productid = value;
				this.SendPropertyChanged("productid");
				this.OnproductidChanged();
			}
		}
	}
	
	[Column(Storage="_title", Name="title", DbType="varchar(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			if (((_title == value) 
						== false))
			{
				this.OntitleChanging(value);
				this.SendPropertyChanging();
				this._title = value;
				this.SendPropertyChanged("title");
				this.OntitleChanged();
			}
		}
	}
	
	[Column(Storage="_url", Name="url", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string url
	{
		get
		{
			return this._url;
		}
		set
		{
			if (((_url == value) 
						== false))
			{
				this.OnurlChanging(value);
				this.SendPropertyChanging();
				this._url = value;
				this.SendPropertyChanged("url");
				this.OnurlChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_stocks", OtherKey="barcodebc", ThisKey="bc", Name="stock_ibfk_1")]
	[DebuggerNonUserCode()]
	public EntitySet<stock> stocks
	{
		get
		{
			return this._stocks;
		}
		set
		{
			this._stocks = value;
		}
	}
	#endregion
	
	#region Parents
	[Association(Storage="_product", OtherKey="id", ThisKey="productid", Name="barcode_ibfk_1", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public product product
	{
		get
		{
			return this._product.Entity;
		}
		set
		{
			if (((this._product.Entity == value) 
						== false))
			{
				if ((this._product.Entity != null))
				{
					product previousproduct = this._product.Entity;
					this._product.Entity = null;
					previousproduct.barcodes.Remove(this);
				}
				this._product.Entity = value;
				if ((value != null))
				{
					value.barcodes.Add(this);
					_productid = value.id;
				}
				else
				{
					_productid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void stocks_Attach(stock entity)
	{
		this.SendPropertyChanging();
		entity.barcode = this;
	}
	
	private void stocks_Detach(stock entity)
	{
		this.SendPropertyChanging();
		entity.barcode = null;
	}
	#endregion
}

[Table(Name="gebimai_com.category")]
public partial class category : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private int _id;
	
	private int _sort;
	
	private string _title;
	
	private EntitySet<product> _products;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnsortChanged();
		
		partial void OnsortChanging(int value);
		
		partial void OntitleChanged();
		
		partial void OntitleChanging(string value);
		#endregion
	
	
	public category()
	{
		_products = new EntitySet<product>(new Action<product>(this.products_Attach), new Action<product>(this.products_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_sort", Name="sort", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int sort
	{
		get
		{
			return this._sort;
		}
		set
		{
			if ((_sort != value))
			{
				this.OnsortChanging(value);
				this.SendPropertyChanging();
				this._sort = value;
				this.SendPropertyChanged("sort");
				this.OnsortChanged();
			}
		}
	}
	
	[Column(Storage="_title", Name="title", DbType="varchar(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			if (((_title == value) 
						== false))
			{
				this.OntitleChanging(value);
				this.SendPropertyChanging();
				this._title = value;
				this.SendPropertyChanged("title");
				this.OntitleChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_products", OtherKey="categoryid", ThisKey="id", Name="product_ibfk_2")]
	[DebuggerNonUserCode()]
	public EntitySet<product> products
	{
		get
		{
			return this._products;
		}
		set
		{
			this._products = value;
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void products_Attach(product entity)
	{
		this.SendPropertyChanging();
		entity.category = this;
	}
	
	private void products_Detach(product entity)
	{
		this.SendPropertyChanging();
		entity.category = null;
	}
	#endregion
}

[Table(Name="gebimai_com.dev")]
public partial class dev : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _alias;
	
	private int _id;
	
	private int _userid;
	
	private EntityRef<user> _user = new EntityRef<user>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnaliasChanged();
		
		partial void OnaliasChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnuseridChanged();
		
		partial void OnuseridChanging(int value);
		#endregion
	
	
	public dev()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_alias", Name="alias", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string alias
	{
		get
		{
			return this._alias;
		}
		set
		{
			if (((_alias == value) 
						== false))
			{
				this.OnaliasChanging(value);
				this.SendPropertyChanging();
				this._alias = value;
				this.SendPropertyChanged("alias");
				this.OnaliasChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_userid", Name="user_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int userid
	{
		get
		{
			return this._userid;
		}
		set
		{
			if ((_userid != value))
			{
				this.OnuseridChanging(value);
				this.SendPropertyChanging();
				this._userid = value;
				this.SendPropertyChanged("userid");
				this.OnuseridChanged();
			}
		}
	}
	
	#region Parents
	[Association(Storage="_user", OtherKey="id", ThisKey="userid", Name="dev_ibfk_1", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public user user
	{
		get
		{
			return this._user.Entity;
		}
		set
		{
			if (((this._user.Entity == value) 
						== false))
			{
				if ((this._user.Entity != null))
				{
					user previoususer = this._user.Entity;
					this._user.Entity = null;
					previoususer.dev.Remove(this);
				}
				this._user.Entity = value;
				if ((value != null))
				{
					value.dev.Add(this);
					_userid = value.id;
				}
				else
				{
					_userid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.order")]
public partial class order : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _address1;
	
	private string _address2;
	
	private string _alipay;
	
	private int _id;
	
	private int _num;
	
	private int _state;
	
	private int _stockid;
	
	private int _sum;
	
	private System.DateTime _time;
	
	private int _timespanid;
	
	private EntityRef<timespan> _timespan = new EntityRef<timespan>();
	
	private EntityRef<stock> _stock = new EntityRef<stock>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void Onaddress1Changed();
		
		partial void Onaddress1Changing(string value);
		
		partial void Onaddress2Changed();
		
		partial void Onaddress2Changing(string value);
		
		partial void OnalipayChanged();
		
		partial void OnalipayChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnnumChanged();
		
		partial void OnnumChanging(int value);
		
		partial void OnstateChanged();
		
		partial void OnstateChanging(int value);
		
		partial void OnstockidChanged();
		
		partial void OnstockidChanging(int value);
		
		partial void OnsumChanged();
		
		partial void OnsumChanging(int value);
		
		partial void OntimeChanged();
		
		partial void OntimeChanging(System.DateTime value);
		
		partial void OntimespanidChanged();
		
		partial void OntimespanidChanging(int value);
		#endregion
	
	
	public order()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_address1", Name="address1", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string address1
	{
		get
		{
			return this._address1;
		}
		set
		{
			if (((_address1 == value) 
						== false))
			{
				this.Onaddress1Changing(value);
				this.SendPropertyChanging();
				this._address1 = value;
				this.SendPropertyChanged("address1");
				this.Onaddress1Changed();
			}
		}
	}
	
	[Column(Storage="_address2", Name="address2", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string address2
	{
		get
		{
			return this._address2;
		}
		set
		{
			if (((_address2 == value) 
						== false))
			{
				this.Onaddress2Changing(value);
				this.SendPropertyChanging();
				this._address2 = value;
				this.SendPropertyChanged("address2");
				this.Onaddress2Changed();
			}
		}
	}
	
	[Column(Storage="_alipay", Name="alipay", DbType="varchar(100)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string alipay
	{
		get
		{
			return this._alipay;
		}
		set
		{
			if (((_alipay == value) 
						== false))
			{
				this.OnalipayChanging(value);
				this.SendPropertyChanging();
				this._alipay = value;
				this.SendPropertyChanged("alipay");
				this.OnalipayChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_num", Name="num", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int num
	{
		get
		{
			return this._num;
		}
		set
		{
			if ((_num != value))
			{
				this.OnnumChanging(value);
				this.SendPropertyChanging();
				this._num = value;
				this.SendPropertyChanged("num");
				this.OnnumChanged();
			}
		}
	}
	
	[Column(Storage="_state", Name="state", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int state
	{
		get
		{
			return this._state;
		}
		set
		{
			if ((_state != value))
			{
				this.OnstateChanging(value);
				this.SendPropertyChanging();
				this._state = value;
				this.SendPropertyChanged("state");
				this.OnstateChanged();
			}
		}
	}
	
	[Column(Storage="_stockid", Name="stock_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int stockid
	{
		get
		{
			return this._stockid;
		}
		set
		{
			if ((_stockid != value))
			{
				this.OnstockidChanging(value);
				this.SendPropertyChanging();
				this._stockid = value;
				this.SendPropertyChanged("stockid");
				this.OnstockidChanged();
			}
		}
	}
	
	[Column(Storage="_sum", Name="sum", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int sum
	{
		get
		{
			return this._sum;
		}
		set
		{
			if ((_sum != value))
			{
				this.OnsumChanging(value);
				this.SendPropertyChanging();
				this._sum = value;
				this.SendPropertyChanged("sum");
				this.OnsumChanged();
			}
		}
	}
	
	[Column(Storage="_time", Name="time", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.DateTime time
	{
		get
		{
			return this._time;
		}
		set
		{
			if ((_time != value))
			{
				this.OntimeChanging(value);
				this.SendPropertyChanging();
				this._time = value;
				this.SendPropertyChanged("time");
				this.OntimeChanged();
			}
		}
	}
	
	[Column(Storage="_timespanid", Name="timespan_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int timespanid
	{
		get
		{
			return this._timespanid;
		}
		set
		{
			if ((_timespanid != value))
			{
				this.OntimespanidChanging(value);
				this.SendPropertyChanging();
				this._timespanid = value;
				this.SendPropertyChanged("timespanid");
				this.OntimespanidChanged();
			}
		}
	}
	
	#region Parents
	[Association(Storage="_timespan", OtherKey="id", ThisKey="timespanid", Name="order_ibfk_2", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public timespan timespan
	{
		get
		{
			return this._timespan.Entity;
		}
		set
		{
			if (((this._timespan.Entity == value) 
						== false))
			{
				if ((this._timespan.Entity != null))
				{
					timespan previoustimespan = this._timespan.Entity;
					this._timespan.Entity = null;
					previoustimespan.orders.Remove(this);
				}
				this._timespan.Entity = value;
				if ((value != null))
				{
					value.orders.Add(this);
					_timespanid = value.id;
				}
				else
				{
					_timespanid = default(int);
				}
			}
		}
	}
	
	[Association(Storage="_stock", OtherKey="id", ThisKey="stockid", Name="order_ibfk_3", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public stock stock
	{
		get
		{
			return this._stock.Entity;
		}
		set
		{
			if (((this._stock.Entity == value) 
						== false))
			{
				if ((this._stock.Entity != null))
				{
					stock previousstock = this._stock.Entity;
					this._stock.Entity = null;
					previousstock.orders.Remove(this);
				}
				this._stock.Entity = value;
				if ((value != null))
				{
					value.orders.Add(this);
					_stockid = value.id;
				}
				else
				{
					_stockid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.product")]
public partial class product : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private int _categoryid;
	
	private int _id;
	
	private sbyte _isInstante;
	
	private int _sort;
	
	private string _title;
	
	private EntitySet<barcode> _barcodes;
	
	private EntityRef<category> _category = new EntityRef<category>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OncategoryidChanged();
		
		partial void OncategoryidChanging(int value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnisInstanteChanged();
		
		partial void OnisInstanteChanging(sbyte value);
		
		partial void OnsortChanged();
		
		partial void OnsortChanging(int value);
		
		partial void OntitleChanged();
		
		partial void OntitleChanging(string value);
		#endregion
	
	
	public product()
	{
		_barcodes = new EntitySet<barcode>(new Action<barcode>(this.barcodes_Attach), new Action<barcode>(this.barcodes_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_categoryid", Name="category_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int categoryid
	{
		get
		{
			return this._categoryid;
		}
		set
		{
			if ((_categoryid != value))
			{
				this.OncategoryidChanging(value);
				this.SendPropertyChanging();
				this._categoryid = value;
				this.SendPropertyChanged("categoryid");
				this.OncategoryidChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_isInstante", Name="isInstante", DbType="tinyint(1)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public sbyte isInstante
	{
		get
		{
			return this._isInstante;
		}
		set
		{
			if ((_isInstante != value))
			{
				this.OnisInstanteChanging(value);
				this.SendPropertyChanging();
				this._isInstante = value;
				this.SendPropertyChanged("isInstante");
				this.OnisInstanteChanged();
			}
		}
	}
	
	[Column(Storage="_sort", Name="sort", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int sort
	{
		get
		{
			return this._sort;
		}
		set
		{
			if ((_sort != value))
			{
				this.OnsortChanging(value);
				this.SendPropertyChanging();
				this._sort = value;
				this.SendPropertyChanged("sort");
				this.OnsortChanged();
			}
		}
	}
	
	[Column(Storage="_title", Name="title", DbType="varchar(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string title
	{
		get
		{
			return this._title;
		}
		set
		{
			if (((_title == value) 
						== false))
			{
				this.OntitleChanging(value);
				this.SendPropertyChanging();
				this._title = value;
				this.SendPropertyChanged("title");
				this.OntitleChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_barcodes", OtherKey="productid", ThisKey="id", Name="barcode_ibfk_1")]
	[DebuggerNonUserCode()]
	public EntitySet<barcode> barcodes
	{
		get
		{
			return this._barcodes;
		}
		set
		{
			this._barcodes = value;
		}
	}
	#endregion
	
	#region Parents
	[Association(Storage="_category", OtherKey="id", ThisKey="categoryid", Name="product_ibfk_2", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public category category
	{
		get
		{
			return this._category.Entity;
		}
		set
		{
			if (((this._category.Entity == value) 
						== false))
			{
				if ((this._category.Entity != null))
				{
					category previouscategory = this._category.Entity;
					this._category.Entity = null;
					previouscategory.products.Remove(this);
				}
				this._category.Entity = value;
				if ((value != null))
				{
					value.products.Add(this);
					_categoryid = value.id;
				}
				else
				{
					_categoryid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void barcodes_Attach(barcode entity)
	{
		this.SendPropertyChanging();
		entity.product = this;
	}
	
	private void barcodes_Detach(barcode entity)
	{
		this.SendPropertyChanging();
		entity.product = null;
	}
	#endregion
}

[Table(Name="gebimai_com.rebot")]
public partial class rebot : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _content;
	
	private int _id;
	
	private string _keyword;
	
	private string _lockdev;
	
	private System.DateTime _locktime;
	
	private int _sort;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OncontentChanged();
		
		partial void OncontentChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnkeywordChanged();
		
		partial void OnkeywordChanging(string value);
		
		partial void OnlockdevChanged();
		
		partial void OnlockdevChanging(string value);
		
		partial void OnlocktimeChanged();
		
		partial void OnlocktimeChanging(System.DateTime value);
		
		partial void OnsortChanged();
		
		partial void OnsortChanging(int value);
		#endregion
	
	
	public rebot()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_content", Name="content", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string content
	{
		get
		{
			return this._content;
		}
		set
		{
			if (((_content == value) 
						== false))
			{
				this.OncontentChanging(value);
				this.SendPropertyChanging();
				this._content = value;
				this.SendPropertyChanged("content");
				this.OncontentChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_keyword", Name="keyword", DbType="varchar(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string keyword
	{
		get
		{
			return this._keyword;
		}
		set
		{
			if (((_keyword == value) 
						== false))
			{
				this.OnkeywordChanging(value);
				this.SendPropertyChanging();
				this._keyword = value;
				this.SendPropertyChanged("keyword");
				this.OnkeywordChanged();
			}
		}
	}
	
	[Column(Storage="_lockdev", Name="lockdev", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string lockdev
	{
		get
		{
			return this._lockdev;
		}
		set
		{
			if (((_lockdev == value) 
						== false))
			{
				this.OnlockdevChanging(value);
				this.SendPropertyChanging();
				this._lockdev = value;
				this.SendPropertyChanged("lockdev");
				this.OnlockdevChanged();
			}
		}
	}
	
	[Column(Storage="_locktime", Name="locktime", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.DateTime locktime
	{
		get
		{
			return this._locktime;
		}
		set
		{
			if ((_locktime != value))
			{
				this.OnlocktimeChanging(value);
				this.SendPropertyChanging();
				this._locktime = value;
				this.SendPropertyChanged("locktime");
				this.OnlocktimeChanged();
			}
		}
	}
	
	[Column(Storage="_sort", Name="sort", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int sort
	{
		get
		{
			return this._sort;
		}
		set
		{
			if ((_sort != value))
			{
				this.OnsortChanging(value);
				this.SendPropertyChanging();
				this._sort = value;
				this.SendPropertyChanged("sort");
				this.OnsortChanged();
			}
		}
	}
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.secret")]
public partial class secret : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _authToken;
	
	private System.DateTime _authTokenExpire;
	
	private string _data;
	
	private int _id;
	
	private int _logon;
	
	private int _userid;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnauthTokenChanged();
		
		partial void OnauthTokenChanging(string value);
		
		partial void OnauthTokenExpireChanged();
		
		partial void OnauthTokenExpireChanging(System.DateTime value);
		
		partial void OndataChanged();
		
		partial void OndataChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnlogonChanged();
		
		partial void OnlogonChanging(int value);
		
		partial void OnuseridChanged();
		
		partial void OnuseridChanging(int value);
		#endregion
	
	
	public secret()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_authToken", Name="authToken", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string authToken
	{
		get
		{
			return this._authToken;
		}
		set
		{
			if (((_authToken == value) 
						== false))
			{
				this.OnauthTokenChanging(value);
				this.SendPropertyChanging();
				this._authToken = value;
				this.SendPropertyChanged("authToken");
				this.OnauthTokenChanged();
			}
		}
	}
	
	[Column(Storage="_authTokenExpire", Name="authTokenExpire", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.DateTime authTokenExpire
	{
		get
		{
			return this._authTokenExpire;
		}
		set
		{
			if ((_authTokenExpire != value))
			{
				this.OnauthTokenExpireChanging(value);
				this.SendPropertyChanging();
				this._authTokenExpire = value;
				this.SendPropertyChanged("authTokenExpire");
				this.OnauthTokenExpireChanged();
			}
		}
	}
	
	[Column(Storage="_data", Name="data", DbType="varchar(200)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string data
	{
		get
		{
			return this._data;
		}
		set
		{
			if (((_data == value) 
						== false))
			{
				this.OndataChanging(value);
				this.SendPropertyChanging();
				this._data = value;
				this.SendPropertyChanged("data");
				this.OndataChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_logon", Name="logon", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int logon
	{
		get
		{
			return this._logon;
		}
		set
		{
			if ((_logon != value))
			{
				this.OnlogonChanging(value);
				this.SendPropertyChanging();
				this._logon = value;
				this.SendPropertyChanged("logon");
				this.OnlogonChanged();
			}
		}
	}
	
	[Column(Storage="_userid", Name="user_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int userid
	{
		get
		{
			return this._userid;
		}
		set
		{
			if ((_userid != value))
			{
				this.OnuseridChanging(value);
				this.SendPropertyChanging();
				this._userid = value;
				this.SendPropertyChanged("userid");
				this.OnuseridChanged();
			}
		}
	}
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.sender")]
public partial class sender : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _addresses;
	
	private string _alias;
	
	private string _area;
	
	private int _id;
	
	private int _interval;
	
	private int _intervalConnected;
	
	private int _userid;
	
	private EntitySet<timespan> _timespans;
	
	private EntityRef<user> _user = new EntityRef<user>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnaddressesChanged();
		
		partial void OnaddressesChanging(string value);
		
		partial void OnaliasChanged();
		
		partial void OnaliasChanging(string value);
		
		partial void OnareaChanged();
		
		partial void OnareaChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnintervalChanged();
		
		partial void OnintervalChanging(int value);
		
		partial void OnintervalConnectedChanged();
		
		partial void OnintervalConnectedChanging(int value);
		
		partial void OnuseridChanged();
		
		partial void OnuseridChanging(int value);
		#endregion
	
	
	public sender()
	{
		_timespans = new EntitySet<timespan>(new Action<timespan>(this.timespans_Attach), new Action<timespan>(this.timespans_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_addresses", Name="addresses", DbType="varchar(1000)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string addresses
	{
		get
		{
			return this._addresses;
		}
		set
		{
			if (((_addresses == value) 
						== false))
			{
				this.OnaddressesChanging(value);
				this.SendPropertyChanging();
				this._addresses = value;
				this.SendPropertyChanged("addresses");
				this.OnaddressesChanged();
			}
		}
	}
	
	[Column(Storage="_alias", Name="alias", DbType="varchar(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string alias
	{
		get
		{
			return this._alias;
		}
		set
		{
			if (((_alias == value) 
						== false))
			{
				this.OnaliasChanging(value);
				this.SendPropertyChanging();
				this._alias = value;
				this.SendPropertyChanged("alias");
				this.OnaliasChanged();
			}
		}
	}
	
	[Column(Storage="_area", Name="area", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string area
	{
		get
		{
			return this._area;
		}
		set
		{
			if (((_area == value) 
						== false))
			{
				this.OnareaChanging(value);
				this.SendPropertyChanging();
				this._area = value;
				this.SendPropertyChanged("area");
				this.OnareaChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_interval", Name="interval", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int interval
	{
		get
		{
			return this._interval;
		}
		set
		{
			if ((_interval != value))
			{
				this.OnintervalChanging(value);
				this.SendPropertyChanging();
				this._interval = value;
				this.SendPropertyChanged("interval");
				this.OnintervalChanged();
			}
		}
	}
	
	[Column(Storage="_intervalConnected", Name="intervalConnected", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int intervalConnected
	{
		get
		{
			return this._intervalConnected;
		}
		set
		{
			if ((_intervalConnected != value))
			{
				this.OnintervalConnectedChanging(value);
				this.SendPropertyChanging();
				this._intervalConnected = value;
				this.SendPropertyChanged("intervalConnected");
				this.OnintervalConnectedChanged();
			}
		}
	}
	
	[Column(Storage="_userid", Name="user_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int userid
	{
		get
		{
			return this._userid;
		}
		set
		{
			if ((_userid != value))
			{
				this.OnuseridChanging(value);
				this.SendPropertyChanging();
				this._userid = value;
				this.SendPropertyChanged("userid");
				this.OnuseridChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_timespans", OtherKey="senderid", ThisKey="id", Name="timespan_ibfk_1")]
	[DebuggerNonUserCode()]
	public EntitySet<timespan> timespans
	{
		get
		{
			return this._timespans;
		}
		set
		{
			this._timespans = value;
		}
	}
	#endregion
	
	#region Parents
	[Association(Storage="_user", OtherKey="id", ThisKey="userid", Name="sender_ibfk_1", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public user user
	{
		get
		{
			return this._user.Entity;
		}
		set
		{
			if (((this._user.Entity == value) 
						== false))
			{
				if ((this._user.Entity != null))
				{
					user previoususer = this._user.Entity;
					this._user.Entity = null;
					previoususer.senders.Remove(this);
				}
				this._user.Entity = value;
				if ((value != null))
				{
					value.senders.Add(this);
					_userid = value.id;
				}
				else
				{
					_userid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void timespans_Attach(timespan entity)
	{
		this.SendPropertyChanging();
		entity.sender = this;
	}
	
	private void timespans_Detach(timespan entity)
	{
		this.SendPropertyChanging();
		entity.sender = null;
	}
	#endregion
}

[Table(Name="gebimai_com.stock")]
public partial class stock : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _area;
	
	private string _barcodebc;
	
	private bool _enabled;
	
	private int _id;
	
	private int _importprice;
	
	private int _price;
	
	private EntitySet<order> _orders;
	
	private EntityRef<barcode> _barcode = new EntityRef<barcode>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnareaChanged();
		
		partial void OnareaChanging(string value);
		
		partial void OnbarcodebcChanged();
		
		partial void OnbarcodebcChanging(string value);
		
		partial void OnenabledChanged();
		
		partial void OnenabledChanging(bool value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnimportpriceChanged();
		
		partial void OnimportpriceChanging(int value);
		
		partial void OnpriceChanged();
		
		partial void OnpriceChanging(int value);
		#endregion
	
	
	public stock()
	{
		_orders = new EntitySet<order>(new Action<order>(this.orders_Attach), new Action<order>(this.orders_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_area", Name="area", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string area
	{
		get
		{
			return this._area;
		}
		set
		{
			if (((_area == value) 
						== false))
			{
				this.OnareaChanging(value);
				this.SendPropertyChanging();
				this._area = value;
				this.SendPropertyChanged("area");
				this.OnareaChanged();
			}
		}
	}
	
	[Column(Storage="_barcodebc", Name="barcode_bc", DbType="varchar(15)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string barcodebc
	{
		get
		{
			return this._barcodebc;
		}
		set
		{
			if (((_barcodebc == value) 
						== false))
			{
				this.OnbarcodebcChanging(value);
				this.SendPropertyChanging();
				this._barcodebc = value;
				this.SendPropertyChanged("barcodebc");
				this.OnbarcodebcChanged();
			}
		}
	}
	
	[Column(Storage="_enabled", Name="enabled", DbType="bit(1)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public bool enabled
	{
		get
		{
			return this._enabled;
		}
		set
		{
			if ((_enabled != value))
			{
				this.OnenabledChanging(value);
				this.SendPropertyChanging();
				this._enabled = value;
				this.SendPropertyChanged("enabled");
				this.OnenabledChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_importprice", Name="importprice", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int importprice
	{
		get
		{
			return this._importprice;
		}
		set
		{
			if ((_importprice != value))
			{
				this.OnimportpriceChanging(value);
				this.SendPropertyChanging();
				this._importprice = value;
				this.SendPropertyChanged("importprice");
				this.OnimportpriceChanged();
			}
		}
	}
	
	[Column(Storage="_price", Name="price", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int price
	{
		get
		{
			return this._price;
		}
		set
		{
			if ((_price != value))
			{
				this.OnpriceChanging(value);
				this.SendPropertyChanging();
				this._price = value;
				this.SendPropertyChanged("price");
				this.OnpriceChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_orders", OtherKey="stockid", ThisKey="id", Name="order_ibfk_3")]
	[DebuggerNonUserCode()]
	public EntitySet<order> orders
	{
		get
		{
			return this._orders;
		}
		set
		{
			this._orders = value;
		}
	}
	#endregion
	
	#region Parents
	[Association(Storage="_barcode", OtherKey="bc", ThisKey="barcodebc", Name="stock_ibfk_1", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public barcode barcode
	{
		get
		{
			return this._barcode.Entity;
		}
		set
		{
			if (((this._barcode.Entity == value) 
						== false))
			{
				if ((this._barcode.Entity != null))
				{
					barcode previousbarcode = this._barcode.Entity;
					this._barcode.Entity = null;
					previousbarcode.stocks.Remove(this);
				}
				this._barcode.Entity = value;
				if ((value != null))
				{
					value.stocks.Add(this);
					_barcodebc = value.bc;
				}
				else
				{
					_barcodebc = default(string);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void orders_Attach(order entity)
	{
		this.SendPropertyChanging();
		entity.stock = this;
	}
	
	private void orders_Detach(order entity)
	{
		this.SendPropertyChanging();
		entity.stock = null;
	}
	#endregion
}

[Table(Name="gebimai_com.timespan")]
public partial class timespan : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private System.DateTime _cur;
	
	private int _id;
	
	private string _lastAddress;
	
	private int _senderid;
	
	private System.DateTime _start;
	
	private System.DateTime _stop;
	
	private EntitySet<order> _orders;
	
	private EntityRef<sender> _sender = new EntityRef<sender>();
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OncurChanged();
		
		partial void OncurChanging(System.DateTime value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnlastAddressChanged();
		
		partial void OnlastAddressChanging(string value);
		
		partial void OnsenderidChanged();
		
		partial void OnsenderidChanging(int value);
		
		partial void OnstartChanged();
		
		partial void OnstartChanging(System.DateTime value);
		
		partial void OnstopChanged();
		
		partial void OnstopChanging(System.DateTime value);
		#endregion
	
	
	public timespan()
	{
		_orders = new EntitySet<order>(new Action<order>(this.orders_Attach), new Action<order>(this.orders_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_cur", Name="cur", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.DateTime cur
	{
		get
		{
			return this._cur;
		}
		set
		{
			if ((_cur != value))
			{
				this.OncurChanging(value);
				this.SendPropertyChanging();
				this._cur = value;
				this.SendPropertyChanged("cur");
				this.OncurChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_lastAddress", Name="lastAddress", DbType="varchar(50)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string lastAddress
	{
		get
		{
			return this._lastAddress;
		}
		set
		{
			if (((_lastAddress == value) 
						== false))
			{
				this.OnlastAddressChanging(value);
				this.SendPropertyChanging();
				this._lastAddress = value;
				this.SendPropertyChanged("lastAddress");
				this.OnlastAddressChanged();
			}
		}
	}
	
	[Column(Storage="_senderid", Name="sender_id", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int senderid
	{
		get
		{
			return this._senderid;
		}
		set
		{
			if ((_senderid != value))
			{
				this.OnsenderidChanging(value);
				this.SendPropertyChanging();
				this._senderid = value;
				this.SendPropertyChanged("senderid");
				this.OnsenderidChanged();
			}
		}
	}
	
	[Column(Storage="_start", Name="start", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.DateTime start
	{
		get
		{
			return this._start;
		}
		set
		{
			if ((_start != value))
			{
				this.OnstartChanging(value);
				this.SendPropertyChanging();
				this._start = value;
				this.SendPropertyChanged("start");
				this.OnstartChanged();
			}
		}
	}
	
	[Column(Storage="_stop", Name="stop", DbType="datetime", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public System.DateTime stop
	{
		get
		{
			return this._stop;
		}
		set
		{
			if ((_stop != value))
			{
				this.OnstopChanging(value);
				this.SendPropertyChanging();
				this._stop = value;
				this.SendPropertyChanged("stop");
				this.OnstopChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_orders", OtherKey="timespanid", ThisKey="id", Name="order_ibfk_2")]
	[DebuggerNonUserCode()]
	public EntitySet<order> orders
	{
		get
		{
			return this._orders;
		}
		set
		{
			this._orders = value;
		}
	}
	#endregion
	
	#region Parents
	[Association(Storage="_sender", OtherKey="id", ThisKey="senderid", Name="timespan_ibfk_1", IsForeignKey=true)]
	[DebuggerNonUserCode()]
	public sender sender
	{
		get
		{
			return this._sender.Entity;
		}
		set
		{
			if (((this._sender.Entity == value) 
						== false))
			{
				if ((this._sender.Entity != null))
				{
					sender previoussender = this._sender.Entity;
					this._sender.Entity = null;
					previoussender.timespans.Remove(this);
				}
				this._sender.Entity = value;
				if ((value != null))
				{
					value.timespans.Add(this);
					_senderid = value.id;
				}
				else
				{
					_senderid = default(int);
				}
			}
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void orders_Attach(order entity)
	{
		this.SendPropertyChanging();
		entity.timespan = this;
	}
	
	private void orders_Detach(order entity)
	{
		this.SendPropertyChanging();
		entity.timespan = null;
	}
	#endregion
}

[Table(Name="gebimai_com.user")]
public partial class user : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _address;
	
	private string _address2;
	
	private string _alipay;
	
	private string _area;
	
	private string _authID;
	
	private string _authProvider;
	
	private string _avatarUrl;
	
	private string _gender;
	
	private int _id;
	
	private string _message;
	
	private string _name;
	
	private string _username;
	
	private EntitySet<admin> _admins;
	
	private EntitySet<dev> _dev;
	
	private EntitySet<sender> _senders;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnaddressChanged();
		
		partial void OnaddressChanging(string value);
		
		partial void Onaddress2Changed();
		
		partial void Onaddress2Changing(string value);
		
		partial void OnalipayChanged();
		
		partial void OnalipayChanging(string value);
		
		partial void OnareaChanged();
		
		partial void OnareaChanging(string value);
		
		partial void OnauthIdChanged();
		
		partial void OnauthIdChanging(string value);
		
		partial void OnauthProviderChanged();
		
		partial void OnauthProviderChanging(string value);
		
		partial void OnavatarUrlChanged();
		
		partial void OnavatarUrlChanging(string value);
		
		partial void OngenderChanged();
		
		partial void OngenderChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnmessageChanged();
		
		partial void OnmessageChanging(string value);
		
		partial void OnnameChanged();
		
		partial void OnnameChanging(string value);
		
		partial void OnusernameChanged();
		
		partial void OnusernameChanging(string value);
		#endregion
	
	
	public user()
	{
		_admins = new EntitySet<admin>(new Action<admin>(this.admins_Attach), new Action<admin>(this.admins_Detach));
		_dev = new EntitySet<dev>(new Action<dev>(this.dev_Attach), new Action<dev>(this.dev_Detach));
		_senders = new EntitySet<sender>(new Action<sender>(this.senders_Attach), new Action<sender>(this.senders_Detach));
		this.OnCreated();
	}
	
	[Column(Storage="_address", Name="address", DbType="varchar(100)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string address
	{
		get
		{
			return this._address;
		}
		set
		{
			if (((_address == value) 
						== false))
			{
				this.OnaddressChanging(value);
				this.SendPropertyChanging();
				this._address = value;
				this.SendPropertyChanged("address");
				this.OnaddressChanged();
			}
		}
	}
	
	[Column(Storage="_address2", Name="address2", DbType="varchar(50)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string address2
	{
		get
		{
			return this._address2;
		}
		set
		{
			if (((_address2 == value) 
						== false))
			{
				this.Onaddress2Changing(value);
				this.SendPropertyChanging();
				this._address2 = value;
				this.SendPropertyChanged("address2");
				this.Onaddress2Changed();
			}
		}
	}
	
	[Column(Storage="_alipay", Name="alipay", DbType="varchar(50)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string alipay
	{
		get
		{
			return this._alipay;
		}
		set
		{
			if (((_alipay == value) 
						== false))
			{
				this.OnalipayChanging(value);
				this.SendPropertyChanging();
				this._alipay = value;
				this.SendPropertyChanged("alipay");
				this.OnalipayChanged();
			}
		}
	}
	
	[Column(Storage="_area", Name="area", DbType="varchar(200)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string area
	{
		get
		{
			return this._area;
		}
		set
		{
			if (((_area == value) 
						== false))
			{
				this.OnareaChanging(value);
				this.SendPropertyChanging();
				this._area = value;
				this.SendPropertyChanged("area");
				this.OnareaChanged();
			}
		}
	}
	
	[Column(Storage="_authID", Name="authId", DbType="varchar(30)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string authId
	{
		get
		{
			return this._authID;
		}
		set
		{
			if (((_authID == value) 
						== false))
			{
				this.OnauthIdChanging(value);
				this.SendPropertyChanging();
				this._authID = value;
				this.SendPropertyChanged("authId");
				this.OnauthIdChanged();
			}
		}
	}
	
	[Column(Storage="_authProvider", Name="authProvider", DbType="varchar(15)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string authProvider
	{
		get
		{
			return this._authProvider;
		}
		set
		{
			if (((_authProvider == value) 
						== false))
			{
				this.OnauthProviderChanging(value);
				this.SendPropertyChanging();
				this._authProvider = value;
				this.SendPropertyChanged("authProvider");
				this.OnauthProviderChanged();
			}
		}
	}
	
	[Column(Storage="_avatarUrl", Name="avatarUrl", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string avatarUrl
	{
		get
		{
			return this._avatarUrl;
		}
		set
		{
			if (((_avatarUrl == value) 
						== false))
			{
				this.OnavatarUrlChanging(value);
				this.SendPropertyChanging();
				this._avatarUrl = value;
				this.SendPropertyChanged("avatarUrl");
				this.OnavatarUrlChanged();
			}
		}
	}
	
	[Column(Storage="_gender", Name="gender", DbType="varchar(10)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string gender
	{
		get
		{
			return this._gender;
		}
		set
		{
			if (((_gender == value) 
						== false))
			{
				this.OngenderChanging(value);
				this.SendPropertyChanging();
				this._gender = value;
				this.SendPropertyChanged("gender");
				this.OngenderChanged();
			}
		}
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_message", Name="message", DbType="varchar(500)", AutoSync=AutoSync.Never)]
	[DebuggerNonUserCode()]
	public string message
	{
		get
		{
			return this._message;
		}
		set
		{
			if (((_message == value) 
						== false))
			{
				this.OnmessageChanging(value);
				this.SendPropertyChanging();
				this._message = value;
				this.SendPropertyChanged("message");
				this.OnmessageChanged();
			}
		}
	}
	
	[Column(Storage="_name", Name="name", DbType="varchar(30)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string name
	{
		get
		{
			return this._name;
		}
		set
		{
			if (((_name == value) 
						== false))
			{
				this.OnnameChanging(value);
				this.SendPropertyChanging();
				this._name = value;
				this.SendPropertyChanged("name");
				this.OnnameChanged();
			}
		}
	}
	
	[Column(Storage="_username", Name="username", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string username
	{
		get
		{
			return this._username;
		}
		set
		{
			if (((_username == value) 
						== false))
			{
				this.OnusernameChanging(value);
				this.SendPropertyChanging();
				this._username = value;
				this.SendPropertyChanged("username");
				this.OnusernameChanged();
			}
		}
	}
	
	#region Children
	[Association(Storage="_admins", OtherKey="userid", ThisKey="id", Name="admin_ibfk_1")]
	[DebuggerNonUserCode()]
	public EntitySet<admin> admins
	{
		get
		{
			return this._admins;
		}
		set
		{
			this._admins = value;
		}
	}
	
	[Association(Storage="_dev", OtherKey="userid", ThisKey="id", Name="dev_ibfk_1")]
	[DebuggerNonUserCode()]
	public EntitySet<dev> dev
	{
		get
		{
			return this._dev;
		}
		set
		{
			this._dev = value;
		}
	}
	
	[Association(Storage="_senders", OtherKey="userid", ThisKey="id", Name="sender_ibfk_1")]
	[DebuggerNonUserCode()]
	public EntitySet<sender> senders
	{
		get
		{
			return this._senders;
		}
		set
		{
			this._senders = value;
		}
	}
	#endregion
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
	
	#region Attachment handlers
	private void admins_Attach(admin entity)
	{
		this.SendPropertyChanging();
		entity.user = this;
	}
	
	private void admins_Detach(admin entity)
	{
		this.SendPropertyChanging();
		entity.user = null;
	}
	
	private void dev_Attach(dev entity)
	{
		this.SendPropertyChanging();
		entity.user = this;
	}
	
	private void dev_Detach(dev entity)
	{
		this.SendPropertyChanging();
		entity.user = null;
	}
	
	private void senders_Attach(sender entity)
	{
		this.SendPropertyChanging();
		entity.user = this;
	}
	
	private void senders_Detach(sender entity)
	{
		this.SendPropertyChanging();
		entity.user = null;
	}
	#endregion
}

[Table(Name="gebimai_com.wb_dialog")]
public partial class wbdialog : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private string _data;
	
	private string _wbuid;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OndataChanged();
		
		partial void OndataChanging(string value);
		
		partial void OnwbuidChanged();
		
		partial void OnwbuidChanging(string value);
		#endregion
	
	
	public wbdialog()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_data", Name="data", DbType="varchar(100)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string data
	{
		get
		{
			return this._data;
		}
		set
		{
			if (((_data == value) 
						== false))
			{
				this.OndataChanging(value);
				this.SendPropertyChanging();
				this._data = value;
				this.SendPropertyChanged("data");
				this.OndataChanged();
			}
		}
	}
	
	[Column(Storage="_wbuid", Name="wbuid", DbType="varchar(50)", IsPrimaryKey=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public string wbuid
	{
		get
		{
			return this._wbuid;
		}
		set
		{
			if (((_wbuid == value) 
						== false))
			{
				this.OnwbuidChanging(value);
				this.SendPropertyChanging();
				this._wbuid = value;
				this.SendPropertyChanged("wbuid");
				this.OnwbuidChanged();
			}
		}
	}
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}

[Table(Name="gebimai_com.wb_state")]
public partial class wbstate : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
{
	
	private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
	
	private int _id;
	
	private long _lastcommentID;
	
	private long _laststatusID;
	
	private int _operatoruserID;
	
	#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnlastcommentIDChanged();
		
		partial void OnlastcommentIDChanging(long value);
		
		partial void OnlaststatusIDChanged();
		
		partial void OnlaststatusIDChanging(long value);
		
		partial void OnoperatoruserIDChanged();
		
		partial void OnoperatoruserIDChanging(int value);
		#endregion
	
	
	public wbstate()
	{
		this.OnCreated();
	}
	
	[Column(Storage="_id", Name="id", DbType="int", IsPrimaryKey=true, IsDbGenerated=true, AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int id
	{
		get
		{
			return this._id;
		}
		set
		{
			if ((_id != value))
			{
				this.OnidChanging(value);
				this.SendPropertyChanging();
				this._id = value;
				this.SendPropertyChanged("id");
				this.OnidChanged();
			}
		}
	}
	
	[Column(Storage="_lastcommentID", Name="last_commentID", DbType="bigint(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public long lastcommentID
	{
		get
		{
			return this._lastcommentID;
		}
		set
		{
			if ((_lastcommentID != value))
			{
				this.OnlastcommentIDChanging(value);
				this.SendPropertyChanging();
				this._lastcommentID = value;
				this.SendPropertyChanged("lastcommentID");
				this.OnlastcommentIDChanged();
			}
		}
	}
	
	[Column(Storage="_laststatusID", Name="last_statusID", DbType="bigint(20)", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public long laststatusID
	{
		get
		{
			return this._laststatusID;
		}
		set
		{
			if ((_laststatusID != value))
			{
				this.OnlaststatusIDChanging(value);
				this.SendPropertyChanging();
				this._laststatusID = value;
				this.SendPropertyChanged("laststatusID");
				this.OnlaststatusIDChanged();
			}
		}
	}
	
	[Column(Storage="_operatoruserID", Name="operator_userID", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
	[DebuggerNonUserCode()]
	public int operatoruserID
	{
		get
		{
			return this._operatoruserID;
		}
		set
		{
			if ((_operatoruserID != value))
			{
				this.OnoperatoruserIDChanging(value);
				this.SendPropertyChanging();
				this._operatoruserID = value;
				this.SendPropertyChanged("operatoruserID");
				this.OnoperatoruserIDChanged();
			}
		}
	}
	
	public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
	
	public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
	
	protected virtual void SendPropertyChanging()
	{
		System.ComponentModel.PropertyChangingEventHandler h = this.PropertyChanging;
		if ((h != null))
		{
			h(this, emptyChangingEventArgs);
		}
	}
	
	protected virtual void SendPropertyChanged(string propertyName)
	{
		System.ComponentModel.PropertyChangedEventHandler h = this.PropertyChanged;
		if ((h != null))
		{
			h(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
		}
	}
}
