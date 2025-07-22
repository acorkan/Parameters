using MileHighWpf.MvvmModelMessaging;
using ParameterViews.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersDemo
{
    public partial class MainViewModel : ParamPromptViewModel

    {
        public MainViewModel(List<ParamViewModelBase> parameters, bool isReadOnly) : base(parameters, isReadOnly)
        {
        }
    }
}
