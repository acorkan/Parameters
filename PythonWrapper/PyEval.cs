using ParameterModel.Interfaces;
using ParameterModel.Variables;
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

        public bool Eval(string code, IVariablesContext variablesContext, out string result, out string error)
        {
            result = string.Empty;
            error = string.Empty;
            using (Py.GIL())  // Ensure thread safety
            {
                PyDict globals = new PyDict();
                //PyObject locals = null; // new PyObject(PyObject.Null);
                // Convert C# context dictionary to Python dict
                using var locals = new PyDict();
                foreach (var varItem in variablesContext.Variables)
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
                    dynamic dResult = PythonEngine.Eval(code, globals, locals);
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
            result = 0;
            error = "";
            if (Eval(code, variableContext, out string evalResult, out error))
            {
                if ((evalResult != null) && int.TryParse(evalResult, out result))
                {
                    return true;
                }
                error = $"Unable to convert evaluation result '{evalResult}' to int.";
            }
            return false;
        }

        public bool EvalFloat(string code, IVariablesContext variableContext, out float result, out string error)
        {
            result = 0F;
            error = "";
            if (Eval(code, variableContext, out string evalResult, out error))
            {
                if ((evalResult != null) && float.TryParse(evalResult, out result))
                {
                    return true;
                }
                error = $"Unable to convert evaluation result '{evalResult}' to float.";
            }
            return false;
        }

        public bool EvalBool(string code, IVariablesContext variableContext, out bool result, out string error)
        {
            result = false;
            error = "";
            if (Eval(code, variableContext, out string evalResult, out error))
            {
                if ((evalResult != null) && bool.TryParse(evalResult, out result))
                {
                    return true;
                }
                error = $"Unable to convert evaluation result '{evalResult}' to float.";
            }
            return false;
        }

        public bool EvalJson(string code, IVariablesContext variableContext, out string result, out string error)
        {
            result = "";
            error = "";
            if (Eval(code, variableContext, out string evalResult, out error))
            {
                if ((evalResult != null) && VariableBase.IsJson(evalResult))
                {
                    result = evalResult;
                    return true;
                }
                error = $"Evaluation result '{evalResult}' is not JSON.";
            }
            return false;
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

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
