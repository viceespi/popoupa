using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Popoupa.Core.DataBaseManipulation
{
    public interface IRepository<TEntity>
    {
        TEntity Get(Guid id);
        IEnumerable<TEntity> GetAll();
        Guid Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(Guid id);
    }
}
