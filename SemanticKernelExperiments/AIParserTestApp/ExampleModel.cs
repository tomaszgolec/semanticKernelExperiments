using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIParserTestApp
{
    public class ExampleModel
    {
        [KernelFunction("create_examplemodel")]
        [Description("Create instnace of ExampleModel class")]
        [return: Description("An instance of ExampleModel class")]
        public ExampleModel CreateExampleModel (ExampleModel model)
        {
            return model;
        }


        public string User { get; set; }
        public string Name { get; set; }
        public Options Options { get; set; }
        public bool Active { get; set; }
    }

    public class Options
    {
        public string Language { get; set; }
        public string Theme { get; set; }
    }
}
