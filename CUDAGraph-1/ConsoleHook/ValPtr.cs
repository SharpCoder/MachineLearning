using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleHook
{
	// NOTE: This class looks silly, however,
	// objects in C# are passed by ref while primitives are passed
	// byval. Yadda yadda huge flame war over the terminology
	// but you get the gist.
	public class ValPtr
	{
		public float value;
		public Action<ValPtr> onChange = null;

		// Constructors.
		public ValPtr() { }
		public ValPtr(float v) { this.value = v; }
		public void Set(float v)
		{
			float ov = this.value;
			if (onChange != null && ov != v)
				onChange.Invoke(this);

			this.value = v;
		}
	}
}
