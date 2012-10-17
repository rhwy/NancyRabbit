using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.Serialization;
using Microsoft.CSharp;
using Nancy.Hosting.Self;

namespace rabbit
{
    [Serializable]
    public class NancyAutoHost
    {
        private readonly Uri _endpoint;
        private readonly string _sourceWatchFolder;
        private readonly string _libraryLocation;
        private NancyHost _nancyHost;
        private FileSystemWatcher _watcher;

        public NancyAutoHost(string endpoint, string sourceWatchFolder, string libraryLocation)
        {
            _endpoint = new Uri(endpoint);
            _sourceWatchFolder = sourceWatchFolder;
            _libraryLocation = libraryLocation;
        }

        public void AutoRun()
        {
            Start();
        }

        public void Start()
        {
            if (!Directory.Exists(_sourceWatchFolder))
            {
                var inner = new DirectoryNotFoundException(_sourceWatchFolder);
                throw new NancyAutoHostException("You need to provide a valid folder to watch", inner);
            }

            _watcher = CreateFolderWatcher(_sourceWatchFolder);

            _watcher.EnableRaisingEvents = true;
            OnChange(null);

        }

        private AppDomain _ad;
        private void OnChange(FileSystemEventArgs args)
        {
            if (args != null)
            {
                Console.WriteLine("File Change Found:");
                Console.WriteLine("   [{0}] {1}", args.ChangeType, args.Name);
            }

            if (_ad != null)
            {
                AppDomain.Unload(_ad);
                _ad = null;
            }
            _ad = AppDomain.CreateDomain("temp");
            _ad.DoCallBack(CrossDomainRunner);

        }

        public void CrossDomainRunner()
        {
            SourceBuild();
            _nancyHost = RunHost(_nancyHost, _endpoint);
        }

        private void SourceBuild()
        {
            var sourceFiles = Directory.GetFiles(_sourceWatchFolder);
            Console.WriteLine("{0} source files found", sourceFiles.Length);
            Console.WriteLine("Start compiling");
            Compile(sourceFiles);
        }

        public NancyHost RunHost(NancyHost host, Uri endpoint)
        {

            if (host != null)
            {
                Console.WriteLine("Stoping current Nancy host");
                host.Stop();
                host = null;
            }
            Console.WriteLine("Creating new Nancy host");
            host = new NancyHost(endpoint);
            host.Start();
            Console.WriteLine("Running Nancy host on {0}", endpoint.AbsoluteUri);
            Console.WriteLine("Press enter to stop server");
            return host;
        }

        private FileSystemWatcher CreateFolderWatcher(string sourceWatchFolder)
        {
            var watcher = new FileSystemWatcher(sourceWatchFolder, "*.cs") { IncludeSubdirectories = true, EnableRaisingEvents = true };
            watcher.Changed += (sender, args) => OnChange(args);
            watcher.Created += (sender, args) => OnChange(args);
            watcher.Deleted += (sender, args) => OnChange(args);
            return watcher;
        }



        [Serializable]
        public class NancyAutoHostException : Exception
        {
            public NancyAutoHostException()
            {
            }

            public NancyAutoHostException(string message)
                : base(message)
            {
            }

            public NancyAutoHostException(string message, Exception inner)
                : base(message, inner)
            {
            }

            protected NancyAutoHostException(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }


        public void Compile(string[] sources)
        {


            var compilerParams = new CompilerParameters
                                     {
                                         CompilerOptions = "/target:library /optimize",
                                         GenerateExecutable = false,
                                         GenerateInMemory = true,
                                         IncludeDebugInformation = true
                                     };
            compilerParams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParams.ReferencedAssemblies.Add("System.dll");
            compilerParams.ReferencedAssemblies.Add("System.Core.dll");

            compilerParams.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            compilerParams.ReferencedAssemblies.Add(Path.Combine(_libraryLocation, "Nancy.dll"));

            CodeDomProvider codeProvider = new CSharpCodeProvider();
            CompilerResults results = codeProvider.CompileAssemblyFromFile(compilerParams, sources);
        }
    }
}