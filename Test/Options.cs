using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace RtUtil
{
    class Options
    {
        [Option('s', "server", HelpText = "RT server, such as rt.rhino.acme")]
        public string Server { get; set; }

        [Option('u', "user", HelpText = "User name")]
        public string User { get; set; }

        [Option('p', "password", HelpText = "User password")]
        public string Password { get; set; }

        [ValueOption(0)]
        public string Query { get; set; }


        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
