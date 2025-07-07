using ParameterModel.Interfaces;
using Python.Runtime;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace PythonWrapper
{
    public class PyEval : IDisposable
    {
        private static int _TrackingIndexCounter;
        private static bool _isInitialized = false;

        [JsonIgnore]
        private int _trackingIndex = Interlocked.Increment(ref _TrackingIndexCounter);

        private bool disposedValue;

        public PyEval()
        {
            // Initialize the Python engine when the class is instantiated
            if(!_isInitialized)
            {
                InitializePython();
                _isInitialized = true;
            }
            Trace.WriteLine($"PyEval instance created with tracking index: {_trackingIndex}");
        }

        //~PyEval()
        //{
        //    _trackingIndex--; // Decrement the tracking index when the instance is finalized
        //    Trace.WriteLine($"PyEval instance destroyed with tracking index: {_trackingIndex}");
        //    int trackingIndex = Interlocked.Decrement(ref _TrackingIndexCounter);
        //    if(trackingIndex < 0)
        //    {
        //        Trace.WriteLine("Tracking index is negative, which is unexpected.");
        //    }
        //    else
        //    {
        //        Trace.WriteLine($"Tracking index decremented to: {trackingIndex}");
        //    }
        //}

        public bool Eval(string code, IVariablesContext variableContext, out string result, out string error)
        {
            result = string.Empty;
            error = string.Empty;
            using (Py.GIL())  // Ensure thread safety
            {
                PyDict globals = new PyDict();
                //PyObject locals = null; // new PyObject(PyObject.Null);
                // Convert C# context dictionary to Python dict
                using var locals = new PyDict();
                foreach (var varItem in variableContext.Variables)
                {
                    if(varItem.Type == VariableType.Boolean)
                    { 
                        locals.SetItem(varItem.Name.ToPython(), varItem.GetValueAsBool().ToPython());
                    }
                    else if (varItem.Type == VariableType.Integer)
                    {
                        locals.SetItem(varItem.Name.ToPython(), varItem.GetValueAsInt().ToPython());
                    }
                    else if (varItem.Type == VariableType.Float)
                    {
                        locals.SetItem(varItem.Name.ToPython(), varItem.GetValueAsFloat().ToPython());
                    }
                    else if ((varItem.Type == VariableType.String) ||
                        (varItem.Type == VariableType.JSON))
                    {
                        locals.SetItem(varItem.Name.ToPython(), varItem.GetValueAsString().ToPython());
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unsupported variable type: {varItem.Type} for variable '{varItem.Name}'");
                    }
                }
                try
                {
                    //string code = "3 * 4 + 5";
                    dynamic dResult = PythonEngine.Eval(code, globals, locals);

                    //Trace.WriteLine($"Result: {result}");
                    result = dResult.ToString();
                    return true;
                }
                catch (PythonException ex)
                {
                    error = ex.Message;
                    Trace.WriteLine($"Python error: {error}");
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    Trace.WriteLine($"General error: {error}");
                }
            }
            return false;
        }

        public bool EvalInt(string code, IVariablesContext variableContext, out int result, out string error)
        {

        }

                    public bool EvalFloat(string code, IVariablesContext variableContext, out float result, out string error)
        {

        }

                                public bool EvalBool(string code, IVariablesContext variableContext, out bool result, out string error)
        {

        }

                    public bool EvalJson(string code, IVariablesContext variableContext, out string result, out string error)
        {

        }

        /*
         using (Py.GIL())
{
    dynamic locals = new PyDict();
    locals["x"] = 10;
    locals["y"] = 5;
    string code = "x * y + 2";
    dynamic result = PythonEngine.Exec(code, locals);
    Console.WriteLine($"Result: {result}");
}

        To  start with no variables are "evaluated". Then run Eval() on each variable with no locals aand for any that succeed then store as "evaluated".
         * */
        public void InitializePython()
        {
            //if (string.IsNullOrEmpty(PythonEngine.PythonHome))
            {
                // Set the path to the Python DLL
                Runtime.PythonDLL = @"C:\Users\Public\Python\Python310\python310.dll";//  "C:\Path\To\Python";
                PythonEngine.Initialize();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~PyEval()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
