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

        ~PyEval()
        {
            _trackingIndex--; // Decrement the tracking index when the instance is finalized
            Trace.WriteLine($"PyEval instance destroyed with tracking index: {_trackingIndex}");
            int trackingIndex = Interlocked.Decrement(ref _TrackingIndexCounter);
            if(trackingIndex < 0)
            {
                Trace.WriteLine("Tracking index is negative, which is unexpected.");
            }
            else
            {
                Trace.WriteLine($"Tracking index decremented to: {trackingIndex}");
            }
        }
        public void Eval(string code, IVariablesContext variableContext)
        {
            using (Py.GIL())  // Ensure thread safety
            {
                PyDict globals = new PyDict();
                PyObject locals = null; // new PyObject(PyObject.Null);
                try
                {
                    //string code = "3 * 4 + 5";
                    dynamic result = PythonEngine.Eval(code, globals, locals);

                    Trace.WriteLine($"Result: {result}");
                }
                catch (PythonException ex)
                {
                    Trace.WriteLine($"Python error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"General error: {ex.Message}");
                }
            }
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
