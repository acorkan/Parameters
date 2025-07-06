using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParameterModel.Interfaces
{
    public interface IVariablesContext
    {
        //public Dictionary<string, string> Variables { get; } = new Dictionary<string, string>();
        Dictionary<string, string> Variables { get; }
    }
}
