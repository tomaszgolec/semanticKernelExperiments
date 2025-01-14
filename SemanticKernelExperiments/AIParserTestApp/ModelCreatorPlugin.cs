using Microsoft.SemanticKernel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIParserTestApp
{
    public class  ModelCreatorPlugin<T>
    {
        [KernelFunction("create_object")]
        [Description("Create instnace of an object ")]
        [return: Description("An instance of object")]
        public T CreateExampleModel(T model)
        {
            return model;
        }
       
    }
}
