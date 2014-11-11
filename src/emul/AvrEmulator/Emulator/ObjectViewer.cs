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
		private MemoryObject _registers;
		public ObjectViewer()
		{
			InitializeComponent();
			olv_Objects.CanExpandGetter += CanExpandGetter;
			olv_Objects.ChildrenGetter += ChildrenGetter;

			_registers = new RegistersObject(olv_Objects);
			foreach(Register r in Enum.GetValues(typeof(Register)))
			{
				_registers.Inner.Add(new RegisterObject(olv_Objects,r));
			}
			
			
			//olv_Objects.AddObject(mo);
			//var item = olv_Objects.Items[0];
			olv_Objects.Roots = new []{ _registers};
			
			
		}

		public void RefreshAddress(Dictionary<int, byte> addressValueMap)
		{
			_registers.Update(addressValueMap);
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
	public class RegistersObject : MemoryObject
	{
		public RegistersObject(TreeListView tlv)
			:base(tlv)
		{
			Name = "Registers";
			Inner = new List<MemoryObject>();
		}
	}
	public class RegisterObject : MemoryObject
	{
		public RegisterObject(TreeListView tlv, Register register)
			:base (tlv)
		{
			Name = register.ToString();
			Address = (int) register;
			Value = "0x0";
			Dec = "0";
			Bin = "0b00000000";
		}
		protected override void Refresh(Dictionary<int, byte> addressValueMap)
		{
			if (addressValueMap.ContainsKey(Address))
			{
				Value = addressValueMap[Address].ToString("x2");
				Tlv.RefreshObject(this);
			}
		}
	}
	public abstract class MemoryObject
	{
		public TreeListView Tlv { get; set; }

		protected MemoryObject(TreeListView tlv)
		{
			Tlv = tlv;
		}

		public string Value { get; set; }
		public string Name { get; set; }
		public string Dec { get; set; }
		public string Type { get; set; }
		public string Bin { get; set; }
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
