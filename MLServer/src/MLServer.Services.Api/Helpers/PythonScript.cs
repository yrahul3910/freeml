using System;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace MLServer.Services.Api.Helpers
{
    public class PythonScript
    {
        private ScriptEngine _engine;

        public PythonScript()
        {
            _engine = Python.CreateEngine();
        }

        public TResult RunFromString<TResult>(string code, string variableName)
        {
            // for easier debugging write it out to a file and call: _engine.CreateScriptSourceFromFile(filePath);
            ScriptSource source = _engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            CompiledCode cc = source.Compile();

            ScriptScope scope = _engine.CreateScope();
            cc.Execute(scope);

            return scope.GetVariable<TResult>(variableName);
        }
    }
}

