// 
//  ____  _     __  __      _        _ 
// |  _ \| |__ |  \/  | ___| |_ __ _| |
// | | | | '_ \| |\/| |/ _ \ __/ _` | |
// | |_| | |_) | |  | |  __/ || (_| | |
// |____/|_.__/|_|  |_|\___|\__\__,_|_|
//
// Auto-generated from chifanshe_com on 2012-03-30 13:16:30Z.
// Please visit http://code.google.com/p/dblinq2007/ for more information.
//
namespace cfs
{
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
	
	
	public partial class chifanshecom : DataContext
	{
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		#endregion
		
		
		public chifanshecom(string connectionString) : 
				base(connectionString)
		{
			this.OnCreated();
		}
		
		public chifanshecom(string connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public chifanshecom(IDbConnection connection, MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			this.OnCreated();
		}
		
		public Table<food> foods
		{
			get
			{
				return this.GetTable<food>();
			}
		}
		
		public Table<order> orders
		{
			get
			{
				return this.GetTable<order>();
			}
		}
		
		public Table<store> stores
		{
			get
			{
				return this.GetTable<store>();
			}
		}
		
		public Table<timespan> timespans
		{
			get
			{
				return this.GetTable<timespan>();
			}
		}
	}
	
	#region Start MONO_STRICT
#if MONO_STRICT

	public partial class chifanshecom
	{
		
		public chifanshecom(IDbConnection connection) : 
				base(connection)
		{
			this.OnCreated();
		}
	}
	#region End MONO_STRICT
	#endregion
#else     // MONO_STRICT
	
	public partial class chifanshecom
	{
		
		public chifanshecom(IDbConnection connection) : 
				base(connection, new DbLinq.MySql.MySqlVendor())
		{
			this.OnCreated();
		}
		
		public chifanshecom(IDbConnection connection, IVendor sqlDialect) : 
				base(connection, sqlDialect)
		{
			this.OnCreated();
		}
		
		public chifanshecom(IDbConnection connection, MappingSource mappingSource, IVendor sqlDialect) : 
				base(connection, mappingSource, sqlDialect)
		{
			this.OnCreated();
		}
	}
	#region End Not MONO_STRICT
	#endregion
#endif     // MONO_STRICT
	#endregion
	
	[Table(Name="chifanshe_com.food")]
	public partial class food : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _description;
		
		private int _id;
		
		private string _optiondata;
		
		private int _price;
		
		private int _storeID;
		
		private string _title;
		
		private EntityRef<store> _store = new EntityRef<store>();
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OndescriptionChanged();
		
		partial void OndescriptionChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnoptiondataChanged();
		
		partial void OnoptiondataChanging(string value);
		
		partial void OnpriceChanged();
		
		partial void OnpriceChanging(int value);
		
		partial void OnstoreIdChanged();
		
		partial void OnstoreIdChanging(int value);
		
		partial void OntitleChanged();
		
		partial void OntitleChanging(string value);
		#endregion
		
		
		public food()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_description", Name="description", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (((_description == value) 
							== false))
				{
					this.OndescriptionChanging(value);
					this.SendPropertyChanging();
					this._description = value;
					this.SendPropertyChanged("description");
					this.OndescriptionChanged();
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
		
		[Column(Storage="_optiondata", Name="optiondata", DbType="varchar(500)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string optiondata
		{
			get
			{
				return this._optiondata;
			}
			set
			{
				if (((_optiondata == value) 
							== false))
				{
					this.OnoptiondataChanging(value);
					this.SendPropertyChanging();
					this._optiondata = value;
					this.SendPropertyChanged("optiondata");
					this.OnoptiondataChanged();
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
		
		[Column(Storage="_storeID", Name="storeId", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int storeId
		{
			get
			{
				return this._storeID;
			}
			set
			{
				if ((_storeID != value))
				{
					if (_store.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnstoreIdChanging(value);
					this.SendPropertyChanging();
					this._storeID = value;
					this.SendPropertyChanged("storeId");
					this.OnstoreIdChanged();
				}
			}
		}
		
		[Column(Storage="_title", Name="title", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
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
		
		#region Parents
		[Association(Storage="_store", OtherKey="id", ThisKey="storeId", Name="food_ibfk_1", IsForeignKey=true)]
		[DebuggerNonUserCode()]
		public store store
		{
			get
			{
				return this._store.Entity;
			}
			set
			{
				if (((this._store.Entity == value) 
							== false))
				{
					if ((this._store.Entity != null))
					{
						store previousstore = this._store.Entity;
						this._store.Entity = null;
						previousstore.foods.Remove(this);
					}
					this._store.Entity = value;
					if ((value != null))
					{
						value.foods.Add(this);
						_storeID = value.id;
					}
					else
					{
						_storeID = default(int);
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
	
	[Table(Name="chifanshe_com.order")]
	public partial class order : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _address;
		
		private string _content;
		
		private int _id;
		
		private string _ip;
		
		private int _num;
		
		private string _phone;
		
		private int _storeID;
		
		private System.DateTime _time;
		
		private EntityRef<store> _store = new EntityRef<store>();
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnaddressChanged();
		
		partial void OnaddressChanging(string value);
		
		partial void OncontentChanged();
		
		partial void OncontentChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnipChanged();
		
		partial void OnipChanging(string value);
		
		partial void OnnumChanged();
		
		partial void OnnumChanging(int value);
		
		partial void OnphoneChanged();
		
		partial void OnphoneChanging(string value);
		
		partial void OnstoreIdChanged();
		
		partial void OnstoreIdChanging(int value);
		
		partial void OntimeChanged();
		
		partial void OntimeChanging(System.DateTime value);
		#endregion
		
		
		public order()
		{
			this.OnCreated();
		}
		
		[Column(Storage="_address", Name="address", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
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
		
		[Column(Storage="_ip", Name="ip", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string ip
		{
			get
			{
				return this._ip;
			}
			set
			{
				if (((_ip == value) 
							== false))
				{
					this.OnipChanging(value);
					this.SendPropertyChanging();
					this._ip = value;
					this.SendPropertyChanged("ip");
					this.OnipChanged();
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
		
		[Column(Storage="_phone", Name="phone", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				if (((_phone == value) 
							== false))
				{
					this.OnphoneChanging(value);
					this.SendPropertyChanging();
					this._phone = value;
					this.SendPropertyChanged("phone");
					this.OnphoneChanged();
				}
			}
		}
		
		[Column(Storage="_storeID", Name="storeId", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int storeId
		{
			get
			{
				return this._storeID;
			}
			set
			{
				if ((_storeID != value))
				{
					if (_store.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnstoreIdChanging(value);
					this.SendPropertyChanging();
					this._storeID = value;
					this.SendPropertyChanged("storeId");
					this.OnstoreIdChanged();
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
		
		#region Parents
		[Association(Storage="_store", OtherKey="id", ThisKey="storeId", Name="order_ibfk_1", IsForeignKey=true)]
		[DebuggerNonUserCode()]
		public store store
		{
			get
			{
				return this._store.Entity;
			}
			set
			{
				if (((this._store.Entity == value) 
							== false))
				{
					if ((this._store.Entity != null))
					{
						store previousstore = this._store.Entity;
						this._store.Entity = null;
						previousstore.orders.Remove(this);
					}
					this._store.Entity = value;
					if ((value != null))
					{
						value.orders.Add(this);
						_storeID = value.id;
					}
					else
					{
						_storeID = default(int);
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
	
	[Table(Name="chifanshe_com.store")]
	public partial class store : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private string _alias;
		
		private int _id;
		
		private string _phone;
		
		private int _pricelimit;
		
		private string _speed;
		
		private string _title;
		
		private EntitySet<food> _foods;
		
		private EntitySet<order> _orders;
		
		private EntitySet<timespan> _timespans;
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnaliasChanged();
		
		partial void OnaliasChanging(string value);
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnphoneChanged();
		
		partial void OnphoneChanging(string value);
		
		partial void OnpricelimitChanged();
		
		partial void OnpricelimitChanging(int value);
		
		partial void OnspeedChanged();
		
		partial void OnspeedChanging(string value);
		
		partial void OntitleChanged();
		
		partial void OntitleChanging(string value);
		#endregion
		
		
		public store()
		{
			_foods = new EntitySet<food>(new Action<food>(this.foods_Attach), new Action<food>(this.foods_Detach));
			_orders = new EntitySet<order>(new Action<order>(this.orders_Attach), new Action<order>(this.orders_Detach));
			_timespans = new EntitySet<timespan>(new Action<timespan>(this.timespans_Attach), new Action<timespan>(this.timespans_Detach));
			this.OnCreated();
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
		
		[Column(Storage="_phone", Name="phone", DbType="varchar(200)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string phone
		{
			get
			{
				return this._phone;
			}
			set
			{
				if (((_phone == value) 
							== false))
				{
					this.OnphoneChanging(value);
					this.SendPropertyChanging();
					this._phone = value;
					this.SendPropertyChanged("phone");
					this.OnphoneChanged();
				}
			}
		}
		
		[Column(Storage="_pricelimit", Name="pricelimit", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int pricelimit
		{
			get
			{
				return this._pricelimit;
			}
			set
			{
				if ((_pricelimit != value))
				{
					this.OnpricelimitChanging(value);
					this.SendPropertyChanging();
					this._pricelimit = value;
					this.SendPropertyChanged("pricelimit");
					this.OnpricelimitChanged();
				}
			}
		}
		
		[Column(Storage="_speed", Name="speed", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public string speed
		{
			get
			{
				return this._speed;
			}
			set
			{
				if (((_speed == value) 
							== false))
				{
					this.OnspeedChanging(value);
					this.SendPropertyChanging();
					this._speed = value;
					this.SendPropertyChanged("speed");
					this.OnspeedChanged();
				}
			}
		}
		
		[Column(Storage="_title", Name="title", DbType="varchar(50)", AutoSync=AutoSync.Never, CanBeNull=false)]
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
		[Association(Storage="_foods", OtherKey="storeId", ThisKey="id", Name="food_ibfk_1")]
		[DebuggerNonUserCode()]
		public EntitySet<food> foods
		{
			get
			{
				return this._foods;
			}
			set
			{
				this._foods = value;
			}
		}
		
		[Association(Storage="_orders", OtherKey="storeId", ThisKey="id", Name="order_ibfk_1")]
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
		
		[Association(Storage="_timespans", OtherKey="storeId", ThisKey="id", Name="timespan_ibfk_1")]
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
		private void foods_Attach(food entity)
		{
			this.SendPropertyChanging();
			entity.store = this;
		}
		
		private void foods_Detach(food entity)
		{
			this.SendPropertyChanging();
			entity.store = null;
		}
		
		private void orders_Attach(order entity)
		{
			this.SendPropertyChanging();
			entity.store = this;
		}
		
		private void orders_Detach(order entity)
		{
			this.SendPropertyChanging();
			entity.store = null;
		}
		
		private void timespans_Attach(timespan entity)
		{
			this.SendPropertyChanging();
			entity.store = this;
		}
		
		private void timespans_Detach(timespan entity)
		{
			this.SendPropertyChanging();
			entity.store = null;
		}
		#endregion
	}
	
	[Table(Name="chifanshe_com.timespan")]
	public partial class timespan : System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		private static System.ComponentModel.PropertyChangingEventArgs emptyChangingEventArgs = new System.ComponentModel.PropertyChangingEventArgs("");
		
		private int _id;
		
		private System.DateTime _start;
		
		private System.DateTime _stop;
		
		private int _storeID;
		
		private EntityRef<store> _store = new EntityRef<store>();
		
		#region Extensibility Method Declarations
		partial void OnCreated();
		
		partial void OnidChanged();
		
		partial void OnidChanging(int value);
		
		partial void OnstartChanged();
		
		partial void OnstartChanging(System.DateTime value);
		
		partial void OnstopChanged();
		
		partial void OnstopChanging(System.DateTime value);
		
		partial void OnstoreIdChanged();
		
		partial void OnstoreIdChanging(int value);
		#endregion
		
		
		public timespan()
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
		
		[Column(Storage="_storeID", Name="storeId", DbType="int", AutoSync=AutoSync.Never, CanBeNull=false)]
		[DebuggerNonUserCode()]
		public int storeId
		{
			get
			{
				return this._storeID;
			}
			set
			{
				if ((_storeID != value))
				{
					if (_store.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnstoreIdChanging(value);
					this.SendPropertyChanging();
					this._storeID = value;
					this.SendPropertyChanged("storeId");
					this.OnstoreIdChanged();
				}
			}
		}
		
		#region Parents
		[Association(Storage="_store", OtherKey="id", ThisKey="storeId", Name="timespan_ibfk_1", IsForeignKey=true)]
		[DebuggerNonUserCode()]
		public store store
		{
			get
			{
				return this._store.Entity;
			}
			set
			{
				if (((this._store.Entity == value) 
							== false))
				{
					if ((this._store.Entity != null))
					{
						store previousstore = this._store.Entity;
						this._store.Entity = null;
						previousstore.timespans.Remove(this);
					}
					this._store.Entity = value;
					if ((value != null))
					{
						value.timespans.Add(this);
						_storeID = value.id;
					}
					else
					{
						_storeID = default(int);
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
}
