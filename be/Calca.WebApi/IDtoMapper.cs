using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi
{
    public interface IDtoMapper
    {
        TTarget Map<TSource, TTarget>(TSource source);
        void Map<TSource, TTarget>(TSource source, TTarget target);
    }
}
