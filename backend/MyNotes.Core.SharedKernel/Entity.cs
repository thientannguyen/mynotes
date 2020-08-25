using System.Diagnostics.CodeAnalysis;

namespace MyNotes.Core.SharedKernel
{
    using System;

    [ExcludeFromCodeCoverage]
    public abstract class Entity
    {
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        // ReSharper disable once InconsistentNaming
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
        public virtual string id { get; private set; }

        protected Entity()
            : this(Guid.NewGuid().ToString())
        {
        }

        protected Entity(string id)
        {
            this.id = id;
        }
    }
}
