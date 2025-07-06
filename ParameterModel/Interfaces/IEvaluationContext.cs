using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Interfaces
{
    public interface IEvaluationContext : IVariablesContext
    {
        ILog Log { get; }
        bool IsDebug { get; }
        bool IsSimulation { get; }
    }
}
