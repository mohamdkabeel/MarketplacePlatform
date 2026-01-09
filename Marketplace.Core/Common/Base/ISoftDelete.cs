using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Core.Common.Base
{
    public interface ISoftDelete
    {
        public interface ISoftDeletable
        {
            bool IsDeleted { get; }
            DateTime? DeletedAt { get; }

            void SoftDelete();
        }

    }
}
