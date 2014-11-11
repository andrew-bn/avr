﻿using System;
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
		private static Dictionary<string, TypeItem> _objects;
		public ObjectViewer(string name, ObjectItemAddress[] objects)
		{
			InitializeComponent();
			this.Text = name;
			olv_Objects.CanExpandGetter += CanExpandGetter;
			olv_Objects.ChildrenGetter += ChildrenGetter;
			_objects = objects.ToDictionary(o => o.Type.TypeName, o => o.Type);
			_items = objects.Select(o => ParseObject(o.Address,o.Name,o.Type)).ToArray();
			olv_Objects.Roots = _items;
			
		}


		private MemoryObject ParseObject(int address, string name, TypeItem o)
		{
			var mo = new MemoryObject(olv_Objects)
			{
				Address = address,
				Name = name,
				Type = o.TypeName,
			};
			mo.Inner = new List<MemoryObject>();

			foreach (var i in o.Properties)
			{
				MemoryObject prop;
				if (i.Type.ToLower()=="byte")
					prop= new ByteObject(olv_Objects,i.Name,address+i.Offcet);
				else if (i.Type.ToLower()=="int")
					prop = new IntObject(olv_Objects, i.Name, address + i.Offcet);
				else prop = ParseObject(address + i.Offcet, i.Name, _objects[i.Type]);

				mo.Inner.Add(prop);
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
	public class ObjectItemAddress
	{
		public ObjectItemAddress(int address, string name, TypeItem type)
		{
			Type = type;
			Address = address;
			Name = name;
		}
		public TypeItem Type { get; set; }
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
		public int Offcet { get; set; }
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
			Dec = "0";
			Hex = Value;
			Type = "Byte";
		}
		protected override void Refresh(Dictionary<int, byte> addressValueMap)
		{
			if (addressValueMap.ContainsKey(Address))
			{
				Value = "0x"+addressValueMap[Address].ToString("x2");
				Hex = Value;
				Dec = addressValueMap[Address].ToString("G");
				Tlv.RefreshObject(this);
			}
		}
	}
	public class IntObject : MemoryObject
	{
		public IntObject(TreeListView tlv, string name, int address)
			: base(tlv)
		{
			Name = name;
			Address = address;
			Value = "0x0000";
			Dec = "0";
			Hex = Value;
			Type = "Int";
			Inner = new List<MemoryObject>()
			{
				new ByteObject(tlv,"High",address+1),
				new ByteObject(tlv,"Low",address)
			};
		}

		protected override void Refresh(Dictionary<int, byte> addressValueMap)
		{
			if (addressValueMap.ContainsKey(Address))
			{
				var val = addressValueMap[Address] | (addressValueMap[Address] << 8);
				Value = "0x" + val.ToString("x4");
				Hex = Value;
				Dec = val.ToString("G");
				Tlv.RefreshObject(this);
			}
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
		public string Dec { get; set; }
		public string Type { get; set; }
		public string Hex { get; set; }
		public List<MemoryObject> Inner { get; set; }

		public int Address { get; set; }

		public void Update(Dictionary<int, byte> addressValueMap)
		{
			Refresh(addressValueMap);
			if (Inner != null)
			{
				foreach (var o in Inner)
					o.Refresh(addressValueMap);
			}
		}

		protected virtual void Refresh(Dictionary<int, byte> addressValueMap)
		{
		}
	}
}
