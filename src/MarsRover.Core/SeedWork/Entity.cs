using System;

namespace MarsRover.Core.SeedWork
{
    public abstract class Entity
    {
        Guid _id;
        public virtual Guid Id
        {
            get
            {
                return _id;
            }
            protected set
            {
                _id = value;
            }
        }
    }
}
