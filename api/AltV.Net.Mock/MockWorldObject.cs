using System;
using AltV.Net.Data;
using AltV.Net.Elements.Args;
using AltV.Net.Elements.Entities;

namespace AltV.Net.Mock
{
    public class MockWorldObject : WorldObject
    {
        public MockWorldObject(IntPtr nativePointer, BaseObjectType type) : base(nativePointer, type)
        {
        }

        public override Position Position { get; set; }
        public override short Dimension { get; set; }

        public override void SetMetaData(string key, ref MValue value)
        {
            throw new NotImplementedException();
        }

        public override void GetMetaData(string key, ref MValue value)
        {
            throw new NotImplementedException();
        }
    }
}