using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Emulator.Avr;
using WeifenLuo.WinFormsUI.Docking;

namespace Emulator
{
	public partial class ObjectViewer : DockContent
	{
		private MemoryObject[] _items;
		private static Dictionary<string, TypeItem> _objects = new Dictionary<string, TypeItem>();

		public static void AddType(TypeItem type)
		{
			_objects.Add(type.TypeName, type);
		}
		public ObjectViewer(string name, ObjectItem[] objects)
		{
			InitializeComponent();
			this.Text = name;
			olv_Objects.CanExpandGetter += CanExpandGetter;
			olv_Objects.ChildrenGetter += ChildrenGetter;
			_items = objects.Select(o => ParseObject(o.Address,o.Name,_objects[o.Type])).ToArray();
			olv_Objects.Roots = _items;
			
		}


		private MemoryObject ParseObject(int address, string name, TypeItem o)
		{
			var mo = new MemoryObject(olv_Objects)
			{
				Address = address,
				Name = name,
				Type = o.TypeName,
				Size = 0
			};
			mo.Inner = new List<MemoryObject>();

			foreach (var i in o.Properties)
			{
				MemoryObject prop;
				if (i.Type.ToLower()=="byte")
					prop= new ByteObject(olv_Objects,i.Name,address+mo.Size);
				else if (i.Type.ToLower()=="int")
					prop = new IntObject(olv_Objects, i.Name, address + mo.Size);
				else prop = ParseObject(address + mo.Size, i.Name, _objects[i.Type]);

				mo.Inner.Add(prop);
				mo.Size += prop.Size;
			}

			return mo;
		}
		public void RefreshAddress(Dictionary<int, byte> addressValueMap)
		{
			foreach(var i in _items)
				i.Update(addressValueMap);
		}

		private bool CanExpandGetter(object model)
		{
			return ((MemoryObject)model).Inner != null;
		}

		private IEnumerable ChildrenGetter(object model)
		{
			return ((MemoryObject)model).Inner;
		}
	}
	public class ObjectItem
	{
		public ObjectItem(int address, string name, string type)
		{
			Type = type;
			Address = address;
			Name = name;
		}
		public string Type { get; set; }
		public string Name { get; set; }
		public int Address { get; set; }
	}
	public class TypeItem
	{
		public string TypeName { get; set; }
		public PropertyItem[] Properties { get; set; }
	}

	public class PropertyItem
	{
		public string Name { get; set; }
		public string Type { get; set; }
	}

	public class RegistersObject : MemoryObject
	{
		public RegistersObject(TreeListView tlv)
			:base(tlv)
		{
			Name = "Registers";
			Inner = new List<MemoryObject>();
		}
	}

	public class ByteObject : MemoryObject
	{
		public ByteObject(TreeListView tlv, string name, int address)
			:base (tlv)
		{
			Name = name;
			Address = address;
			Value = "0x00";
			Type = "Byte";
			Size = 1;
		}
		protected override void Refresh(Dictionary<int, byte> addressValueMap)
		{
			if (addressValueMap.ContainsKey(Address))
			{
				Value = "0x"+addressValueMap[Address].ToString("x2");
				Tlv.RefreshObject(this);
			}
		}
	}
	public class IntObject : MemoryObject
	{
		private int _value = 0;
		public IntObject(TreeListView tlv, string name, int address)
			: base(tlv)
		{
			Name = name;
			Address = address;
			Value = "0x0000";
			Type = "Int";
			Size = 2;
			Inner = new List<MemoryObject>()
			{
				new ByteObject(tlv,"High",address+1),
				new ByteObject(tlv,"Low",address)
			};
		}

		protected override void Refresh(Dictionary<int, byte> addressValueMap)
		{
			byte temp1 = 0;
			int temp = _value;
			if (addressValueMap.TryGetValue(Address, out temp1))
				temp = (temp & 0xff00) | temp1;
			if (addressValueMap.TryGetValue(Address + 1, out temp1))
				temp = (temp & 0xff) | (temp1 << 8);
			if (temp == _value) return;
			_value = temp;
			
			Value = "0x" + _value.ToString("x4");
			Tlv.RefreshObject(this);
		}
	}
	public class MemoryObject
	{
		public TreeListView Tlv { get; set; }

		public MemoryObject(TreeListView tlv)
		{
			Tlv = tlv;
		}

		public string Value { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public int Size { get; set; }
		public List<MemoryObject> Inner { get; set; }

		public int Address { get; set; }

		public void Update(Dictionary<int, byte> addressValueMap)
		{
			Refresh(addressValueMap);
			if (Inner != null)
			{
				foreach (var o in Inner)
					o.Update(addressValueMap);
			}
		}

		protected virtual void Refresh(Dictionary<int, byte> addressValueMap)
		{
		}
	}
}
