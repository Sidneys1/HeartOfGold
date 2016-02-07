using OpenTK;
using System.Runtime.InteropServices;

namespace VirusFactory.OpenTK.GameHelpers.VBOHelper {

	[StructLayout(LayoutKind.Explicit)]
	public struct BufferElement {

		[FieldOffset(0)]
		public Vector3 Vertex;

		[FieldOffset(3 * sizeof(float))]
		public Vector3 Color;

		public BufferElement(Vector3 vertex, Vector3 color) {
			Vertex = vertex;
			Color = color;
		}

		public static readonly int SizeInBytes = Vector3.SizeInBytes + Vector3.SizeInBytes;
	}
}