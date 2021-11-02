using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comunication.Shared.Interfaces;

namespace Comunication.Shared
{
    public class Payload<TW> where TW : IMessage
    {
        public TW Value { get; private set; }
        public CRUD Type { get; private set; }
        public DateTime Time { get; private set; }

        public Payload(TW _massage, CRUD _type)
        {
            Value = _massage;
            Type = _type;
            Time = DateTime.Now;
        }
    }

    public enum CRUD
    {
        Create,
        Read,
        Update,
        Delete,
        Exists
    }
}
